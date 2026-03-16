using SystemPrzychodznia.Data;
using System.Collections.Generic;
using System.Linq;

namespace SystemPrzychodznia.Services
{
    public class ValidationResult
    {
        private bool result;
        private string mess;
        private int code;
        public ValidationResult(bool result, int code, string mess) 
        {
            this.result = result;
            this.code = code;
            this.mess = mess;
            
        }
        public bool getResult()
        {
            return result;
        }

        public string getMessage()
        {
            return mess;
        }

        public int getCode()
        {
            return code;
        }
    }

    public class UserService
    {
        private readonly UserRepository _repository = new UserRepository();

        private ValidationResult valSuc = new ValidationResult(
            true,
            1,
            "Poprawna walidacja"
            );

        private ValidationResult valFail = new ValidationResult(
            false,
            2,
            "Niepoprawna walidacja"
            );

        public ValidationResult AddUser(UserFull user)
        {
            // Podstawowa walidacja
            if (string.IsNullOrWhiteSpace(user.Login) ||
                string.IsNullOrWhiteSpace(user.FirstName) ||
                string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.PESEL))
            {
                return valFail; // lub rzuć wyjątkiem
            }

            // Sprawdzenie unikalności email (można dodać metodę GetByEmail w repozytorium)
            // var existing = _repository.GetByEmail(user.Email);
            // if (existing != null) return false;

            _repository.Add(user);
            return valSuc;
        }



        public List<User> GetListUsers() => _repository.GetList();


    }
}
