using SystemPrzychodznia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

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

        public (bool success, string message, int userId) AttemptLogin(string login, string password)
        {
            var loginData = _repository.GetLoginData(login);

            if (loginData.Id == 0) return (false, "Nieprawidłowy login lub hasło.", 0);

            if (loginData.BlockedUntil.HasValue)
            {
                if (loginData.BlockedUntil.Value > DateTime.Now)
                {
                    var span = loginData.BlockedUntil.Value - DateTime.Now;
                    return (false, $"Konto zablokowane. Spróbuj ponownie za {(int)span.TotalMinutes} min.", 0);
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
                    _repository.UpdateBlockedUntil(loginData.Id, DateTime.Now.AddMinutes(30));
                    return (false, "Trzy nieudane próby! Konto zablokowane na 30 minut.", 0);
                }

                return (false, $"Błędne hasło. Pozostało prób: {3 - _failedAttempts[login]}.", 0);
            }

            _failedAttempts[login] = 0;
            return (true, "Zalogowano pomyślnie.", loginData.Id);
        }

        // --- ZMODYFIKOWANE ODZYSKIWANIE HASŁA (WG. WYMAGAŃ Z KARTY) ---
        public (bool success, string message) RecoverPassword(string login, string email)
        {
            var data = _repository.GetLoginData(login);

            // Wymagania 3 i 4: Jeśli nie ma usera lub email nie pasuje, ten sam generyczny komunikat
            if (data.Id == 0 || !string.Equals(data.Email, email, StringComparison.OrdinalIgnoreCase))
            {
                return (false, "Podano błędny identyfikator użytkownika i/lub adres e-mail.");
            }

            try
            {
                // Generowanie losowego hasła (Wymaganie 2)
                string newPassword = GenerateRandomPassword(10);

                // Zapisz nowe hasło w bazie
                _repository.SaveNewPassword(data.Id, newPassword);

                // Wyślij e-mail z nowym hasłem
                SendEmail(data.Email, newPassword);

                return (true, "Nowe hasło zostało wygenerowane przez system i wysłane na podany adres e-mail.");
            }
            catch (Exception ex)
            {
                return (false, $"Błąd podczas wysyłania komunikacji sieciowej: {ex.Message}");
            }
        }

        // Generator bezpiecznych haseł
        private string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
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
                Body = $"Witaj,\n\nZgodnie z prośbą, system wygenerował dla Ciebie nowe hasło.\n\nTwoje nowe hasło to: {userPassword}\n\nPozdrawiamy,\nAdministratorzy Systemu",
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

        public List<ForgottenUser> GetListForgottenUsers() => _repository.GetListForgottenUsers();
        public List<User> GetListUsers(SearchTerms s) => _repository.GetListUsers(s);
        public UserFull GetUserFull(int id) => _repository.GetUserFull(id);

        public ValidationResult ForgetUser(UserFull user, int forgottenBy)
        {
            ForgottenUser f = user.Forget(forgottenBy);
            var validation = _validator.ValidateUserFull(user, true);

            if (!validation.IsValid) return validation;

            _repository.ForgetUser(f, f.ForgottenBy);
            return new ValidationResult(true);
        }

        public List<Uprawnienie> GetUprawnienia() => _repository.GetUprawnienia();

        public List<User> GetUsersByRole(int roleId)
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