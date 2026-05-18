using Microsoft.Data.Sqlite;
using System;

namespace SystemPrzychodznia.Data
{
    internal static partial class DatabaseInitializer
    {
        private const string ConnectionString = "Data Source=przychodnia.db";

        public static void Initialize(bool testing)
        {
            InitializeDatabase();
            if (testing)
            {
                InitializeTestData();
            }
        }

        public static void InitializeDatabase()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            var createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText = @"
PRAGMA foreign_keys = ON;

-- ============================================================
-- 1. Tabela Adres
-- ============================================================
CREATE TABLE IF NOT EXISTS Adres (
    ID_Adresu INTEGER PRIMARY KEY AUTOINCREMENT,
    Miejscowosc TEXT NOT NULL,
    Kod_pocztowy TEXT NOT NULL,
    Ulica TEXT,
    Numer_posesji_domu TEXT NOT NULL,
    Numer_lokalu_mieszkania TEXT
);

-- ============================================================
-- Dodane: Tabela Ustawienia_Systemu (Czas blokady)
-- ============================================================
CREATE TABLE IF NOT EXISTS Ustawienia_Systemu (
    Czas_blokady_minuty INTEGER NOT NULL
);

INSERT INTO Ustawienia_Systemu (Czas_blokady_minuty)
SELECT 30 WHERE NOT EXISTS (SELECT 1 FROM Ustawienia_Systemu);

-- ============================================================
-- 2. Tabela Uzytkownik
-- ============================================================
CREATE TABLE IF NOT EXISTS Uzytkownik (
    ID_Uzytkownika INTEGER PRIMARY KEY AUTOINCREMENT,
    ID_Adresu INTEGER NOT NULL,
    Login TEXT UNIQUE NOT NULL,
    Imie TEXT NOT NULL,
    Nazwisko TEXT NOT NULL,
    PESEL TEXT NOT NULL,
    Data_urodzenia TEXT NOT NULL,
    Plec TEXT NOT NULL CHECK (Plec IN ('K', 'M', 'Inna')),
    Adres_email TEXT UNIQUE NOT NULL,
    Numer_telefonu TEXT NOT NULL,
    Blokada_konta_do TEXT,
    Czy_zapomniany INTEGER NOT NULL DEFAULT 0 CHECK (Czy_zapomniany IN (0,1)),
    Wymaga_zmiany_hasla INTEGER NOT NULL DEFAULT 0 CHECK (Wymaga_zmiany_hasla IN (0,1)),
    Data_zapomnienia TEXT,
    ID_Kto_Zapomnial INTEGER,
    Zapomniany_Login_Enc TEXT,
    Zapomniane_Imie_Enc TEXT,
    Zapomniane_Nazwisko_Enc TEXT,
    FOREIGN KEY (ID_Adresu) REFERENCES Adres(ID_Adresu) ON DELETE RESTRICT,
    FOREIGN KEY (ID_Kto_Zapomnial) REFERENCES Uzytkownik(ID_Uzytkownika) ON DELETE SET NULL
);

-- ============================================================
-- 3. Tabela Historia_Hasel
-- ============================================================
CREATE TABLE IF NOT EXISTS Historia_Hasel (
    ID_Hasla INTEGER PRIMARY KEY AUTOINCREMENT,
    ID_Uzytkownika INTEGER NOT NULL,
    Haslo_Hash TEXT NOT NULL,
    Data_ustawienia TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ID_Uzytkownika) REFERENCES Uzytkownik(ID_Uzytkownika) ON DELETE CASCADE
);

-- ============================================================
-- 4. Tabela Uprawnienie
-- ============================================================
CREATE TABLE IF NOT EXISTS Uprawnienie (
    ID_Uprawnienia INTEGER PRIMARY KEY AUTOINCREMENT,
    Nazwa TEXT UNIQUE NOT NULL
);

-- ============================================================
-- 5. Tabela Uzytkownik_Uprawnienie (łączeniowa)
-- ============================================================
CREATE TABLE IF NOT EXISTS Uzytkownik_Uprawnienie (
    ID_Uzytkownika INTEGER NOT NULL,
    ID_Uprawnienia INTEGER NOT NULL,
    PRIMARY KEY (ID_Uzytkownika, ID_Uprawnienia),
    FOREIGN KEY (ID_Uzytkownika) REFERENCES Uzytkownik(ID_Uzytkownika) ON DELETE CASCADE,
    FOREIGN KEY (ID_Uprawnienia) REFERENCES Uprawnienie(ID_Uprawnienia) ON DELETE CASCADE
);

-- ============================================================
-- 6. Tabela Pacjent
-- ============================================================
CREATE TABLE IF NOT EXISTS Pacjent (
    ID_Pacjenta INTEGER PRIMARY KEY AUTOINCREMENT,
    ID_Adresu INTEGER NOT NULL,
    Imie TEXT NOT NULL,
    Nazwisko TEXT NOT NULL,
    PESEL TEXT NOT NULL UNIQUE,
    Adres_email TEXT,
    Numer_telefonu TEXT NOT NULL,
    Data_urodzenia TEXT NOT NULL,
    Plec TEXT NOT NULL CHECK (Plec IN ('K', 'M')),
    FOREIGN KEY (ID_Adresu) REFERENCES Adres(ID_Adresu) ON DELETE RESTRICT
);

-- ============================================================
-- Dodane: Specjalizacja, Lekarze, Gabinety, Wizyty
-- ============================================================

CREATE TABLE IF NOT EXISTS Specjalizacja (
    ID_Specjalizacji INTEGER PRIMARY KEY AUTOINCREMENT,
    Nazwa TEXT UNIQUE NOT NULL
);

CREATE TABLE IF NOT EXISTS Lekarz_Specjalizacja (
    ID_Uzytkownika INTEGER NOT NULL,
    ID_Specjalizacji INTEGER NOT NULL,
    PRIMARY KEY (ID_Uzytkownika, ID_Specjalizacji),
    FOREIGN KEY (ID_Uzytkownika) REFERENCES Uzytkownik(ID_Uzytkownika) ON DELETE CASCADE,
    FOREIGN KEY (ID_Specjalizacji) REFERENCES Specjalizacja(ID_Specjalizacji) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Gabinet (
    ID_Gabinetu INTEGER PRIMARY KEY AUTOINCREMENT,
    Nazwa_Numer TEXT UNIQUE NOT NULL
);

CREATE TABLE IF NOT EXISTS Wizyta (
    ID_Wizyty INTEGER PRIMARY KEY AUTOINCREMENT,
    ID_Pacjenta INTEGER NOT NULL,
    ID_Lekarza INTEGER NOT NULL,
    ID_Gabinetu INTEGER NOT NULL,
    Data_i_godzina_rozpoczecia TEXT NOT NULL,
    Status TEXT NOT NULL,
    Schorzenia_i_dolegliwosci TEXT,
    Zalecenia_i_lekarstwa TEXT,
    FOREIGN KEY (ID_Pacjenta) REFERENCES Pacjent(ID_Pacjenta) ON DELETE CASCADE,
    FOREIGN KEY (ID_Lekarza) REFERENCES Lekarz(ID_Uzytkownika) ON DELETE RESTRICT,
    FOREIGN KEY (ID_Gabinetu) REFERENCES Gabinet(ID_Gabinetu) ON DELETE RESTRICT
);

-- ============================================================
-- 7. Wstawienie domyślnego adresu
-- ============================================================
INSERT INTO Adres (Miejscowosc, Kod_pocztowy, Ulica, Numer_posesji_domu, Numer_lokalu_mieszkania)
SELECT '-', '-', '-', '-', '-'
WHERE NOT EXISTS (SELECT 1 FROM Adres WHERE ID_Adresu = 1);

-- ============================================================
-- 8. Wstawienie użytkownika SuperAdmin
-- ============================================================
INSERT INTO Uzytkownik (
    ID_Adresu, Login, Imie, Nazwisko, PESEL, Data_urodzenia, Plec, Adres_email, Numer_telefonu, Blokada_konta_do, Czy_zapomniany, Wymaga_zmiany_hasla, Data_zapomnienia, ID_Kto_Zapomnial
)
SELECT 
    1, 'SuperAdmin', '-', '-', '-', '2026-03-17', 'Inna', 'customer_service@ict_supplier.com', '000000000', NULL, 0, 0, NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM Uzytkownik WHERE Login = 'SuperAdmin');

-- ============================================================
-- 9. Wstawienie hasła dla SuperAdmina
-- ============================================================
INSERT INTO Historia_Hasel (ID_Uzytkownika, Haslo_Hash)
SELECT ID_Uzytkownika, 'AdminPass'
FROM Uzytkownik
WHERE Login = 'SuperAdmin'
  AND NOT EXISTS (SELECT 1 FROM Historia_Hasel h 
                  JOIN Uzytkownik u ON h.ID_Uzytkownika = u.ID_Uzytkownika 
                  WHERE u.Login = 'SuperAdmin');

-- ============================================================
-- 10. Dodanie uprawnień
-- ============================================================
INSERT INTO Uprawnienie (Nazwa) SELECT 'SuperAdmin' WHERE NOT EXISTS (SELECT 1 FROM Uprawnienie WHERE Nazwa = 'SuperAdmin');
INSERT INTO Uprawnienie (Nazwa) SELECT 'Admin' WHERE NOT EXISTS (SELECT 1 FROM Uprawnienie WHERE Nazwa = 'Admin');
INSERT INTO Uprawnienie (Nazwa) SELECT 'Lekarz' WHERE NOT EXISTS (SELECT 1 FROM Uprawnienie WHERE Nazwa = 'Lekarz');
INSERT INTO Uprawnienie (Nazwa) SELECT 'Recepcja' WHERE NOT EXISTS (SELECT 1 FROM Uprawnienie WHERE Nazwa = 'Recepcja');
INSERT INTO Uprawnienie (Nazwa) SELECT 'Brak_roli' WHERE NOT EXISTS (SELECT 1 FROM Uprawnienie WHERE Nazwa = 'Brak_roli');

-- ============================================================
-- 11. Nadanie uprawnienia 'Admin' dla SuperAdmina
-- ============================================================
INSERT INTO Uzytkownik_Uprawnienie (ID_Uzytkownika, ID_Uprawnienia)
SELECT u.ID_Uzytkownika, p.ID_Uprawnienia
FROM Uzytkownik u, Uprawnienie p
WHERE u.Login = 'SuperAdmin'
  AND p.Nazwa = 'SuperAdmin'
  AND NOT EXISTS (SELECT 1 FROM Uzytkownik_Uprawnienie up
                    WHERE up.ID_Uzytkownika = u.ID_Uzytkownika
                    AND up.ID_Uprawnienia = p.ID_Uprawnienia);
                    
-- ============================================================
-- 12. Domyślne dane słownikowe (Gabinety, Specjalizacje)
-- ============================================================
INSERT INTO Specjalizacja (Nazwa) SELECT 'Kardiolog' WHERE NOT EXISTS (SELECT 1 FROM Specjalizacja WHERE Nazwa = 'Kardiolog');
INSERT INTO Specjalizacja (Nazwa) SELECT 'Pediatra' WHERE NOT EXISTS (SELECT 1 FROM Specjalizacja WHERE Nazwa = 'Pediatra');
INSERT INTO Specjalizacja (Nazwa) SELECT 'Lekarz rodzinny' WHERE NOT EXISTS (SELECT 1 FROM Specjalizacja WHERE Nazwa = 'Lekarz rodzinny');
INSERT INTO Specjalizacja (Nazwa) SELECT 'Neurolog' WHERE NOT EXISTS (SELECT 1 FROM Specjalizacja WHERE Nazwa = 'Neurolog');

INSERT INTO Gabinet (Nazwa_Numer) SELECT 'Gabinet 101' WHERE NOT EXISTS (SELECT 1 FROM Gabinet WHERE Nazwa_Numer = 'Gabinet 101');
INSERT INTO Gabinet (Nazwa_Numer) SELECT 'Gabinet 102' WHERE NOT EXISTS (SELECT 1 FROM Gabinet WHERE Nazwa_Numer = 'Gabinet 102');
INSERT INTO Gabinet (Nazwa_Numer) SELECT 'Gabinet Zabiegowy' WHERE NOT EXISTS (SELECT 1 FROM Gabinet WHERE Nazwa_Numer = 'Gabinet Zabiegowy');
";
            createTableCmd.ExecuteNonQuery();

