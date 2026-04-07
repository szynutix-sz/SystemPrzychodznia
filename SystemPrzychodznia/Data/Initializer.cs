using Microsoft.Data.Sqlite;

namespace SystemPrzychodznia.Data
{
    internal static class DatabaseInitializer
    {
        private const string ConnectionString = "Data Source=przychodnia.db";

        public static void Initialize()
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
    Ulica TEXT NOT NULL,
    Numer_posesji_domu TEXT NOT NULL,
    Numer_lokalu_mieszkania TEXT
);

-- ============================================================
-- 2. Tabela Uzytkownik
-- ============================================================
CREATE TABLE IF NOT EXISTS Uzytkownik (
    ID_Uzytkownika INTEGER PRIMARY KEY AUTOINCREMENT,
    ID_Adresu INTEGER NOT NULL,
    Login TEXT UNIQUE NOT NULL,
    Imie TEXT NOT NULL,
    Nazwisko TEXT NOT NULL,
    PESEL TEXT UNIQUE NOT NULL,
    Data_urodzenia TEXT NOT NULL,
    Plec TEXT NOT NULL CHECK (Plec IN ('K', 'M', 'Inna')),
    Adres_email TEXT UNIQUE NOT NULL,
    Numer_telefonu TEXT NOT NULL,
    Blokada_konta_do TEXT,
    Czy_zapomniany INTEGER NOT NULL DEFAULT 0 CHECK (Czy_zapomniany IN (0,1)),
    Data_zapomnienia TEXT,
    ID_Kto_Zapomnial INTEGER,
    FOREIGN KEY (ID_Adresu) REFERENCES Adres(ID_Adresu) ON DELETE RESTRICT,
    FOREIGN KEY (ID_Kto_Zapomnial) REFERENCES Uzytkownik(ID_Uzytkownika) ON DELETE SET NULL
);

-- ============================================================
-- 3. Tabela Historia_Hasel (przechowuje hasła)
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
-- 6. Wstawienie domyślnego adresu (jeśli potrzeba dla SuperAdmina)
-- ============================================================
INSERT INTO Adres (Miejscowosc, Kod_pocztowy, Ulica, Numer_posesji_domu, Numer_lokalu_mieszkania)
SELECT '-', '-', '-', '-', '-'
WHERE NOT EXISTS (SELECT 1 FROM Adres WHERE ID_Adresu = 1);

-- ============================================================
-- 7. Wstawienie użytkownika SuperAdmin (jeśli nie istnieje)
-- ============================================================
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
SELECT 
    1,                                    -- ID_Adresu (zakładamy, że pierwszy adres ma ID=1)
    'SuperAdmin',
    '-',
    '-',
    '-',
    '2026-03-17',                         -- format YYYY-MM-DD
    'Inna',                                  -- Plec (wartość dozwolona: K/M/Inna, tu '-' nie jest dozwolone – poprawiam na 'Inna')
    'customer_service@ict_supplier.com',
    '000000000',
    NULL,                                 -- brak blokady
    0,                                    -- nie zapomniany
    NULL,
    NULL
WHERE NOT EXISTS (SELECT 1 FROM Uzytkownik WHERE Login = 'SuperAdmin');

-- ============================================================
-- 8. Wstawienie hasła dla SuperAdmina (jeśli użytkownik został dodany)
--    UWAGA: W rzeczywistej aplikacji hasło powinno być silnie zahaszowane
--    (np. bcrypt, Argon2). Poniższy zapis to tylko przykład.
-- ============================================================
INSERT INTO Historia_Hasel (ID_Uzytkownika, Haslo_Hash)
SELECT ID_Uzytkownika, 'AdminPass'   -- W PRODUKCJI: użyj hasha np. '$2y$10$...' 
FROM Uzytkownik
WHERE Login = 'SuperAdmin'
  AND NOT EXISTS (SELECT 1 FROM Historia_Hasel h 
                  JOIN Uzytkownik u ON h.ID_Uzytkownika = u.ID_Uzytkownika 
                  WHERE u.Login = 'SuperAdmin');

-- ============================================================
-- 9. Dodanie uprawnienia 'Admin' (jeśli nie istnieje)
-- ============================================================
INSERT INTO Uprawnienie (Nazwa)
SELECT 'Admin'
WHERE NOT EXISTS (SELECT 1 FROM Uprawnienie WHERE Nazwa = 'Admin');

-- ============================================================
-- 10. Nadanie uprawnienia 'Admin' dla SuperAdmina
-- ============================================================
INSERT INTO Uzytkownik_Uprawnienie (ID_Uzytkownika, ID_Uprawnienia)
SELECT u.ID_Uzytkownika, p.ID_Uprawnienia
FROM Uzytkownik u, Uprawnienie p
WHERE u.Login = 'SuperAdmin'
  AND p.Nazwa = 'Admin'
  AND NOT EXISTS (SELECT 1 FROM Uzytkownik_Uprawnienie up
                  WHERE up.ID_Uzytkownika = u.ID_Uzytkownika
                    AND up.ID_Uprawnienia = p.ID_Uprawnienia);";
            createTableCmd.ExecuteNonQuery();



        }
    }
}