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
-- ============================================================
INSERT INTO Historia_Hasel (ID_Uzytkownika, Haslo_Hash)
SELECT ID_Uzytkownika, 'AdminPass'   -- W PRODUKCJI: użyj hasha np. '$2y$10$...' 
FROM Uzytkownik
WHERE Login = 'SuperAdmin'
  AND NOT EXISTS (SELECT 1 FROM Historia_Hasel h 
                  JOIN Uzytkownik u ON h.ID_Uzytkownika = u.ID_Uzytkownika 
                  WHERE u.Login = 'SuperAdmin');

-- ============================================================
-- 9. Dodanie uprawnień' (jeśli nie istnieją)
-- ============================================================
INSERT INTO Uprawnienie (Nazwa)
SELECT 'SuperAdmin'
WHERE NOT EXISTS (SELECT 1 FROM Uprawnienie WHERE Nazwa = 'SuperAdmin');


INSERT INTO Uprawnienie (Nazwa)
SELECT 'Admin'
WHERE NOT EXISTS (SELECT 1 FROM Uprawnienie WHERE Nazwa = 'Admin');

INSERT INTO Uprawnienie (Nazwa)
SELECT 'Lekarz'
WHERE NOT EXISTS (SELECT 1 FROM Uprawnienie WHERE Nazwa = 'Lekarz');

INSERT INTO Uprawnienie (Nazwa)
SELECT 'Recepcja'
WHERE NOT EXISTS (SELECT 1 FROM Uprawnienie WHERE Nazwa = 'Recepcja');

INSERT INTO Uprawnienie (Nazwa)
SELECT 'Brak_roli'
WHERE NOT EXISTS (SELECT 1 FROM Uprawnienie WHERE Nazwa = 'Brak_roli');

-- ============================================================
-- 10. Nadanie uprawnienia 'SuperAdmin' dla SuperAdmina
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
-- 11. UŻYTKOWNICY TESTOWI
-- ============================================================

-- Test 1: Anna Nowak (Recepcja), ur. 1990-05-20, K, PESEL: 90052001241
INSERT INTO Adres (Miejscowosc, Kod_pocztowy, Ulica, Numer_posesji_domu, Numer_lokalu_mieszkania)
SELECT 'Warszawa', '00001', 'Kwiatowa', '5', '2'
WHERE NOT EXISTS (SELECT 1 FROM Uzytkownik WHERE Adres_email = 'anna.nowak@test.pl');

INSERT INTO Uzytkownik (ID_Adresu, Login, Imie, Nazwisko, PESEL, Data_urodzenia, Plec, Adres_email, Numer_telefonu, Blokada_konta_do, Czy_zapomniany, Data_zapomnienia, ID_Kto_Zapomnial)
SELECT last_insert_rowid(), 'anna.nowak', 'Anna', 'Nowak', '90052001241', '1990-05-20', 'K', 'anna.nowak@test.pl', '600100200', NULL, 0, NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM Uzytkownik WHERE Adres_email = 'anna.nowak@test.pl');

INSERT INTO Historia_Hasel (ID_Uzytkownika, Haslo_Hash)
SELECT ID_Uzytkownika, 'Pass123' FROM Uzytkownik WHERE Login = 'anna.nowak'
  AND NOT EXISTS (SELECT 1 FROM Historia_Hasel h JOIN Uzytkownik u ON h.ID_Uzytkownika = u.ID_Uzytkownika WHERE u.Login = 'anna.nowak');

INSERT INTO Uzytkownik_Uprawnienie (ID_Uzytkownika, ID_Uprawnienia)
SELECT u.ID_Uzytkownika, p.ID_Uprawnienia FROM Uzytkownik u, Uprawnienie p
WHERE u.Login = 'anna.nowak' AND p.Nazwa = 'Recepcja'
  AND NOT EXISTS (SELECT 1 FROM Uzytkownik_Uprawnienie up WHERE up.ID_Uzytkownika = u.ID_Uzytkownika AND up.ID_Uprawnienia = p.ID_Uprawnienia);

