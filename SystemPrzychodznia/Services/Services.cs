using SystemPrzychodznia.Data;
using System.Collections.Generic;
using System.Linq;

namespace SystemPrzychodznia.Services
{
    public class UserService
    {
        private readonly UserRepository _repository = new UserRepository();

        public bool AddUser(UserFull user)
        {
            // Podstawowa walidacja
            if (string.IsNullOrWhiteSpace(user.Login) ||
                string.IsNullOrWhiteSpace(user.FirstName) ||
                string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.PESEL))
            {
                return false; // lub rzuć wyjątkiem
            }

            // Sprawdzenie unikalności email (można dodać metodę GetByEmail w repozytorium)
            // var existing = _repository.GetByEmail(user.Email);
            // if (existing != null) return false;

            _repository.Add(user);
            return true;
        }

        public List<User> GetListUsers() => _repository.GetList();


    }
}
