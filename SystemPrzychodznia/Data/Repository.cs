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
WHERE Uzytkownik_Uprawnienie.ID_Uzytkownika = $id;'";

            command.Parameters.AddWithValue("$id", user_id);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                int i = reader.GetInt32(0);
                if (i == 1)
                    continue; // pomijamy SuperAdmina, który nie jest normalnym uprawnieniem do przypisywania   
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
    Nazwa NOT LIKE 'SuperAdmin'";

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
    PESEL,
    Id_Uzytkownika
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
                    Id = reader.GetInt32(5)
                });
            }
            return users;
        }

        public void  EditUser(UserFull user)
        {
            try
            {
                if (user.Id == 1)
                {
                    throw new UnauthorizedAccessException("Nie można edytować SuperAdmin");
                }
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
        u.Blokada_konta_do IS NULL               -- konto nie jest zablokowane
        OR u.Blokada_konta_do <= CURRENT_TIMESTAMP  -- blokada już wygasła
    )
    AND u.Czy_zapomniany = 0;                    -- konto nie jest oznaczone jako zapomniane";


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

        public void ZmieńUprawnienia(int userId, List<Uprawnienie> noweUprawnienia)
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
            foreach (Uprawnienie uprawnienie in noweUprawnienia)
            {
                if (!uprawnienie.Posiadane)
                    continue; // pomijamy uprawnienia, które nie są zaznaczone jako posiadane
                var insertCommand = connection.CreateCommand();
                insertCommand.CommandText = @"
INSERT INTO Uzytkownik_Uprawnienie (ID_Uzytkownika, ID_Uprawnienia)
VALUES ( $userId , $uprawnienieId);
";
                insertCommand.Parameters.AddWithValue("$userId", userId);
                insertCommand.Parameters.AddWithValue("$uprawnienieId", uprawnienie.Id);
                insertCommand.ExecuteNonQuery();
            }
}

        public const int VAL_LOGIN = 0;
        public const int VAL_EMAIL = 1;
        public const int VAL_PESEL = 2;
        public bool CzyIstniejeDanyUżytkowik(string wartoscAtrybutu, int jakiAtrybut, bool exlude = false, int excludeId = -1)
        // funkcja do sprawdzenia czy istnieje już użytkownik z danym loginem, emailem lub PESELem.
        // Jeśli exlude=true, to sprawdzamy czy istnieje inny użytkownik z tym atrybutem niż ten o ID=excludeId (przydatne przy edycji danych)
        // jakiAtrybut: UserRepository.VAL_LOGIN, UserRepository.VAL_EMAIL, UserRepository.VAL_PESEL
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
            long count = (long)command.ExecuteScalar(); //bez long jest błąd przy castowaniu
            return count > 0;
        }

    }
}