-- Test 2: Jan Kowalski (Lekarz), ur. 1985-10-10, M, PESEL: 85101004577
INSERT INTO Adres (Miejscowosc, Kod_pocztowy, Ulica, Numer_posesji_domu, Numer_lokalu_mieszkania)
SELECT 'Kraków', '30002', 'Lekarska', '10', ''
WHERE NOT EXISTS (SELECT 1 FROM Uzytkownik WHERE Adres_email = 'jan.kowalski@test.pl');

INSERT INTO Uzytkownik (ID_Adresu, Login, Imie, Nazwisko, PESEL, Data_urodzenia, Plec, Adres_email, Numer_telefonu, Blokada_konta_do, Czy_zapomniany, Data_zapomnienia, ID_Kto_Zapomnial)
SELECT last_insert_rowid(), 'jan.kowalski', 'Jan', 'Kowalski', '85101004577', '1985-10-10', 'M', 'jan.kowalski@test.pl', '511200300', NULL, 0, NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM Uzytkownik WHERE Adres_email = 'jan.kowalski@test.pl');

INSERT INTO Historia_Hasel (ID_Uzytkownika, Haslo_Hash)
SELECT ID_Uzytkownika, 'Pass123' FROM Uzytkownik WHERE Login = 'jan.kowalski'
  AND NOT EXISTS (SELECT 1 FROM Historia_Hasel h JOIN Uzytkownik u ON h.ID_Uzytkownika = u.ID_Uzytkownika WHERE u.Login = 'jan.kowalski');

INSERT INTO Uzytkownik_Uprawnienie (ID_Uzytkownika, ID_Uprawnienia)
SELECT u.ID_Uzytkownika, p.ID_Uprawnienia FROM Uzytkownik u, Uprawnienie p
WHERE u.Login = 'jan.kowalski' AND p.Nazwa = 'Lekarz'
  AND NOT EXISTS (SELECT 1 FROM Uzytkownik_Uprawnienie up WHERE up.ID_Uzytkownika = u.ID_Uzytkownika AND up.ID_Uprawnienia = p.ID_Uprawnienia);

-- Test 3: Maria Wiśniewska (Brak_roli), ur. 2000-03-15, K, PESEL: 00231502340
INSERT INTO Adres (Miejscowosc, Kod_pocztowy, Ulica, Numer_posesji_domu, Numer_lokalu_mieszkania)
SELECT 'Gdańsk', '80001', 'Morska', '3', '7'
WHERE NOT EXISTS (SELECT 1 FROM Uzytkownik WHERE Adres_email = 'maria.wisniewska@test.pl');

INSERT INTO Uzytkownik (ID_Adresu, Login, Imie, Nazwisko, PESEL, Data_urodzenia, Plec, Adres_email, Numer_telefonu, Blokada_konta_do, Czy_zapomniany, Data_zapomnienia, ID_Kto_Zapomnial)
SELECT last_insert_rowid(), 'maria.wisniewska', 'Maria', 'Wiśniewska', '00231502340', '2000-03-15', 'K', 'maria.wisniewska@test.pl', '700300400', NULL, 0, NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM Uzytkownik WHERE Adres_email = 'maria.wisniewska@test.pl');

INSERT INTO Historia_Hasel (ID_Uzytkownika, Haslo_Hash)
SELECT ID_Uzytkownika, 'Pass123' FROM Uzytkownik WHERE Login = 'maria.wisniewska'
  AND NOT EXISTS (SELECT 1 FROM Historia_Hasel h JOIN Uzytkownik u ON h.ID_Uzytkownika = u.ID_Uzytkownika WHERE u.Login = 'maria.wisniewska');

INSERT INTO Uzytkownik_Uprawnienie (ID_Uzytkownika, ID_Uprawnienia)
SELECT u.ID_Uzytkownika, p.ID_Uprawnienia FROM Uzytkownik u, Uprawnienie p
WHERE u.Login = 'maria.wisniewska' AND p.Nazwa = 'Brak_roli'
  AND NOT EXISTS (SELECT 1 FROM Uzytkownik_Uprawnienie up WHERE up.ID_Uzytkownika = u.ID_Uzytkownika AND up.ID_Uprawnienia = p.ID_Uprawnienia);

