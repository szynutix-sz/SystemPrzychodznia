using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemPrzychodznia.Data
{
    internal static partial class DatabaseInitializer
    {
        public static void InitializeTestData()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            var createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText = @"
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