            EnsureColumnExists(connection, "Uzytkownik", "Zapomniany_Login_Enc", "TEXT");
            EnsureColumnExists(connection, "Uzytkownik", "Zapomniane_Imie_Enc", "TEXT");
            EnsureColumnExists(connection, "Uzytkownik", "Zapomniane_Nazwisko_Enc", "TEXT");
            MigrateLegacyPatients(connection);
        }

        private static void EnsureColumnExists(SqliteConnection connection, string tableName, string columnName, string columnDefinition)
        {
            var checkCommand = connection.CreateCommand();
            checkCommand.CommandText = $"PRAGMA table_info({tableName});";

            using (var reader = checkCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (string.Equals(reader.GetString(1), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        return;
                    }
                }
            }

            var alterCommand = connection.CreateCommand();
            alterCommand.CommandText = $"ALTER TABLE {tableName} ADD COLUMN {columnName} {columnDefinition};";
            alterCommand.ExecuteNonQuery();
        }

        private static void MigrateLegacyPatients(SqliteConnection connection)
        {
            var migrationCommand = connection.CreateCommand();
            migrationCommand.CommandText = @"
BEGIN TRANSACTION;

INSERT INTO Pacjent (
    ID_Adresu,
    Imie,
    Nazwisko,
    PESEL,
    Adres_email,
    Numer_telefonu,
    Data_urodzenia,
    Plec
)
SELECT
    u.ID_Adresu,
    u.Imie,
    u.Nazwisko,
    u.PESEL,
    u.Adres_email,
    u.Numer_telefonu,
    u.Data_urodzenia,
    CASE
        WHEN u.Plec IN ('K', 'M') THEN u.Plec
        ELSE 'K'
    END
FROM Uzytkownik u
WHERE u.Czy_zapomniany = 0
  AND EXISTS (
      SELECT 1
      FROM Uzytkownik_Uprawnienie uu
      JOIN Uprawnienie up ON up.ID_Uprawnienia = uu.ID_Uprawnienia
      WHERE uu.ID_Uzytkownika = u.ID_Uzytkownika
        AND up.Nazwa = 'Brak_roli'
  )
  AND NOT EXISTS (
      SELECT 1
      FROM Pacjent p
      WHERE p.PESEL = u.PESEL
  );

DELETE FROM Uzytkownik
WHERE Czy_zapomniany = 0
  AND EXISTS (
      SELECT 1
      FROM Uzytkownik_Uprawnienie uu
      JOIN Uprawnienie up ON up.ID_Uprawnienia = uu.ID_Uprawnienia
      WHERE uu.ID_Uzytkownika = Uzytkownik.ID_Uzytkownika
        AND up.Nazwa = 'Brak_roli'
  );

COMMIT;";
            migrationCommand.ExecuteNonQuery();
        }
    }
}