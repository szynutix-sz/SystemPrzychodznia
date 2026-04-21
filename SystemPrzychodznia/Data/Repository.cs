using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace SystemPrzychodznia.Data
{
    internal partial class UserRepository
    {
        private readonly string _connectionString = "Data Source=przychodnia.db";

        // --- METODY DO LOGOWANIA I BLOKAD ---
        public (int Id, DateTime? BlockedUntil, string Password, string Email) GetLoginData(string login)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT u.ID_Uzytkownika, u.Blokada_konta_do, h.Haslo_Hash, u.Adres_email
                FROM Uzytkownik u
                JOIN Historia_Hasel h ON u.ID_Uzytkownika = h.ID_Uzytkownika
                WHERE u.Login = $login AND u.Czy_zapomniany = 0
                ORDER BY h.ID_Hasla DESC LIMIT 1;";
            command.Parameters.AddWithValue("$login", login);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                int id = reader.GetInt32(0);
                DateTime? blockedUntil = reader.IsDBNull(1) ? (DateTime?)null : DateTime.Parse(reader.GetString(1));
                string password = reader.GetString(2);
                string email = reader.GetString(3);
                return (id, blockedUntil, password, email);
            }
            return (0, null, null, null);
        }

        public void UpdateBlockedUntil(int userId, DateTime? blockedUntil)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "UPDATE Uzytkownik SET Blokada_konta_do = $date WHERE ID_Uzytkownika = $id;";
            command.Parameters.AddWithValue("$id", userId);

            if (blockedUntil.HasValue)
                command.Parameters.AddWithValue("$date", blockedUntil.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            else
                command.Parameters.AddWithValue("$date", DBNull.Value);

            command.ExecuteNonQuery();
        }

        // --- ZAPIS NOWO WYGENEROWANEGO HASŁA ---
        public void SaveNewPassword(int userId, string newPassword)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText = "INSERT INTO Historia_Hasel (ID_Uzytkownika, Haslo_Hash) VALUES ($id, $pass);";
            command.Parameters.AddWithValue("$id", userId);
            command.Parameters.AddWithValue("$pass", newPassword);

            command.ExecuteNonQuery();
        }

        public int GetUserID(string login)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
