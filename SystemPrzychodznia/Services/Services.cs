using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using SystemPrzychodznia.Data;

namespace SystemPrzychodznia.Services
{
    public class UserService
    {
        private readonly UserRepository _repository;
        private readonly Validator _validator;
        private Dictionary<string, int> _failedAttempts = new Dictionary<string, int>();

        public UserService()
        {
            _repository = new UserRepository();
            _validator = new Validator(_repository);
        }

        // --- POBIERANIE CZASU Z NTP (OCHRONA PRZED LOKALNĄ ZMIANĄ ZEGARA) ---
        private DateTime GetRealTime()
        {
            try
            {
                var addresses = System.Net.Dns.GetHostEntry("time.google.com").AddressList;
                var ipEndPoint = new System.Net.IPEndPoint(addresses[0], 123);
                var ntpData = new byte[48]; ntpData[0] = 0x1B;

                using (var socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp))
                {
                    socket.Connect(ipEndPoint);
                    socket.ReceiveTimeout = 2000;
                    socket.Send(ntpData);
                    socket.Receive(ntpData);
                }
                ulong intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | (ulong)ntpData[43];
                ulong fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | (ulong)ntpData[47];
                var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
                return (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds).ToLocalTime();
            }
            catch
            {
                return DateTime.Now;
            }
        }

        public (bool success, string message, int userId, bool requiresPasswordChange) AttemptLogin(string login, string password)
        {
            var loginData = _repository.GetLoginData(login);
            if (loginData.Id == 0) return (false, "Nieprawidłowy login lub hasło.", 0, false);

            DateTime realTimeNow = GetRealTime();

            if (loginData.BlockedUntil.HasValue)
            {
                if (loginData.BlockedUntil.Value > realTimeNow)
                {
                    var span = loginData.BlockedUntil.Value - realTimeNow;
                    return (false, $"Konto zablokowane. Spróbuj ponownie za {(int)span.TotalMinutes + 1} min.", 0, false);
                }
                else
                {
                    _repository.UpdateBlockedUntil(loginData.Id, null);
                    if (_failedAttempts.ContainsKey(login)) _failedAttempts[login] = 0;
                }
            }

            if (loginData.Password != password)
            {
                if (!_failedAttempts.ContainsKey(login)) _failedAttempts[login] = 0;
                _failedAttempts[login]++;

                if (_failedAttempts[login] >= 3)
                {
                    int lockoutMinutes = _repository.GetLockoutDurationMinutes();
                    _repository.UpdateBlockedUntil(loginData.Id, realTimeNow.AddMinutes(lockoutMinutes));
                    return (false, $"Trzy nieudane próby! Konto zablokowane na {lockoutMinutes} minut.", 0, false);
                }
                return (false, $"Błędne hasło. Pozostało prób: {3 - _failedAttempts[login]}.", 0, false);
            }

            _failedAttempts[login] = 0;
            return (true, "Zalogowano pomyślnie.", loginData.Id, loginData.RequiresPasswordChange);
        }

        public (bool success, string message) RecoverPassword(string login, string email)
        {
            var data = _repository.GetLoginData(login);
            string genericSuccessMsg = "Jeśli podano prawidłowe dane, nowe hasło zostało wygenerowane i wysłane na powiązany adres e-mail.";

            if (data.Id == 0 || !string.Equals(data.Email, email, StringComparison.OrdinalIgnoreCase)) return (true, genericSuccessMsg);

            try
            {
                string newPassword = GenerateRandomPassword();
                _repository.SaveNewPassword(data.Id, newPassword);
                SendEmail(data.Email, newPassword);
                return (true, genericSuccessMsg);
            }
            catch (Exception ex) { return (false, $"Błąd komunikacji: {ex.Message}"); }
        }

        public ValidationResult ChangeUserPassword(int userId, string newPassword)
        {
            ValidationResult validation = new ValidationResult(false);
            validation.Errors = _validator.CzyPoprawneHaslo(userId, newPassword);

            if (validation.Errors.Count != 0)
            {
                validation.IsValid = false;
                return validation;
            }

            _repository.ChangeUserPassword(userId, newPassword);
            return new ValidationResult(true);
        }

