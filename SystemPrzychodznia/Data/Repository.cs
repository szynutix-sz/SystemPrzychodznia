using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic.Logging;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace SystemPrzychodznia.Data
{
    public class SearchTerms
    {
        public string Login = "";
        public string FirstName = "";
        public string LastName = "";
        public string Email = "";
        public string PESEL = "";
    }
    public class UserRepository
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
                WHERE Status = 'A'";

            command.CommandText += $"\n AND Login LIKE '%{s.Login}%'";
            command.CommandText += $"\n AND FirstName LIKE '%{s.FirstName}%'";
            command.CommandText += $"\n AND LastName LIKE '%{s.LastName}%'";
            command.CommandText += $"\n AND Email LIKE '%{s.Email}%'";
            command.CommandText += $"\n AND PESEL LIKE '%{s.PESEL}%'";
            command.CommandText += ";";

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
                WHERE Status = 'A'";

            command.CommandText += $"\n AND Login = '{S_Login}'";
            command.CommandText += ";";

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



    }
}