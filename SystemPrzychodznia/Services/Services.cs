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

        public void ForgetUser(int id) => _repository.ForgetUser(id);

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
    }
}
