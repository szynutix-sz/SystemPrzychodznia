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

        

        public List<User> GetListUsers(SearchTerms s) => _repository.GetList(s);

        public UserFull GetUserFull(int id) => _repository.GetUserFull(id);

        public void ForgetUser(int id) => _repository.ForgetUser(id);

        public List<string> GetUprawnienia() => _repository.GetUprawnienia();

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
    }
}
