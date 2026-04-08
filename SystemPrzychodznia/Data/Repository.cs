using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System;

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
INSERT INTO Addresses (
    Locality,
    PostalCode,
    Street,
    PropertyNumber,
    HouseUnitNumber
)
VALUES (
    $locality,
    $postalCode,
    $street,
    $propertyNumber,
    $houseUnitNumber
);

-- 2. Wstawienie użytkownika (AddressId pobierane automatycznie)
INSERT INTO Users (
    AddressId,
    Login,
    FirstName,
    LastName,
    PESEL,
    BirthDate,
    Gender,
    Email,
    Phone,
    IsForgotten
)
VALUES (
    last_insert_rowid(),
    $login,
    $firstName,
    $lastName,
    $pesel,
    $birthDate,
    $gender,
    $email,
    $phone,
    0
);

-- 3. Wstawienie hasła do historii haseł
INSERT INTO PasswordHistory (
    UserId,
    PasswordHash
)
VALUES (
    last_insert_rowid(),
    $password
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
            catch (SqliteException) { throw; }
            catch (Exception) { throw; }
        }

        public int GetUserID(string login)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id FROM Users WHERE Login = $login;";
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
SELECT r.Name
FROM UserRoles ur
JOIN Roles r ON ur.RoleId = r.Id
WHERE ur.UserId = $id;";

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
            command.CommandText = "SELECT Name FROM Roles WHERE Name NOT LIKE 'SuperAdmin'";

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
    FirstName, 
    LastName, 
    Email, 
    PESEL 
FROM Users
WHERE 
    IsForgotten = 0
    AND Login LIKE '%' || $login || '%'
    AND FirstName LIKE '%' || $firstName || '%'
    AND LastName LIKE '%' || $lastName || '%'
    AND Email LIKE '%' || $email || '%'
    AND PESEL LIKE '%' || $pesel || '%';";

            command.Parameters.AddWithValue("$login", s.Login ?? "");
            command.Parameters.AddWithValue("$firstName", s.FirstName ?? "");
            command.Parameters.AddWithValue("$lastName", s.LastName ?? "");
            command.Parameters.AddWithValue("$pesel", s.PESEL ?? "");
            command.Parameters.AddWithValue("$email", s.Email ?? "");

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

        public void EditUser(UserFull user)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
BEGIN TRANSACTION;

UPDATE Addresses
SET 
    Locality = $locality,
    PostalCode = $postalCode,
    Street = $street,
    PropertyNumber = $propertyNumber,
    HouseUnitNumber = $houseUnitNumber
WHERE Id = (SELECT AddressId FROM Users WHERE Id = $id);

UPDATE Users
SET 
    Login = $login,
    FirstName = $firstName,
    LastName = $lastName,
    PESEL = $pesel,
    BirthDate = $birthDate,
    Gender = $gender,
    Email = $email,
    Phone = $phone
WHERE Id = $id;

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
            catch (SqliteException) { throw; }
            catch (Exception) { throw; }
        }

        public UserFull GetUserFull(string S_Login)
        {
            var users = new List<UserFull>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
SELECT 
    u.Id,
    u.Login,
    u.FirstName,
    u.LastName,
    a.Locality,
    a.PostalCode,
    a.Street,
    a.PropertyNumber,
    a.HouseUnitNumber,
    u.PESEL,
    u.BirthDate,
    u.Gender,
    u.Email,
    u.Phone
FROM Users u
JOIN Addresses a ON u.AddressId = a.Id
WHERE 
    u.Login = $login
    AND (u.BlockedUntil IS NULL OR u.BlockedUntil <= CURRENT_TIMESTAMP)
    AND u.IsForgotten = 0;";

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

            if (users.Count > 0)
            {
                users[0].Uprawnienia = GetUserUprawnienia(users[0].Id);
                return users[0];
            }
            return null;
        }

        public void ZmieńUprawnienia(int userId, List<string> noweUprawnienia)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var deleteCommand = connection.CreateCommand();
            deleteCommand.CommandText = @"
