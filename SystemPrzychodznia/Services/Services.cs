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

        public UserService()
        {
            _repository = new UserRepository();
            _validator = new Validator(_repository);
        }

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
        /// Metoda przygotowująca surowe dane do walidacji, np. usuwająca zbędne spacje
        /// Nie obowiązuje w przypadku daty urodzenia, która jest pobierana z DateTimePicker, więc nie jest surowym stringiem, a już sformatowaną datą
        /// Nie obowiązuje również w przypadku płci, która jest pobierana z ComboBoxa, więc nie jest surowym stringiem, a już sformatowaną płcią
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

            // Walidacja formatu daty
            if (!DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime birthDate))
            {
                throw new ArgumentException("Nieprawidłowy format daty. Oczekiwany format: YYYY-MM-DD");
            }

            // Walidacja płci
            string genderLower = gender?.ToLower();
            if (genderLower != "m" && genderLower != "k")
            {
                throw new ArgumentException("Płeć musi być 'M', 'K'");
            }

            int year = birthDate.Year;
            int month = birthDate.Month;
            int day = birthDate.Day;

            // Modyfikacja miesiąca dla PESEL (dla różnych stuleci)
            if (year >= 1800 && year <= 1899)
            {
                month += 80;
            }
            else if (year >= 1900 && year <= 1999)
            {
                // month pozostaje bez zmian (0-12)
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

            // Pobranie dwóch ostatnich cyfr roku
            int yearTwoDigits = year % 100;

            // Przygotowanie pierwszych 9 cyfr PESEL
            string peselBase = $"{yearTwoDigits:D2}{month:D2}{day:D2}";

            // Dodanie liczby porządkowej (na razie 000) - w uproszczeniu
            // W rzeczywistości powinna to być unikalna liczba, tutaj używamy 000
            peselBase += $"{rnd.Next(0,10)}{rnd.Next(0, 10)}{rnd.Next(0, 10)}";

            // Określenie cyfry płci (przedostatnia cyfra w PESEL)
            // Dla kobiety parzysta, dla mężczyzny nieparzysta
            int genderDigit;
            if (genderLower == "m")
            {
                // Mężczyzna - cyfra nieparzysta (np. 1,3,5,7,9)
                genderDigit = rnd.Next(1, 6)*2 - 1;
            }
            else
            {
                // Kobieta - cyfra parzysta (np. 0,2,4,6,8)
                genderDigit = rnd.Next(0, 5) * 2;
            }

            // Dodanie cyfry płci jako przedostatniej cyfry
            peselBase += $"{genderDigit}";

            // Obliczenie sumy kontrolnej
            int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            int sum = 0;

            for (int i = 0; i < 10; i++)
            {
                sum += (int.Parse(peselBase.Substring(i, 1))) * weights[i];
            }

            int controlDigit = (10 - (sum % 10)) % 10;

            // Zbudowanie pełnego PESEL
            string pesel = peselBase + $"{controlDigit}";

            return pesel;
        }


    }
}
