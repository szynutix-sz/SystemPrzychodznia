using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Logging;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
BEGIN TRANSACTION;

-- 1. Wstawienie adresu
INSERT INTO Adres (
    Miejscowosc,
    Kod_pocztowy,
    Ulica,
    Numer_posesji_domu,
    Numer_lokalu_mieszkania
)
VALUES (
    $locality,
    $postalCode,
    $street,
    $propertyNumber,
    $houseUnitNumber
);

-- 2. Wstawienie użytkownika (ID_Adresu pobierane automatycznie)
INSERT INTO Uzytkownik (
    ID_Adresu,
    Login,
    Imie,
    Nazwisko,
    PESEL,
    Data_urodzenia,
    Plec,
    Adres_email,
    Numer_telefonu,
    Blokada_konta_do,
    Czy_zapomniany,
    Data_zapomnienia,
    ID_Kto_Zapomnial
)
VALUES (
    last_insert_rowid(),   -- ID właśnie dodanego adresu
    $login,
    $firstName,
    $lastName,
    $pesel,
    $birthDate,
    $gender,
    $email,
    $phone,
    NULL,                  -- domyślnie brak blokady
    0,                     -- konto nieoznaczone jako zapomniane
    NULL,
    NULL
);

-- 3. Wstawienie hasła do historii haseł
INSERT INTO Historia_Hasel (
    ID_Uzytkownika,
    Haslo_Hash,
    Data_ustawienia
)
VALUES (
    last_insert_rowid(),   -- ID właśnie dodanego użytkownika
    $password,
    CURRENT_TIMESTAMP      -- data ustawienia hasła
);

COMMIT;
";
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

                user.Id = GetUserID(user.Login);

                ZmieńUprawnienia(user.Id, user.Uprawnienia);

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

        public List<string> GetUserUprawnienia(int user_id)
        {
            List<string> uprawnienia = new List<string>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
SELECT Uprawnienie.Nazwa
FROM Uzytkownik_Uprawnienie
JOIN Uprawnienie ON Uzytkownik_Uprawnienie.ID_Uprawnienia = Uprawnienie.ID_Uprawnienia
WHERE Uzytkownik_Uprawnienie.ID_Uzytkownika = $id;'";

            command.Parameters.AddWithValue("$id", user_id);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                uprawnienia.Add(reader.GetString(0));
            }
            return uprawnienia;
        }
        public List<string> GetUprawnienia()
        {
            List<string> uprawnienia = new List<string>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
SELECT 
    Nazwa
FROM Uprawnienie
WHERE 
    Nazwa NOT LIKE 'SuperAdmin'";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                uprawnienia.Add(reader.GetString(0));
            }
            return uprawnienia;
        }

        public List<User> GetList(SearchTerms s)
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
    PESEL 
FROM Uzytkownik
WHERE 
    Czy_zapomniany = 0
    AND Login LIKE '%' || $login || '%'
    AND Imie LIKE '%' || $firstName || '%'
    AND Nazwisko LIKE '%' || $lastName || '%'
    AND Adres_email LIKE '%' || $email || '%'
    AND PESEL LIKE '%' || $pesel || '%';";

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
                   BEGIN TRANSACTION;

-- 1. Aktualizacja danych adresowych powiązanych z użytkownikiem
UPDATE Adres
SET 
    Miejscowosc = $locality,
    Kod_pocztowy = $postalCode,
    Ulica = $street,
    Numer_posesji_domu = $propertyNumber,
    Numer_lokalu_mieszkania = $houseUnitNumber
WHERE ID_Adresu = (
    SELECT ID_Adresu 
    FROM Uzytkownik 
    WHERE ID_Uzytkownika = $id
);

-- 2. Aktualizacja danych użytkownika
UPDATE Uzytkownik
SET 
    Login = $login,
    Imie = $firstName,
    Nazwisko = $lastName,
    PESEL = $pesel,
    Data_urodzenia = $birthDate,
    Plec = $gender,
    Adres_email = $email,
    Numer_telefonu = $phone
WHERE ID_Uzytkownika = $id;

