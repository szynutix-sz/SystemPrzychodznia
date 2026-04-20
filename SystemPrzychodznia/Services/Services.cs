using SystemPrzychodznia.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemPrzychodznia.Services
{

    public class UserService
    {
        private readonly UserRepository _repository;

        private readonly Validator _validator;

        // --- NOWY SŁOWNIK DO LICZENIA PRÓB ---
        private Dictionary<string, int> _failedAttempts = new Dictionary<string, int>();

        public UserService()
        {
            _repository = new UserRepository();
            _validator = new Validator(_repository);
        }

        // --- NOWE METODY DO LOGOWANIA I ODZYSKIWANIA HASŁA ---
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

        public (bool success, string message) RecoverPassword(string login)
        {
            var data = _repository.GetLoginData(login);
            if (data.Id == 0) return (true, "Jeśli login istnieje, wysłaliśmy instrukcje na email.");
            return (true, $"Hasło do konta zostało wysłane na przypisany adres:\n{data.Email}");
        }
        // -----------------------------------------------------

        public ValidationResult ValidateUserFull(UserFull user, bool editing) => _validator.ValidateUserFull(user, editing);
        public ValidationResult AddUser(UserFull user)
        {
            var validation = ValidateUserFull(user, false);
            if (!validation.IsValid)
            {
                return validation;
            }

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

            if (!validation.IsValid)
            {
                return validation;
            }

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

            if (!validation.IsValid)
            {
                return validation;
            }

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
            userBeforeValid.Street = user.Street.Trim();
            userBeforeValid.PropertyNumber = user.PropertyNumber.Trim();
            userBeforeValid.HouseUnitNumber = user.HouseUnitNumber.Trim();
            userBeforeValid.PESEL = user.PESEL.Trim();
            userBeforeValid.Email = user.Email.Trim();
            userBeforeValid.Phone = user.Phone.Trim();

            return userBeforeValid;
        }

        public static string PESELfromData(string date, string gender)
        {
            Random rnd = new Random();

            if (!DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime birthDate))
            {
                throw new ArgumentException("Nieprawidłowy format daty. Oczekiwany format: YYYY-MM-DD");
            }

            string genderLower = gender?.ToLower();
            if (genderLower != "m" && genderLower != "k")
            {
                throw new ArgumentException("Płeć musi być 'M', 'K'");
            }

            int year = birthDate.Year;
            int month = birthDate.Month;
            int day = birthDate.Day;

            if (year >= 1800 && year <= 1899)
            {
                month += 80;
            }
            else if (year >= 1900 && year <= 1999)
            {

            }
            else if (year >= 2000 && year <= 2099)
            {
                month += 20;
            }
            else if (year >= 2100 && year <= 2199)
            {
                month += 40;
            }
            else if (year >= 2200 && year <= 2299)
            {
                month += 60;
            }

            int yearTwoDigits = year % 100;

            string peselBase = $"{yearTwoDigits:D2}{month:D2}{day:D2}";

            peselBase += $"{rnd.Next(0, 10)}{rnd.Next(0, 10)}{rnd.Next(0, 10)}";

            int genderDigit;
            if (genderLower == "m")
            {
                genderDigit = rnd.Next(1, 6) * 2 - 1;
            }
            else
            {
                genderDigit = rnd.Next(0, 5) * 2;
            }

            peselBase += $"{genderDigit}";

            int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            int sum = 0;

            for (int i = 0; i < 10; i++)
            {
                sum += (int.Parse(peselBase.Substring(i, 1))) * weights[i];
            }

            int controlDigit = (10 - (sum % 10)) % 10;

            string pesel = peselBase + $"{controlDigit}";

            return pesel;
        }


    }
}