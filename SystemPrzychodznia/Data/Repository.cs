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

        // --- POBIERANIE CZASU BLOKADY Z BAZY ---
        public int GetLockoutDurationMinutes()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Czas_blokady_minuty FROM Ustawienia_Systemu LIMIT 1;";
            using var reader = command.ExecuteReader();
            if (reader.Read()) return reader.GetInt32(0);
            return 30; // Domyślnie 30, gdyby tabeli zabrakło
        }

        // --- ZMODYFIKOWANE POBIERANIE DANYCH DO LOGOWANIA ---
        public (int Id, DateTime? BlockedUntil, string Password, string Email, bool RequiresPasswordChange) GetLoginData(string login)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT u.ID_Uzytkownika, u.Blokada_konta_do, h.Haslo_Hash, u.Adres_email, u.Wymaga_zmiany_hasla
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
                bool requiresChange = reader.GetInt32(4) == 1; // 1 = true
                return (id, blockedUntil, password, email, requiresChange);
            }
            return (0, null, null, null, false);
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

        // --- ZAPIS NOWO WYGENEROWANEGO HASŁA (Z FLAGĄ WYMUSZENIA) ---
        public void SaveNewPassword(int userId, string newPassword)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Uzytkownik SET Wymaga_zmiany_hasla = 1 WHERE ID_Uzytkownika = $id;
                INSERT INTO Historia_Hasel (ID_Uzytkownika, Haslo_Hash) VALUES ($id, $pass);";
            command.Parameters.AddWithValue("$id", userId);
            command.Parameters.AddWithValue("$pass", newPassword);
            command.ExecuteNonQuery();
        }

        // --- SAMODZIELNA ZMIANA HASŁA (ZDJĘCIE FLAGI) ---
        public void ChangeUserPassword(int userId, string newPassword)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Uzytkownik SET Wymaga_zmiany_hasla = 0 WHERE ID_Uzytkownika = $id;
                INSERT INTO Historia_Hasel (ID_Uzytkownika, Haslo_Hash) VALUES ($id, $pass);";
            command.Parameters.AddWithValue("$id", userId);
            command.Parameters.AddWithValue("$pass", newPassword);
            command.ExecuteNonQuery();
        }

        public List<string> GetUserPasswordHistory(int userId)
        {
            List<string> passwordHistory = new List<string>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Haslo_Hash FROM Historia_Hasel WHERE ID_Uzytkownika = $id ORDER BY Data_ustawienia DESC;";
            command.Parameters.AddWithValue("$id", userId);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                passwordHistory.Add(reader.GetString(0));
            }
            return passwordHistory;
        }

        public int GetUserID(string login)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT ID_Uzytkownika FROM Uzytkownik WHERE Login = $login;";
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
                if (i == 1) continue;
                Uprawnienie up = uprawnienia.Find(u => u.Id == i);
                if (up != null) up.Posiadane = true;
            }
            return uprawnienia;
        }


        public List<Specjalizacja> GetUserSpec(int user_id)
        {

            List<Uprawnienie> uprawnienia = GetUserUprawnienia(user_id);
            List<Specjalizacja> specjalizacje = GetSpecjalizacje();
            if (uprawnienia.Exists(u => u.Id == 3 && u.Posiadane == true)) // jest lekarzem
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
SELECT Specjalizacja.ID_Specjalizacji
FROM Lekarz_Specjalizacja
JOIN Specjalizacja ON Lekarz_Specjalizacja.ID_Specjalizacji = Specjalizacja.ID_Specjalizacji
WHERE Lekarz_Specjalizacja.ID_Uzytkownika = $id;";
                command.Parameters.AddWithValue("$id", user_id);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int i = reader.GetInt32(0);
                    Specjalizacja sp = specjalizacje.Find(s => s.Id == i);
                    if (sp != null) sp.Posiadane = true;
                }
            }
            return specjalizacje;
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

        public List<UserBasic> GetListUsers(SearchTerms s)
        {
            var users = new List<UserBasic>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
SELECT 
    Login, Imie AS FirstName, Nazwisko AS LastName, Adres_email AS Email, PESEL, Id_Uzytkownika
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
                    command.CommandText += $"\n AND ID_Uzytkownika IN (SELECT ID_Uzytkownika FROM Uzytkownik_Uprawnienie WHERE ID_Uprawnienia = {u.Id})";
                else if (u.Posiadane == false)
                    command.CommandText += $"\n AND ID_Uzytkownika NOT IN (SELECT ID_Uzytkownika FROM Uzytkownik_Uprawnienie WHERE ID_Uprawnienia = {u.Id})";
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
                users.Add(new UserBasic
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
    u.ID_Uzytkownika AS Id, u.Login, u.Imie AS FirstName, u.Nazwisko AS LastName,
    a.Miejscowosc AS Locality, a.Kod_pocztowy AS PostalCode, a.Ulica AS Street,
    a.Numer_posesji_domu AS PropertyNumber, a.Numer_lokalu_mieszkania AS HouseUnitNumber,
    u.PESEL, u.Data_urodzenia AS BirthDate, u.Plec AS Gender, u.Adres_email AS Email, u.Numer_telefonu AS Phone
FROM Uzytkownik u
LEFT JOIN Adres a ON u.ID_Adresu = a.ID_Adresu
WHERE 
    u.ID_Uzytkownika = $id
    AND u.Czy_zapomniany = 0;";
            command.Parameters.AddWithValue("$id", id);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new UserFull
                {
                    Id = reader.GetInt32(0),
                    Login = reader.GetString(1),
                    FirstName = reader.GetString(2),
                    LastName = reader.GetString(3),
                    Locality = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    PostalCode = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    Street = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    PropertyNumber = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                    HouseUnitNumber = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    PESEL = reader.GetString(9),
                    BirthDate = reader.GetString(10),
                    Gender = reader.GetString(11),
                    Email = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                    Phone = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                });
            }

            if (users.Count > 0)
            {
                users[0].Uprawnienia = GetUserUprawnienia(users[0].Id);
                users[0].Specjalizacje = GetUserSpec(users[0].Id);
                if (users[0].Id == 1)
                {
                    users[0].Uprawnienia.Add(new Uprawnienie { Id = 1, Nazwa = "SuperAdmin", Posiadane = true });
                }
                return users[0];
            }
            return null;
        }

        public List<ForgottenUser> GetListForgottenUsers()
        {
            var users = new List<ForgottenUser>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
SELECT 
    u.Login, u.Imie AS FirstName, u.Nazwisko AS LastName, u.Data_zapomnienia AS DateForgotten, 
    u.ID_Kto_Zapomnial AS ForgottenBy, u.Id_Uzytkownika, COALESCE(admin.Login, 'Nieznany') AS ForgottenByLogin
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
                    ForgottenBy = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
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

        // ==========================================
        // GABINETY
        // ==========================================
        public List<Gabinet> GetGabinety()
        {
            var gabinety = new List<Gabinet>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT ID_Gabinetu, Nazwa_Numer FROM Gabinet ORDER BY Nazwa_Numer;";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                gabinety.Add(new Gabinet { Id = reader.GetInt32(0), Nazwa = reader.GetString(1) });
            }
            return gabinety;
        }

        public void AddGabinet(string nazwa)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Gabinet (Nazwa_Numer) VALUES ($nazwa);";
            command.Parameters.AddWithValue("$nazwa", nazwa);
            command.ExecuteNonQuery();
        }

        public void EditGabinet(int id, string nowaNazwa)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "UPDATE Gabinet SET Nazwa_Numer = $nazwa WHERE ID_Gabinetu = $id;";
            command.Parameters.AddWithValue("$nazwa", nowaNazwa);
            command.Parameters.AddWithValue("$id", id);
            command.ExecuteNonQuery();
        }

        // ==========================================
        // SPECJALIZACJE I LEKARZE
        // ==========================================
        public List<Specjalizacja> GetSpecjalizacje()
        {
            var specjalizacje = new List<Specjalizacja>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT ID_Specjalizacji, Nazwa FROM Specjalizacja ORDER BY Nazwa;";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                specjalizacje.Add(new Specjalizacja { Id = reader.GetInt32(0), Nazwa = reader.GetString(1) });
            }
            return specjalizacje;
        }

        public void AddSpecjalizacja(string nazwa)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Specjalizacja (Nazwa) VALUES ($nazwa);";
            command.Parameters.AddWithValue("$nazwa", nazwa);
            command.ExecuteNonQuery();
        }

        public void EditSpecjalizacja(int id, string nowaNazwa)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "UPDATE Specjalizacja SET Nazwa = $nazwa WHERE ID_Specjalizacji = $id;";
            command.Parameters.AddWithValue("$nazwa", nowaNazwa);
            command.Parameters.AddWithValue("$id", id);
            command.ExecuteNonQuery();
        }

        // Wyszukiwanie lekarzy o określonej specjalizacji
        public List<UserBasic> GetLekarzeBySpecjalizacja(int idSpecjalizacji)
        {
            var users = new List<UserBasic>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT u.Login, u.Imie, u.Nazwisko, u.Adres_email, u.PESEL, u.ID_Uzytkownika
                FROM Uzytkownik u
                JOIN Lekarz_Specjalizacja ls ON u.ID_Uzytkownika = ls.ID_Uzytkownika
                WHERE ls.ID_Specjalizacji = $id AND u.Czy_zapomniany = 0
                ORDER BY u.Nazwisko, u.Imie;";

            command.Parameters.AddWithValue("$id", idSpecjalizacji);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new UserBasic
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

        // ==========================================
        // WIZYTY I KOLIZJE
        // ==========================================
        public bool CheckWizytaCollision(int lekarzId, int gabinetId, DateTime data, int? ignoreWizytaId = null)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();

            // Kolizja: jeśli status to nie Odwołana i nie Zakończona
            // Oraz jeśli czas wizyty mieści się w promieniu +/- 29 minut (zakładamy, że wizyta trwa 30 minut).
            command.CommandText = @"
                SELECT COUNT(*) FROM Wizyta 
                WHERE Status NOT IN ('Odwołana', 'Zakończona')
                  AND (ID_Lekarza = $lekarz OR ID_Gabinetu = $gabinet)
                  AND ABS(ROUND((JULIANDAY(Data_i_godzina_rozpoczecia) - JULIANDAY($data)) * 1440)) < 30";

            if (ignoreWizytaId.HasValue)
            {
                command.CommandText += " AND ID_Wizyty != $ignoreId";
                command.Parameters.AddWithValue("$ignoreId", ignoreWizytaId.Value);
            }

            command.Parameters.AddWithValue("$lekarz", lekarzId);
            command.Parameters.AddWithValue("$gabinet", gabinetId);
            command.Parameters.AddWithValue("$data", data.ToString("yyyy-MM-dd HH:mm"));

            long count = (long)command.ExecuteScalar();
            return count > 0;
        }

        public void AddWizyta(Wizyta wizyta)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Wizyta (ID_Pacjenta, ID_Lekarza, ID_Gabinetu, Data_i_godzina_rozpoczecia, Status, Schorzenia_i_dolegliwosci, Zalecenia_i_lekarstwa) 
                VALUES ($pacjent, $lekarz, $gabinet, $data, $status, $schorzenia, $zalecenia);";

            command.Parameters.AddWithValue("$pacjent", wizyta.IdPacjenta);
            command.Parameters.AddWithValue("$lekarz", wizyta.IdLekarza);
            command.Parameters.AddWithValue("$gabinet", wizyta.IdGabinetu);
            command.Parameters.AddWithValue("$data", wizyta.DataRozpoczecia.ToString("yyyy-MM-dd HH:mm"));
            command.Parameters.AddWithValue("$status", wizyta.Status ?? "Zaplanowana");
            command.Parameters.AddWithValue("$schorzenia", (object)wizyta.Schorzenia ?? DBNull.Value);
            command.Parameters.AddWithValue("$zalecenia", (object)wizyta.Zalecenia ?? DBNull.Value);

            command.ExecuteNonQuery();
        }

        public void UpdateWizytaStatus(int wizytaId, string nowyStatus, string schorzenia = null, string zalecenia = null)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Wizyta 
                SET Status = $status, 
                    Schorzenia_i_dolegliwosci = COALESCE($schorzenia, Schorzenia_i_dolegliwosci),
                    Zalecenia_i_lekarstwa = COALESCE($zalecenia, Zalecenia_i_lekarstwa)
                WHERE ID_Wizyty = $id;";

            command.Parameters.AddWithValue("$status", nowyStatus);
            command.Parameters.AddWithValue("$schorzenia", (object)schorzenia ?? DBNull.Value);
            command.Parameters.AddWithValue("$zalecenia", (object)zalecenia ?? DBNull.Value);
            command.Parameters.AddWithValue("$id", wizytaId);
            command.ExecuteNonQuery();
        }

        public void FullEditWizyta(Wizyta wizyta)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Wizyta 
                SET ID_Pacjenta = $pacjent,
                    ID_Lekarza = $lekarz,
                    ID_Gabinetu = $gabinet,
                    Data_i_godzina_rozpoczecia = $data,
                    Status = $status,
                    Schorzenia_i_dolegliwosci = $schorzenia,
                    Zalecenia_i_lekarstwa = $zalecenia
                WHERE ID_Wizyty = $id;";

            command.Parameters.AddWithValue("$pacjent", wizyta.IdPacjenta);
            command.Parameters.AddWithValue("$lekarz", wizyta.IdLekarza);
            command.Parameters.AddWithValue("$gabinet", wizyta.IdGabinetu);
            command.Parameters.AddWithValue("$data", wizyta.DataRozpoczecia.ToString("yyyy-MM-dd HH:mm"));
            command.Parameters.AddWithValue("$status", wizyta.Status ?? "Zaplanowana");
            command.Parameters.AddWithValue("$schorzenia", (object)wizyta.Schorzenia ?? DBNull.Value);
            command.Parameters.AddWithValue("$zalecenia", (object)wizyta.Zalecenia ?? DBNull.Value);
            command.Parameters.AddWithValue("$id", wizyta.Id);
            command.ExecuteNonQuery();
        }

        public void DeleteWizyta(int wizytaId)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Wizyta WHERE ID_Wizyty = $id;";
            command.Parameters.AddWithValue("$id", wizytaId);
            command.ExecuteNonQuery();
        }

        // Filtrowanie Wizyt za pomocą modelu SearchTermsWizyta
        public List<Wizyta> GetWizyty(SearchTermsWizyta s = null)
        {
            var wizyty = new List<Wizyta>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT 
                    w.ID_Wizyty, w.ID_Pacjenta, w.ID_Lekarza, w.ID_Gabinetu, w.Data_i_godzina_rozpoczecia, w.Status, w.Schorzenia_i_dolegliwosci, w.Zalecenia_i_lekarstwa,
                    p.Imie || ' ' || p.Nazwisko AS PacjentNazwa,
                    l.Imie || ' ' || l.Nazwisko AS LekarzNazwa,
                    g.Nazwa_Numer AS GabinetNazwa
                FROM Wizyta w
                JOIN Pacjent p ON w.ID_Pacjenta = p.ID_Pacjenta
                JOIN Uzytkownik l ON w.ID_Lekarza = l.ID_Uzytkownika
                JOIN Gabinet g ON w.ID_Gabinetu = g.ID_Gabinetu
                WHERE 1=1 ";

            if (s != null)
            {
                if (s.DataOd.HasValue)
                {
                    command.CommandText += " AND w.Data_i_godzina_rozpoczecia >= $dataOd";
                    command.Parameters.AddWithValue("$dataOd", s.DataOd.Value.ToString("yyyy-MM-dd 00:00"));
                }
                if (s.DataDo.HasValue)
                {
                    command.CommandText += " AND w.Data_i_godzina_rozpoczecia <= $dataDo";
                    command.Parameters.AddWithValue("$dataDo", s.DataDo.Value.ToString("yyyy-MM-dd 23:59"));
                }
                if (s.IdLekarza.HasValue && s.IdLekarza.Value > 0)
                {
                    command.CommandText += " AND w.ID_Lekarza = $lekarz";
                    command.Parameters.AddWithValue("$lekarz", s.IdLekarza.Value);
                }
                if (s.IdPacjenta.HasValue && s.IdPacjenta.Value > 0)
                {
                    command.CommandText += " AND w.ID_Pacjenta = $pacjent";
                    command.Parameters.AddWithValue("$pacjent", s.IdPacjenta.Value);
                }
                if (!string.IsNullOrWhiteSpace(s.Status))
                {
                    command.CommandText += " AND w.Status = $status";
                    command.Parameters.AddWithValue("$status", s.Status);
                }
            }

            command.CommandText += " ORDER BY w.Data_i_godzina_rozpoczecia DESC;";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                wizyty.Add(new Wizyta
                {
                    Id = reader.GetInt32(0),
                    IdPacjenta = reader.GetInt32(1),
                    IdLekarza = reader.GetInt32(2),
                    IdGabinetu = reader.GetInt32(3),
                    DataRozpoczecia = DateTime.Parse(reader.GetString(4)),
                    Status = reader.GetString(5),
                    Schorzenia = reader.IsDBNull(6) ? "" : reader.GetString(6),
                    Zalecenia = reader.IsDBNull(7) ? "" : reader.GetString(7),
                    NazwaPacjenta = reader.GetString(8),
                    NazwaLekarza = reader.GetString(9),
                    NazwaGabinetu = reader.GetString(10)
                });
            }
            return wizyty;
        }
    }

    // ==========================================
    // NOWE MODELE DANYCH
    // ==========================================
    public class Gabinet
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }
    }

    public class Specjalizacja
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }

        public bool? Posiadane { get; set; } = false;
    }

    public class Wizyta
    {
        public int Id { get; set; }
        public int IdPacjenta { get; set; }
        public int IdLekarza { get; set; }
        public int IdGabinetu { get; set; }
        public DateTime DataRozpoczecia { get; set; }
        public string Status { get; set; }
        public string Schorzenia { get; set; }
        public string Zalecenia { get; set; }

        public string NazwaPacjenta { get; set; }
        public string NazwaLekarza { get; set; }
        public string NazwaGabinetu { get; set; }
    }

    public class SearchTermsWizyta
    {
        public DateTime? DataOd { get; set; }
        public DateTime? DataDo { get; set; }
        public int? IdLekarza { get; set; }
        public int? IdPacjenta { get; set; }
        public string Status { get; set; }
    }
}