-- Test 4: Piotr Wójcik (Admin), ur. 1975-07-22, M, PESEL: 75072205637
INSERT INTO Adres (Miejscowosc, Kod_pocztowy, Ulica, Numer_posesji_domu, Numer_lokalu_mieszkania)
SELECT 'Poznań', '60001', 'Poznańska', '22', ''
WHERE NOT EXISTS (SELECT 1 FROM Uzytkownik WHERE Adres_email = 'piotr.wojcik@test.pl');

INSERT INTO Uzytkownik (ID_Adresu, Login, Imie, Nazwisko, PESEL, Data_urodzenia, Plec, Adres_email, Numer_telefonu, Blokada_konta_do, Czy_zapomniany, Data_zapomnienia, ID_Kto_Zapomnial)
SELECT last_insert_rowid(), 'piotr.wojcik', 'Piotr', 'Wójcik', '75072205637', '1975-07-22', 'M', 'piotr.wojcik@test.pl', '888400500', NULL, 0, NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM Uzytkownik WHERE Adres_email = 'piotr.wojcik@test.pl');

INSERT INTO Historia_Hasel (ID_Uzytkownika, Haslo_Hash)
SELECT ID_Uzytkownika, 'Pass123' FROM Uzytkownik WHERE Login = 'piotr.wojcik'
  AND NOT EXISTS (SELECT 1 FROM Historia_Hasel h JOIN Uzytkownik u ON h.ID_Uzytkownika = u.ID_Uzytkownika WHERE u.Login = 'piotr.wojcik');

INSERT INTO Uzytkownik_Uprawnienie (ID_Uzytkownika, ID_Uprawnienia)
SELECT u.ID_Uzytkownika, p.ID_Uprawnienia FROM Uzytkownik u, Uprawnienie p
WHERE u.Login = 'piotr.wojcik' AND p.Nazwa = 'Admin'
  AND NOT EXISTS (SELECT 1 FROM Uzytkownik_Uprawnienie up WHERE up.ID_Uzytkownika = u.ID_Uzytkownika AND up.ID_Uprawnienia = p.ID_Uprawnienia);

-- Test 5: Katarzyna Kamińska (Brak_roli), ur. 1995-12-03, K, PESEL: 95120308901
INSERT INTO Adres (Miejscowosc, Kod_pocztowy, Ulica, Numer_posesji_domu, Numer_lokalu_mieszkania)
SELECT 'Wrocław', '50001', 'Wrocławska', '8', '4'
WHERE NOT EXISTS (SELECT 1 FROM Uzytkownik WHERE Adres_email = 'katarzyna.kaminska@test.pl');

INSERT INTO Uzytkownik (ID_Adresu, Login, Imie, Nazwisko, PESEL, Data_urodzenia, Plec, Adres_email, Numer_telefonu, Blokada_konta_do, Czy_zapomniany, Data_zapomnienia, ID_Kto_Zapomnial)
SELECT last_insert_rowid(), 'katarzyna.kaminska', 'Katarzyna', 'Kamińska', '95120308901', '1995-12-03', 'K', 'katarzyna.kaminska@test.pl', '900500600', NULL, 0, NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM Uzytkownik WHERE Adres_email = 'katarzyna.kaminska@test.pl');

INSERT INTO Historia_Hasel (ID_Uzytkownika, Haslo_Hash)
SELECT ID_Uzytkownika, 'Pass123' FROM Uzytkownik WHERE Login = 'katarzyna.kaminska'
  AND NOT EXISTS (SELECT 1 FROM Historia_Hasel h JOIN Uzytkownik u ON h.ID_Uzytkownika = u.ID_Uzytkownika WHERE u.Login = 'katarzyna.kaminska');

INSERT INTO Uzytkownik_Uprawnienie (ID_Uzytkownika, ID_Uprawnienia)
SELECT u.ID_Uzytkownika, p.ID_Uprawnienia FROM Uzytkownik u, Uprawnienie p
WHERE u.Login = 'katarzyna.kaminska' AND p.Nazwa = 'Brak_roli'
  AND NOT EXISTS (SELECT 1 FROM Uzytkownik_Uprawnienie up WHERE up.ID_Uzytkownika = u.ID_Uzytkownika AND up.ID_Uprawnienia = p.ID_Uprawnienia);";
            createTableCmd.ExecuteNonQuery();



        }
    }
}