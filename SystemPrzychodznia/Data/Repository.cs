using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace SystemPrzychodznia.Data
{
    public class UserRepository
    {
        private readonly string _connectionString = "Data Source=przychodnia.db";

        public void Add(UserFull user)
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

        public List<User> GetList()
        {
            var users = new List<User>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT Login, FirstName, LastName, Email, PESEL FROM Users";

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

    }
}