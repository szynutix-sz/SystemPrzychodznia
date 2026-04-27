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
                return DateTime.Now; // Awaryjny powrót w razie braku internetu
            }
        }

        // Zwracamy też requiresPasswordChange w wyniku logowania
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
                    // Pobranie czasu od admina (z bazy)
                    int lockoutMinutes = _repository.GetLockoutDurationMinutes();
                    _repository.UpdateBlockedUntil(loginData.Id, realTimeNow.AddMinutes(lockoutMinutes));
                    return (false, $"Trzy nieudane próby! Konto zablokowane na {lockoutMinutes} minut.", 0, false);
                }

                return (false, $"Błędne hasło. Pozostało prób: {3 - _failedAttempts[login]}.", 0, false);
            }

            _failedAttempts[login] = 0;
            return (true, "Zalogowano pomyślnie.", loginData.Id, loginData.RequiresPasswordChange);
        }

        // --- ZMIANA: ZAWSZE TEN SAM KOMUNIKAT DLA BEZPIECZEŃSTWA (ANTI-ENUMERATION) ---
        public (bool success, string message) RecoverPassword(string login, string email)
        {
            var data = _repository.GetLoginData(login);
            string genericSuccessMsg = "Jeśli podano prawidłowe dane, nowe hasło zostało wygenerowane i wysłane na powiązany adres e-mail.";

            if (data.Id == 0 || !string.Equals(data.Email, email, StringComparison.OrdinalIgnoreCase))
            {
                return (true, genericSuccessMsg); // Udajemy sukces!
            }

            try
            {
                string newPassword = GenerateRandomPassword(10);
                _repository.SaveNewPassword(data.Id, newPassword);
                SendEmail(data.Email, newPassword);

                return (true, genericSuccessMsg);
            }
            catch (Exception ex)
            {
                return (false, $"Błąd komunikacji: {ex.Message}");
            }
        }

        public ValidationResult ChangeUserPassword(int userId, string newPassword)
        {

            ValidationResult validation = new ValidationResult(false);

            validation.Errors = _validator.CzyPoprawneHaslo(userId ,newPassword);

            if (validation.Errors.Count != 0)
            {
                validation.IsValid = false;
                return validation;
            }



            _repository.ChangeUserPassword(userId, newPassword);
            return new ValidationResult(true);


            
        }

        private string GenerateRandomPassword(int length)
        {
            if (length < 5) throw new ArgumentException("Password length must be at least 5 characters.");
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%";
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "!@#$%";
            var random = new Random();
            string the_rest = new string(Enumerable.Repeat(chars, length-4).Select(s => s[random.Next(s.Length)]).ToArray());
            char up = upper[random.Next(upper.Length)];
            char low = lower[random.Next(lower.Length)];
            char dig = digits[random.Next(digits.Length)];
            char spec = special[random.Next(special.Length)];
            return the_rest + up + low + dig + spec;
        }

        public List<string> GetUserPasswordHistory(int userId)
        {

            return _repository.GetUserPasswordHistory(userId);
        }


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

        public ValidationResult AddUser(UserFull user)
        {
            var validation = ValidateUserFull(user, false);
            if (!validation.IsValid) return validation;

            _repository.Add(user);
            return new ValidationResult(true);
        }

        public List<ForgottenUser> GetListForgottenUsers() => _repository.GetListForgottenUsers(new SearchTermsForgotten());
        public List<ForgottenUser> GetListForgottenUsers(SearchTermsForgotten searchTerms) => _repository.GetListForgottenUsers(searchTerms);
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

        public List<UserBasic> GetUsersByRole(int roleId)
        {
            var s = new SearchTerms();
            foreach (var up in _repository.GetUprawnienia())
                s.Uprawnienia.Add(new Uprawnienie { Id = up.Id, Nazwa = up.Nazwa, Posiadane = up.Id == roleId ? true : (bool?)null });
            return _repository.GetListUsers(s);
        }

        public ValidationResult EditUser(UserFull user)
        {
            var validation = _validator.ValidateUserFull(user, true);
            if (!validation.IsValid) return validation;

            _repository.EditUser(user);
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

            string genderLower = gender?.ToLower();
            int year = birthDate.Year;
            int month = birthDate.Month;
            int day = birthDate.Day;

            if (year >= 1800 && year <= 1899) month += 80;
            else if (year >= 2000 && year <= 2099) month += 20;
            else if (year >= 2100 && year <= 2199) month += 40;
            else if (year >= 2200 && year <= 2299) month += 60;

            string peselBase = $"{(year % 100):D2}{month:D2}{day:D2}";
            peselBase += $"{rnd.Next(0, 10)}{rnd.Next(0, 10)}{rnd.Next(0, 10)}";
            int genderDigit = (genderLower == "m") ? (rnd.Next(1, 6) * 2 - 1) : (rnd.Next(0, 5) * 2);
            peselBase += $"{genderDigit}";

            int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            int sum = 0;
            for (int i = 0; i < 10; i++) sum += (int.Parse(peselBase.Substring(i, 1))) * weights[i];

            int controlDigit = (10 - (sum % 10)) % 10;
            return peselBase + $"{controlDigit}";
        }
    }
}
