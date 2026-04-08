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

        public ValidationResult ValidateUserFull(UserFull user) => _validator.ValidateUserFull(user);
        public ValidationResult AddUser(UserFull user)
                {
                    var validation = ValidateUserFull(user);
                    if (!validation.IsValid)
                    {
                        return validation;
                    }

                    _repository.Add(user);
                    return new ValidationResult(true);
                }

        

        public List<User> GetListUsers(SearchTerms s) => _repository.GetList(s);

        public UserFull GetUserFull(String s_Login) => _repository.GetUserFull(s_Login);

        public List<string> GetUprawnienia() => _repository.GetUprawnienia();

        public ValidationResult EditUser(UserFull user)
        {
            var validation = _validator.ValidateUserFull(user, user.Id);

            if (!validation.IsValid)
            {
                return validation;
            }

            _repository.EditUser(user);
            return new ValidationResult(true);

        }

        public List<string> GetUserUprawnienia() => _repository.GetUprawnienia();

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
        public List<User> GetUsersByRole(string roleName) => _repository.GetUsersByRole(roleName);
        public List<User> GetUsersByMultipleRoles(List<string> roleNames) => _repository.GetUsersByMultipleRoles(roleNames);

        public List<ForgottenUser> GetForgottenUsers(string searchId = null) => _repository.GetForgottenUsers(searchId);

        public void ForgetSystemUser(int userIdToForget, int currentAdminId)
        {
            //Losowy ciąg znaków (imie, nazwisko)
            string randomString = Guid.NewGuid().ToString("N").Substring(0, 10);

            // Data i zgodny, ale losowy PESEL aby przechodzilo przez walidator
            // Ustawiamy sztywną datę urodzenia: 1900-01-01
            string fakeDate = "1900-01-01";

            // Pesel musi zaczynać się od 000101 (zgodnie z metodą WyciagnijDateZPesel w Validator.cs)
            // Pozostałe 5 cyfr generujemy losowo, aby zachować wymóg pola UNIQUE w bazie
            Random rng = new Random();
            string randomSuffix = rng.Next(10000, 99999).ToString();
            string fakePesel = "000101" + randomSuffix;

            _repository.ForgetSystemUser(userIdToForget, currentAdminId, randomString, fakePesel, fakeDate);
        }
    }
}
