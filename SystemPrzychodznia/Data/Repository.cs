using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Logging;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace SystemPrzychodznia.Data
{

    internal class UserRepository
    {
        private readonly string _connectionString = "Data Source=przychodnia.db";

        public void Add(UserFull user)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Users (Login, FirstName, LastName, Locality, PostalCode, Street, PropertyNumber, HouseUnitNumber, PESEL,BirthDate, Gender,     Email                               , Phone      , Password)
                    VALUES ($login ,$firstName, $lastName, $locality, $postalCode, $street, $propertyNumber, $houseUnitNumber, $pesel, $birthDate, $gender , $email, $phone, $password);";
                command.Parameters.AddWithValue("$login", user.Login);
                command.Parameters.AddWithValue("$firstName", user.FirstName);
                command.Parameters.AddWithValue("$lastName", user.LastName);
                command.Parameters.AddWithValue("$locality", user.Locality);
                command.Parameters.AddWithValue("$postalCode", user.PostalCode);
                command.Parameters.AddWithValue("$street", user.Street ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("$propertyNumber", user.PropertyNumber);
                command.Parameters.AddWithValue("$houseUnitNumber", user.HouseUnitNumber ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("$pesel", user.PESEL);
                command.Parameters.AddWithValue("$birthDate", user.BirthDate);
                command.Parameters.AddWithValue("$gender", user.Gender);
                command.Parameters.AddWithValue("$email", user.Email);
                command.Parameters.AddWithValue("$phone", user.Phone);
                command.Parameters.AddWithValue("$password", user.Password);
            
                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<User> GetList(SearchTerms s)
        {
            var users = new List<User>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Login, FirstName, LastName, Email, PESEL FROM Users
                WHERE Status = 'A' AND
                Login LIKE '%' || $login || '%' AND
                FirstName LIKE '%' || $firstName || '%' AND
                LastName LIKE '%' || $lastName || '%' AND
                Email LIKE '%' || $email || '%' AND
                PESEL LIKE '%' || $pesel || '%';";

            command.Parameters.AddWithValue("$login", s.Login);
            command.Parameters.AddWithValue("$firstName", s.FirstName);
            command.Parameters.AddWithValue("$lastName", s.LastName);
            command.Parameters.AddWithValue("$pesel", s.PESEL);
            command.Parameters.AddWithValue("$email", s.Email);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new User
                {
                    Login = reader.GetString(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    PESEL = reader.GetString(4),
                });
            }
            return users;
        }

        public void  EditUser(UserFull user)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE Users
                    SET Login = $login,
                        FirstName = $firstName,
                        LastName = $lastName,
                        Locality = $locality,
                        PostalCode = $postalCode,
                        Street = $street,
                        PropertyNumber = $propertyNumber,
                        HouseUnitNumber = $houseUnitNumber,
                        PESEL = $pesel,
                        BirthDate = $birthDate,
                        Gender = $gender,
                        Email = $email,
                        Phone = $phone
                    WHERE Id = $id;";


                command.Parameters.AddWithValue("$id", user.Id);
                command.Parameters.AddWithValue("$login", user.Login);
                command.Parameters.AddWithValue("$firstName", user.FirstName);
                command.Parameters.AddWithValue("$lastName", user.LastName);
                command.Parameters.AddWithValue("$locality", user.Locality);
                command.Parameters.AddWithValue("$postalCode", user.PostalCode);
                command.Parameters.AddWithValue("$street", user.Street ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("$propertyNumber", user.PropertyNumber);
                command.Parameters.AddWithValue("$houseUnitNumber", user.HouseUnitNumber ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("$pesel", user.PESEL);
                command.Parameters.AddWithValue("$birthDate", user.BirthDate);
                command.Parameters.AddWithValue("$gender", user.Gender);
                command.Parameters.AddWithValue("$email", user.Email);
                command.Parameters.AddWithValue("$phone", user.Phone);

                command.ExecuteNonQuery();

            }
            catch (SqliteException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public UserFull GetUserFull(string S_Login)
        {
            var users = new List<UserFull>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, Login, FirstName, LastName, Locality, PostalCode, Street, PropertyNumber, HouseUnitNumber, PESEL,BirthDate, Gender, Email, Phone
                FROM Users
                WHERE Status = 'A' AND
                Login = $login;";


            command.Parameters.AddWithValue("$login", S_Login);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new UserFull
                {
                    Id = reader.GetInt32(0),

                    Login = reader.GetString(1),
                    FirstName = reader.GetString(2),
                    LastName = reader.GetString(3),

                    Locality = reader.GetString(4),
                    PostalCode = reader.GetString(5),
                    Street = reader.IsDBNull(6) ? null : reader.GetString(6),
                    PropertyNumber = reader.GetString(7),
                    HouseUnitNumber = reader.IsDBNull(8) ? null : reader.GetString(8),

                    PESEL = reader.GetString(9),

                    BirthDate = reader.GetString(10),
                    Gender = reader.GetString(11),
                    Email = reader.GetString(12),
                    Phone = reader.GetString(13),

                });
            }
            return users[0];
        }

        public bool CzyIstniejeLogin(string login)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM Users WHERE Login = $login;";
            command.Parameters.AddWithValue("$login", login);
            long count = (long)command.ExecuteScalar();
            return count > 0;
        }

        public bool CzyIstniejeEmail(string email)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM Users WHERE Email = $email;";
            command.Parameters.AddWithValue("$email", email);
            long count = (long)command.ExecuteScalar();
            return count > 0;
        }

        public bool CzyIstnieje_PESEL(string pesel)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM Users WHERE PESEL = $pesel;";
            command.Parameters.AddWithValue("$pesel", pesel);
            long count = (long)command.ExecuteScalar();
            return count > 0;
        }
        public List<User> GetUsersByRole(string roleName)
        {
            var users = new List<User>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT u.Login, u.FirstName, u.LastName, u.Email, u.PESEL 
                FROM Users u
                JOIN UserRoles ur ON u.Id = ur.UserId
                JOIN Roles r ON ur.RoleId = r.Id
                WHERE u.Status = 'A' AND r.Name = $roleName;";

            command.Parameters.AddWithValue("$roleName", roleName);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new User
                {
                    Login = reader.GetString(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    PESEL = reader.GetString(4)
                });
            }
            return users;
        }

        public void ForgetSystemUser(int userId, int adminId, string randomString, string fakePesel, string fakeDate)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                //Zmiana danych na losowe, ustawienie flagi RODO i zmiana statusu na 'F' (Forgotten)
                var cmdUser = connection.CreateCommand();
                cmdUser.Transaction = transaction;
                cmdUser.CommandText = @"
                    UPDATE Users 
                    SET FirstName = $rand,
                        LastName = $rand,
                        PESEL = $pesel,
                        BirthDate = $date,
                        Gender = 'M',
                        Status = 'F',
                        ForgottenDate = CURRENT_TIMESTAMP,
                        ForgottenBy = $adminId
                    WHERE Id = $userId;";

                cmdUser.Parameters.AddWithValue("$rand", randomString);
                cmdUser.Parameters.AddWithValue("$pesel", fakePesel);
                cmdUser.Parameters.AddWithValue("$date", fakeDate);
                cmdUser.Parameters.AddWithValue("$adminId", adminId);
                cmdUser.Parameters.AddWithValue("$userId", userId);
                cmdUser.ExecuteNonQuery();

                //Usunięcie ról (żeby nie mógł się zalogować ani nic robić)
                var cmdRoles = connection.CreateCommand();
                cmdRoles.Transaction = transaction;
                cmdRoles.CommandText = "DELETE FROM UserRoles WHERE UserId = $userId;";
                cmdRoles.Parameters.AddWithValue("$userId", userId);
                cmdRoles.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (SqliteException) { transaction.Rollback(); throw; }
            catch (Exception) { transaction.Rollback(); throw; }
        }

        public List<ForgottenUser> GetForgottenUsers(string searchId = null)
        {
            var users = new List<ForgottenUser>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            string query = @"
                SELECT Id, FirstName || ' ' || LastName, ForgottenDate, ForgottenBy 
                FROM Users 
                WHERE ForgottenDate IS NOT NULL AND Status = 'F'";

            if (!string.IsNullOrWhiteSpace(searchId))
            {
                query += " AND Id = $id";
                command.Parameters.AddWithValue("$id", searchId);
            }

            command.CommandText = query;

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new ForgottenUser
                {
                    Identyfikator = reader.GetInt32(0),
                    ImieINazwiskoPoZapomnieniu = reader.GetString(1),
                    DataZapomnienia = reader.GetString(2),
                    IdKtoZapomnial = reader.GetInt32(3)
                });
            }
            return users;
        }


    }
}