COMMIT;";


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

                ZmieńUprawnienia(user.Id, user.Uprawnienia);

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
    u.Login = $login
    AND (
        u.Blokada_konta_do IS NULL               -- konto nie jest zablokowane
        OR u.Blokada_konta_do <= CURRENT_TIMESTAMP  -- blokada już wygasła
    )
    AND u.Czy_zapomniany = 0;                    -- konto nie jest oznaczone jako zapomniane";


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

            users[0].Uprawnienia = GetUserUprawnienia(users[0].Id);
            return users[0];
        }

        public void ForgetUser(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "UPDATE Uzytkownik SET Czy_zapomniany = 1 WHERE ID_Uzytkownika = $id;";
            command.Parameters.AddWithValue("$id", id);
            command.ExecuteNonQuery();
        }

        public void ZmieńUprawnienia(int userId, List<string> noweUprawnienia)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();

            // Najpierw usuwamy wszystkie obecne uprawnienia użytkownika
            var deleteCommand = connection.CreateCommand();
            deleteCommand.CommandText = @"
DELETE FROM Uzytkownik_Uprawnienie 
WHERE ID_Uzytkownika = $userId
    AND ID_Uprawnienia != 
        (SELECT ID_Uprawnienia FROM Uprawnienie WHERE Nazwa = 'SuperAdmin');";
            deleteCommand.Parameters.AddWithValue("$userId", userId);
            deleteCommand.ExecuteNonQuery();
            // Następnie dodajemy nowe uprawnienia
            foreach (var uprawnienie in noweUprawnienia)
            {
                var insertCommand = connection.CreateCommand();
                insertCommand.CommandText = @"
INSERT INTO Uzytkownik_Uprawnienie (ID_Uzytkownika, ID_Uprawnienia)
SELECT u.ID_Uzytkownika, p.ID_Uprawnienia
FROM Uzytkownik u, Uprawnienie p
WHERE u.ID_Uzytkownika = $userId
  AND p.Nazwa = $uprawnienie
  AND NOT EXISTS (SELECT 1 FROM Uzytkownik_Uprawnienie up
                  WHERE up.ID_Uzytkownika = u.ID_Uzytkownika
                    AND up.ID_Uprawnienia = p.ID_Uprawnienia);
";
                insertCommand.Parameters.AddWithValue("$userId", userId);
                insertCommand.Parameters.AddWithValue("$uprawnienie", uprawnienie);
                insertCommand.ExecuteNonQuery();
            }
}

        public bool CzyIstniejeLogin(string login, int excludeId = 0)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            if (excludeId > 0)
            {
                command.CommandText = "SELECT COUNT(*) FROM Uzytkownik WHERE Login = $login AND ID_Uzytkownika != $excludeId;";
                command.Parameters.AddWithValue("$excludeId", excludeId);
            }
            else
            {
                command.CommandText = "SELECT COUNT(*) FROM Uzytkownik WHERE Login = $login;";
            }
            command.Parameters.AddWithValue("$login", login);
            long count = (long)command.ExecuteScalar();
            return count > 0;
        }

        public bool CzyIstniejeEmail(string email, int excludeId = 0)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            if (excludeId > 0)
            {
                command.CommandText = "SELECT COUNT(*) FROM Uzytkownik WHERE Adres_email = $email AND ID_Uzytkownika != $excludeId;";
                command.Parameters.AddWithValue("$excludeId", excludeId);
            }
            else
            {
                command.CommandText = "SELECT COUNT(*) FROM Uzytkownik WHERE Adres_email = $email;";
            }
            command.Parameters.AddWithValue("$email", email);
            long count = (long)command.ExecuteScalar();
            return count > 0;
        }

        public bool CzyIstnieje_PESEL(string pesel, int excludeId = 0)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            if (excludeId > 0)
            {
                command.CommandText = "SELECT COUNT(*) FROM Uzytkownik WHERE PESEL = $pesel AND ID_Uzytkownika != $excludeId;";
                command.Parameters.AddWithValue("$excludeId", excludeId);
            }
            else
            {
                command.CommandText = "SELECT COUNT(*) FROM Uzytkownik WHERE PESEL = $pesel;";
            }
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