DELETE FROM UserRoles 
WHERE UserId = $userId
  AND RoleId != (SELECT Id FROM Roles WHERE Name = 'SuperAdmin');";
            deleteCommand.Parameters.AddWithValue("$userId", userId);
            deleteCommand.ExecuteNonQuery();

            foreach (var uprawnienie in noweUprawnienia)
            {
                var insertCommand = connection.CreateCommand();
                insertCommand.CommandText = @"
INSERT INTO UserRoles (UserId, RoleId)
SELECT u.Id, p.Id
FROM Users u, Roles p
WHERE u.Id = $userId
  AND p.Name = $uprawnienie
  AND NOT EXISTS (SELECT 1 FROM UserRoles ur
                  WHERE ur.UserId = u.Id
                    AND ur.RoleId = p.Id);";
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
                command.CommandText = "SELECT COUNT(*) FROM Users WHERE Login = $login AND Id != $excludeId;";
                command.Parameters.AddWithValue("$excludeId", excludeId);
            }
            else
            {
                command.CommandText = "SELECT COUNT(*) FROM Users WHERE Login = $login;";
            }
            command.Parameters.AddWithValue("$login", login);
            return (long)command.ExecuteScalar() > 0;
        }

        public bool CzyIstniejeEmail(string email, int excludeId = 0)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            if (excludeId > 0)
            {
                command.CommandText = "SELECT COUNT(*) FROM Users WHERE Email = $email AND Id != $excludeId;";
                command.Parameters.AddWithValue("$excludeId", excludeId);
            }
            else
            {
                command.CommandText = "SELECT COUNT(*) FROM Users WHERE Email = $email;";
            }
            command.Parameters.AddWithValue("$email", email);
            return (long)command.ExecuteScalar() > 0;
        }

        public bool CzyIstnieje_PESEL(string pesel, int excludeId = 0)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            if (excludeId > 0)
            {
                command.CommandText = "SELECT COUNT(*) FROM Users WHERE PESEL = $pesel AND Id != $excludeId;";
                command.Parameters.AddWithValue("$excludeId", excludeId);
            }
            else
            {
                command.CommandText = "SELECT COUNT(*) FROM Users WHERE PESEL = $pesel;";
            }
            command.Parameters.AddWithValue("$pesel", pesel);
            return (long)command.ExecuteScalar() > 0;
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
                WHERE u.IsForgotten = 0 AND r.Name = $roleName;";

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
        public List<User> GetUsersByMultipleRoles(List<string> roleNames)
        {
            var users = new List<User>();

            if (roleNames == null || roleNames.Count == 0) return users;

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();

            var parameters = new List<string>();
            for (int i = 0; i < roleNames.Count; i++)
            {
                string paramName = $"$r{i}";
                parameters.Add(paramName);
                command.Parameters.AddWithValue(paramName, roleNames[i]);
            }

            string inClause = string.Join(", ", parameters);

            command.CommandText = $@"
                SELECT u.Login, u.FirstName, u.LastName, u.Email, u.PESEL 
                FROM Users u
                JOIN UserRoles ur ON u.Id = ur.UserId
                JOIN Roles r ON ur.RoleId = r.Id
                WHERE u.IsForgotten = 0 AND r.Name IN ({inClause})
                GROUP BY u.Id
                HAVING COUNT(DISTINCT r.Id) = {roleNames.Count};";

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
                var cmdUser = connection.CreateCommand();
                cmdUser.Transaction = transaction;
                cmdUser.CommandText = @"
                    UPDATE Users 
                    SET FirstName = $rand,
                        LastName = $rand,
                        PESEL = $pesel,
                        BirthDate = $date,
                        Gender = 'Inna',
                        IsForgotten = 1,
                        ForgottenDate = CURRENT_TIMESTAMP,
                        ForgottenBy = $adminId
                    WHERE Id = $userId;";

                cmdUser.Parameters.AddWithValue("$rand", randomString);
                cmdUser.Parameters.AddWithValue("$pesel", fakePesel);
                cmdUser.Parameters.AddWithValue("$date", fakeDate);
                cmdUser.Parameters.AddWithValue("$adminId", adminId);
                cmdUser.Parameters.AddWithValue("$userId", userId);
                cmdUser.ExecuteNonQuery();

                var cmdRoles = connection.CreateCommand();
                cmdRoles.Transaction = transaction;
                cmdRoles.CommandText = "DELETE FROM UserRoles WHERE UserId = $userId;";
                cmdRoles.Parameters.AddWithValue("$userId", userId);
                cmdRoles.ExecuteNonQuery();

                transaction.Commit();
            }
            catch { transaction.Rollback(); throw; }
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
                WHERE ForgottenDate IS NOT NULL AND IsForgotten = 1";

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
                    DataZapomnienia = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    IdKtoZapomnial = reader.IsDBNull(3) ? 0 : reader.GetInt32(3)
                });
            }
            return users;
        }
    }
}