SELECT ID_Uzytkownika FROM Uzytkownik WHERE Login = $login;
";
            command.Parameters.AddWithValue("$login", login);
            using var reader_id = command.ExecuteReader();

            reader_id.Read();
            return reader_id.GetInt32(0);
        }

        public List<Uprawnienie> GetUserUprawnienia(int user_id)
        {
            List<Uprawnienie> uprawnienia = GetUprawnienia();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
SELECT Uprawnienie.ID_Uprawnienia
FROM Uzytkownik_Uprawnienie
JOIN Uprawnienie ON Uzytkownik_Uprawnienie.ID_Uprawnienia = Uprawnienie.ID_Uprawnienia
WHERE Uzytkownik_Uprawnienie.ID_Uzytkownika = $id;";

            command.Parameters.AddWithValue("$id", user_id);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                int i = reader.GetInt32(0);
                if (i == 1)
                    continue;
                Uprawnienie up = uprawnienia.Find(u => u.Id == i);
                up.Posiadane = true;
            }
            return uprawnienia;
        }

        public List<Uprawnienie> GetUprawnienia()
        {
            List<Uprawnienie> uprawnienia = new List<Uprawnienie>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
SELECT 
    Nazwa,
    ID_Uprawnienia
FROM Uprawnienie
WHERE 
    ID_Uprawnienia  != 1;";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                uprawnienia.Add(new Uprawnienie
                {
                    Nazwa = reader.GetString(0),
                    Id = reader.GetInt32(1)
                });
            }
            return uprawnienia;
        }

        public List<User> GetListUsers(SearchTerms s)
        {
            var users = new List<User>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
SELECT 
    Login, 
    Imie AS FirstName, 
    Nazwisko AS LastName, 
    Adres_email AS Email, 
    PESEL,
    Id_Uzytkownika
FROM Uzytkownik
WHERE 
    Czy_zapomniany = 0
    AND Login LIKE '%' || $login || '%'
    AND Imie LIKE '%' || $firstName || '%'
    AND Nazwisko LIKE '%' || $lastName || '%'
    AND Adres_email LIKE '%' || $email || '%'
    AND PESEL LIKE '%' || $pesel || '%'";

            foreach (Uprawnienie u in s.Uprawnienia)
            {
                if (u.Posiadane == true)
                {
                    command.CommandText += $"\n AND ID_Uzytkownika IN (SELECT ID_Uzytkownika FROM Uzytkownik_Uprawnienie WHERE ID_Uprawnienia = {u.Id})";
                }
                else if (u.Posiadane == false)
                {
                    command.CommandText += $"\n AND ID_Uzytkownika NOT IN (SELECT ID_Uzytkownika FROM Uzytkownik_Uprawnienie WHERE ID_Uprawnienia = {u.Id})";
                }
            }

            command.CommandText += ";";

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
                    Id = reader.GetInt32(5)
                });
            }
            return users;
        }

        public UserFull GetUserFull(int id)
        {
            var users = new List<UserFull>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
               SELECT 
    u.ID_Uzytkownika AS Id,
    u.Login,
    u.Imie AS FirstName,
    u.Nazwisko AS LastName,
    a.Miejscowosc AS Locality,
    a.Kod_pocztowy AS PostalCode,
    a.Ulica AS Street,
    a.Numer_posesji_domu AS PropertyNumber,
    a.Numer_lokalu_mieszkania AS HouseUnitNumber,
    u.PESEL,
    u.Data_urodzenia AS BirthDate,
    u.Plec AS Gender,
    u.Adres_email AS Email,
    u.Numer_telefonu AS Phone
FROM Uzytkownik u
JOIN Adres a ON u.ID_Adresu = a.ID_Adresu
WHERE 
    u.ID_Uzytkownika = $id
    AND (
        u.Blokada_konta_do IS NULL               
        OR u.Blokada_konta_do <= CURRENT_TIMESTAMP  
    )
    AND u.Czy_zapomniany = 0;";

            command.Parameters.AddWithValue("id", id);

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

            users[0].Uprawnienia = GetUserUprawnienia(users[0].Id);

            if (users[0].Id == 1)
            {
                users[0].Uprawnienia.Add(new Uprawnienie
                {
                    Id = 1,
                    Nazwa = "SuperAdmin",
                    Posiadane = true
                });
            }

            return users[0];
        }

        public List<ForgottenUser> GetListForgottenUsers()
        {
            var users = new List<ForgottenUser>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
SELECT 
    u.Login, 
    u.Imie AS FirstName, 
    u.Nazwisko AS LastName, 
    u.Data_zapomnienia AS DateForgotten, 
    u.ID_Kto_Zapomnial AS ForgottenBy,
    u.Id_Uzytkownika,
    COALESCE(admin.Login, 'Nieznany') AS ForgottenByLogin
FROM Uzytkownik u
LEFT JOIN Uzytkownik admin ON u.ID_Kto_Zapomnial = admin.ID_Uzytkownika
WHERE u.Czy_zapomniany = 1;";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new ForgottenUser
                {
                    Login = reader.GetString(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    DateForgotten = reader.GetString(3),
                    ForgottenBy = reader.GetInt32(4),
                    Id = reader.GetInt32(5),
                    ForgottenByLogin = reader.GetString(6)
                });
            }
            return users;
        }

        public const int VAL_LOGIN = 0;
        public const int VAL_EMAIL = 1;
        public const int VAL_PESEL = 2;
        public bool CzyIstniejeDanyUżytkowik(string wartoscAtrybutu, int jakiAtrybut, bool exlude = false, int excludeId = -1)
        {
            string atrybut = jakiAtrybut switch
            {
                VAL_LOGIN => "Login",
                VAL_EMAIL => "Adres_email",
                VAL_PESEL => "PESEL",
                _ => throw new ArgumentException("Nieprawidłowy atrybut do sprawdzenia.")
            };
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            if (exlude)
            {
                command.CommandText = $"SELECT COUNT(*) FROM Uzytkownik WHERE {atrybut} = $wartoscAtrybutu AND ID_Uzytkownika != $excludeId;";
                command.Parameters.AddWithValue("$excludeId", excludeId);
            }
            else
            {
                command.CommandText = $"SELECT COUNT(*) FROM Uzytkownik WHERE {atrybut} = $wartoscAtrybutu;";
            }

            command.Parameters.AddWithValue("$wartoscAtrybutu", wartoscAtrybutu);
            long count = (long)command.ExecuteScalar();
            return count > 0;
        }
    }
}