        private string GenerateRandomPassword()
        {
            const string upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            const string lower = "abcdefghijkmnopqrstuvwxyz";
            const string digits = "123456789";
            const string special = "-_!*#$&";
            var random = new Random();
            char[] array = new char[] {
                upper[random.Next(upper.Length)], upper[random.Next(upper.Length)], upper[random.Next(upper.Length)],
                lower[random.Next(lower.Length)], lower[random.Next(lower.Length)], lower[random.Next(lower.Length)],
                digits[random.Next(digits.Length)], digits[random.Next(digits.Length)],
                special[random.Next(special.Length)], special[random.Next(special.Length)]
            };
            random.Shuffle(array);
            return new string(array);
        }

        public List<string> GetUserPasswordHistory(int userId) => _repository.GetUserPasswordHistory(userId);

        private void SendEmail(string targetEmail, string userPassword)
        {
            string senderEmail = "logan.harrisems@gmail.com";
            string senderAppPassword = "mblhaamtwfcrhoso";

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderAppPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, "System Przychodnia"),
                Subject = "Nowe hasło - System Przychodnia",
                Body = $"Witaj,\n\nZgodnie z prośbą, system wygenerował dla Ciebie nowe hasło.\n\nTwoje nowe tymczasowe hasło to: {userPassword}\n\nPo zalogowaniu system poprosi Cię o ustalenie własnego hasła.\n\nPozdrawiamy,\nAdministratorzy Systemu",
                IsBodyHtml = false,
            };
            mailMessage.To.Add(targetEmail);
            smtpClient.Send(mailMessage);
        }

        public ValidationResult ValidateUserFull(UserFull user, bool editing) => _validator.ValidateUserFull(user, editing);
        public ValidationResult ValidatePatientFull(PatientFull patient, bool editing) => _validator.ValidatePatientFull(patient, editing);

        public ValidationResult AddUser(UserFull user)
        {
            var validation = ValidateUserFull(user, false);
            if (!validation.IsValid) return validation;
            _repository.Add(user);
            return new ValidationResult(true);
        }

        public List<ForgottenUser> GetListForgottenUsers() => _repository.GetListForgottenUsers(new SearchTermsForgotten());
        public List<ForgottenUser> GetListForgottenUsers(SearchTermsForgotten searchTerms) => _repository.GetListForgottenUsers(searchTerms);
        public List<PatientListItem> GetListPatients(SearchTermsPatient searchTerms) => _repository.GetListPatients(searchTerms);
        public PatientFull? GetPatientFull(int id) => _repository.GetPatientFull(id);
        public List<UserBasic> GetListUsers(SearchTerms s) => _repository.GetListUsers(s);
        public UserFull GetUserFull(int id) => _repository.GetUserFull(id);

        public ValidationResult ForgetUser(UserFull user, int forgottenBy)
        {
            ForgottenUser f = user.Forget(forgottenBy);
            var validation = _validator.ValidateUserFull(user, true, validatePassword: false);
            if (!validation.IsValid) return validation;
            _repository.ForgetUser(f, f.ForgottenBy);
            return new ValidationResult(true);
        }

        public List<Uprawnienie> GetUprawnienia() => _repository.GetUprawnienia();

        public List<UserBasic> GetUsersByRole(string roleName)
        {
            var s = new SearchTerms();
            foreach (var up in _repository.GetUprawnienia())
                s.Uprawnienia.Add(new Uprawnienie
                {
                    Id = up.Id,
                    Nazwa = up.Nazwa,
                    Posiadane = string.Equals(up.Nazwa, roleName, StringComparison.OrdinalIgnoreCase) ? true : (bool?)null
                });
            return _repository.GetListUsers(s);
        }

        public ValidationResult EditUser(UserFull user)
        {
            var validation = _validator.ValidateUserFull(user, true);
            if (!validation.IsValid) return validation;
            _repository.EditUser(user);
            return new ValidationResult(true);
        }

        public ValidationResult RegisterPatient(PatientFull patient)
        {
            var preparedPatient = PreparePatientForSave(patient);
            var validation = _validator.ValidatePatientFull(preparedPatient, false);
            if (!validation.IsValid) return validation;
            _repository.AddPatient(preparedPatient);
            patient.Id = preparedPatient.Id;
            return new ValidationResult(true);
        }

        public ValidationResult EditPatient(PatientFull patient)
        {
            var preparedPatient = PreparePatientForSave(patient);
            var validation = _validator.ValidatePatientFull(preparedPatient, true);
            if (!validation.IsValid) return validation;
            _repository.EditPatient(preparedPatient);
            return new ValidationResult(true);
        }

        public UserFull PrepareRawStrings(UserFull user)
        {
            UserFull userBeforeValid = new UserFull();
            userBeforeValid.Login = user.Login.Trim();
            userBeforeValid.FirstName = user.FirstName.Trim();
            userBeforeValid.LastName = user.LastName.Trim();
            userBeforeValid.Locality = user.Locality.Trim();
            userBeforeValid.PostalCode = user.PostalCode.Trim();
            userBeforeValid.Street = user.Street?.Trim() ?? "";
            userBeforeValid.PropertyNumber = user.PropertyNumber.Trim();
            userBeforeValid.HouseUnitNumber = user.HouseUnitNumber?.Trim() ?? "";
            userBeforeValid.PESEL = user.PESEL.Trim();
            userBeforeValid.Email = user.Email.Trim();
            userBeforeValid.Phone = user.Phone.Trim();
            return userBeforeValid;
        }

        public static string PESELfromData(string date, string gender)
        {
            Random rnd = new Random();
            if (!DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime birthDate))
                throw new ArgumentException("Nieprawidłowy format daty.");

            int year = birthDate.Year;
            int month = birthDate.Month;
            int day = birthDate.Day;

            if (year >= 1800 && year <= 1899) month += 80;
            else if (year >= 2000 && year <= 2099) month += 20;
            else if (year >= 2100 && year <= 2199) month += 40;
            else if (year >= 2200 && year <= 2299) month += 60;

            string peselBase = $"{(year % 100):D2}{month:D2}{day:D2}{rnd.Next(0, 10)}{rnd.Next(0, 10)}{rnd.Next(0, 10)}";
            int genderDigit = (gender?.ToLower() == "m") ? (rnd.Next(1, 6) * 2 - 1) : (rnd.Next(0, 5) * 2);
            peselBase += $"{genderDigit}";

            int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            int sum = 0;
            for (int i = 0; i < 10; i++) sum += (int.Parse(peselBase.Substring(i, 1))) * weights[i];

            int controlDigit = (10 - (sum % 10)) % 10;
            return peselBase + $"{controlDigit}";
        }

        private PatientFull PreparePatientForSave(PatientFull patient)
        {
            return new PatientFull
            {
                Id = patient.Id,
                FirstName = patient.FirstName.Trim(),
                LastName = patient.LastName.Trim(),
                Locality = patient.Locality.Trim(),
                PostalCode = patient.PostalCode.Trim().Replace("-", ""),
                Street = string.IsNullOrWhiteSpace(patient.Street) ? string.Empty : patient.Street.Trim(),
                PropertyNumber = patient.PropertyNumber.Trim(),
                HouseUnitNumber = string.IsNullOrWhiteSpace(patient.HouseUnitNumber) ? string.Empty : patient.HouseUnitNumber.Trim(),
                PESEL = patient.PESEL.Trim(),
                BirthDate = patient.BirthDate.Trim(),
                Gender = patient.Gender.Trim(),
                Email = string.IsNullOrWhiteSpace(patient.Email) ? string.Empty : patient.Email.Trim(),
                Phone = patient.Phone.Trim()
            };
        }

        // ============================================================
        // GABINETY I SPECJALIZACJE ORAZ LEKARZE Z DANEJ SPECJALIZACJI
        // ============================================================
        public List<Gabinet> GetGabinety() => _repository.GetGabinety();
        public List<Specjalizacja> GetSpecjalizacje() => _repository.GetSpecjalizacje();
        public List<UserBasic> GetLekarzeBySpecjalizacja(int idSpecjalizacji) => _repository.GetLekarzeBySpecjalizacja(idSpecjalizacji);

        public (bool success, string message) AddGabinet(string nazwa)
        {
            if (string.IsNullOrWhiteSpace(nazwa)) return (false, "Nazwa gabinetu nie może być pusta.");
            try { _repository.AddGabinet(nazwa.Trim()); return (true, "Dodano gabinet."); }
            catch (Exception ex) { return (false, $"Błąd bazy: Nazwa prawdopodobnie już istnieje. ({ex.Message})"); }
        }

        public (bool success, string message) EditGabinet(int id, string nowaNazwa)
        {
            if (string.IsNullOrWhiteSpace(nowaNazwa)) return (false, "Nazwa nie może być pusta.");
            try { _repository.EditGabinet(id, nowaNazwa.Trim()); return (true, "Zaktualizowano gabinet."); }
            catch (Exception) { return (false, "Błąd aktualizacji."); }
        }

        public (bool success, string message) AddSpecjalizacja(string nazwa)
        {
            if (string.IsNullOrWhiteSpace(nazwa)) return (false, "Nazwa specjalizacji nie może być pusta.");
            try { _repository.AddSpecjalizacja(nazwa.Trim()); return (true, "Dodano specjalizację."); }
            catch (Exception) { return (false, "Taka specjalizacja prawdopodobnie już istnieje."); }
        }

        // ============================================================
        // WIZYTY Z KOLIZJAMI I FILTROWANIEM
        // ============================================================
        public List<Wizyta> GetWizyty(SearchTermsWizyta s = null) => _repository.GetWizyty(s);

        public (bool success, string message) AddWizyta(int pacjentId, int lekarzId, int gabinetId, DateTime data)
        {
            if (data < DateTime.Now) return (false, "Nie można zaplanować wizyty w przeszłości.");

            // Pobranie dokładnego czasu kolidującej wizyty
            string collisionTime = _repository.GetWizytaCollisionTime(lekarzId, gabinetId, data);
            if (collisionTime != null)
            {
                return (false, $"Kolizja terminu! Wybrany lekarz lub gabinet jest już zajęty przez wizytę zaplanowaną na godzinę: {collisionTime}.");
            }

            var wizyta = new Wizyta { IdPacjenta = pacjentId, IdLekarza = lekarzId, IdGabinetu = gabinetId, DataRozpoczecia = data, Status = "Zarejestrowana" };

            try
            {
                _repository.AddWizyta(wizyta);
                return (true, "Wizyta została pomyślnie zarejestrowana.");
            }
            catch (Exception ex) { return (false, $"Błąd podczas dodawania wizyty: {ex.Message}"); }
        }

        public void ZmienStatusWizyty(int wizytaId, string status, string schorzenia = null, string zalecenia = null)
        {
            _repository.UpdateWizytaStatus(wizytaId, status, schorzenia, zalecenia);
        }

        public (bool success, string message) EditWizyta(Wizyta wizyta)
        {
            if (wizyta.IdPacjenta <= 0 || wizyta.IdLekarza <= 0 || wizyta.IdGabinetu <= 0)
                return (false, "Wizyta musi mieć przypisanego pacjenta, lekarza oraz gabinet.");

            // Pobranie dokładnego czasu kolidującej wizyty podczas edycji
            string collisionTime = _repository.GetWizytaCollisionTime(wizyta.IdLekarza, wizyta.IdGabinetu, wizyta.DataRozpoczecia, wizyta.Id);
            if (collisionTime != null)
            {
                return (false, $"Kolizja terminu! Wybrany lekarz lub gabinet jest już zajęty przez wizytę zaplanowaną na godzinę: {collisionTime}.");
            }

            try
            {
                _repository.FullEditWizyta(wizyta);
                return (true, "Wizyta została pomyślnie zaktualizowana.");
            }
            catch (Exception ex) { return (false, $"Błąd aktualizacji: {ex.Message}"); }
        }

        public (bool success, string message) DeleteWizyta(int wizytaId)
        {
            try
            {
                _repository.DeleteWizyta(wizytaId);
                return (true, "Wizyta została usunięta z systemu.");
            }
            catch (Exception ex) { return (false, $"Błąd podczas usuwania: {ex.Message}"); }
        }
    }
}