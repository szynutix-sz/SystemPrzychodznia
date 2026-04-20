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
  AND NOT EXISTS (SELECT 1 FROM Uzytkownik_Uprawnienie up WHERE up.ID_Uzytkownika = u.ID_Uzytkownika AND up.ID_Uprawnienia = p.ID_Uprawnienia);

-- Test 6: Tomasz Zieliński (Recepcja), ur. 1988-02-11, M, PESEL: 88021103456
INSERT INTO Adres (Miejscowosc, Kod_pocztowy, Ulica, Numer_posesji_domu, Numer_lokalu_mieszkania)
SELECT 'Lublin', '20001', 'Słoneczna', '12', '1'
WHERE NOT EXISTS (SELECT 1 FROM Uzytkownik WHERE Adres_email = 'tomasz.zielinski@test.pl');

INSERT INTO Uzytkownik (ID_Adresu, Login, Imie, Nazwisko, PESEL, Data_urodzenia, Plec, Adres_email, Numer_telefonu, Blokada_konta_do, Czy_zapomniany, Data_zapomnienia, ID_Kto_Zapomnial)
SELECT last_insert_rowid(), 'tomasz.zielinski', 'Tomasz', 'Zieliński', '88021103456', '1988-02-11', 'M', 'tomasz.zielinski@test.pl', '600700800', NULL, 0, NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM Uzytkownik WHERE Adres_email = 'tomasz.zielinski@test.pl');

INSERT INTO Historia_Hasel (ID_Uzytkownika, Haslo_Hash)
SELECT ID_Uzytkownika, 'Pass123' FROM Uzytkownik WHERE Login = 'tomasz.zielinski'
  AND NOT EXISTS (SELECT 1 FROM Historia_Hasel h JOIN Uzytkownik u ON h.ID_Uzytkownika = u.ID_Uzytkownika WHERE u.Login = 'tomasz.zielinski');

INSERT INTO Uzytkownik_Uprawnienie (ID_Uzytkownika, ID_Uprawnienia)
SELECT u.ID_Uzytkownika, p.ID_Uprawnienia FROM Uzytkownik u, Uprawnienie p
WHERE u.Login = 'tomasz.zielinski' AND p.Nazwa = 'Recepcja'
  AND NOT EXISTS (SELECT 1 FROM Uzytkownik_Uprawnienie up WHERE up.ID_Uzytkownika = u.ID_Uzytkownika AND up.ID_Uprawnienia = p.ID_Uprawnienia);

-- Test 7: Agnieszka Zalewska (Lekarz), ur. 1992-09-09, K, PESEL: 92090912345
INSERT INTO Adres (Miejscowosc, Kod_pocztowy, Ulica, Numer_posesji_domu, Numer_lokalu_mieszkania)
SELECT 'Szczecin', '70001', 'Jasna', '4', ''
WHERE NOT EXISTS (SELECT 1 FROM Uzytkownik WHERE Adres_email = 'agnieszka.zalewska@test.pl');

INSERT INTO Uzytkownik (ID_Adresu, Login, Imie, Nazwisko, PESEL, Data_urodzenia, Plec, Adres_email, Numer_telefonu, Blokada_konta_do, Czy_zapomniany, Data_zapomnienia, ID_Kto_Zapomnial)
SELECT last_insert_rowid(), 'agnieszka.zalewska', 'Agnieszka', 'Zalewska', '92090912345', '1992-09-09', 'K', 'agnieszka.zalewska@test.pl', '502600700', NULL, 0, NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM Uzytkownik WHERE Adres_email = 'agnieszka.zalewska@test.pl');

INSERT INTO Historia_Hasel (ID_Uzytkownika, Haslo_Hash)
SELECT ID_Uzytkownika, 'Pass123' FROM Uzytkownik WHERE Login = 'agnieszka.zalewska'
  AND NOT EXISTS (SELECT 1 FROM Historia_Hasel h JOIN Uzytkownik u ON h.ID_Uzytkownika = u.ID_Uzytkownika WHERE u.Login = 'agnieszka.zalewska');

INSERT INTO Uzytkownik_Uprawnienie (ID_Uzytkownika, ID_Uprawnienia)
SELECT u.ID_Uzytkownika, p.ID_Uprawnienia FROM Uzytkownik u, Uprawnienie p
WHERE u.Login = 'agnieszka.zalewska' AND p.Nazwa = 'Lekarz'
  AND NOT EXISTS (SELECT 1 FROM Uzytkownik_Uprawnienie up WHERE up.ID_Uzytkownika = u.ID_Uzytkownika AND up.ID_Uprawnienia = p.ID_Uprawnienia);

-- Test 8: Bartosz Nowicki (Brak_roli), ur. 2001-06-30, M, PESEL: 01063056789
INSERT INTO Adres (Miejscowosc, Kod_pocztowy, Ulica, Numer_posesji_domu, Numer_lokalu_mieszkania)
SELECT 'Białystok', '15001', 'Leśna', '6', ''
WHERE NOT EXISTS (SELECT 1 FROM Uzytkownik WHERE Adres_email = 'bartosz.nowicki@test.pl');

INSERT INTO Uzytkownik (ID_Adresu, Login, Imie, Nazwisko, PESEL, Data_urodzenia, Plec, Adres_email, Numer_telefonu, Blokada_konta_do, Czy_zapomniany, Data_zapomnienia, ID_Kto_Zapomnial)
SELECT last_insert_rowid(), 'bartosz.nowicki', 'Bartosz', 'Nowicki', '01063056789', '2001-06-30', 'M', 'bartosz.nowicki@test.pl', '601800900', NULL, 0, NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM Uzytkownik WHERE Adres_email = 'bartosz.nowicki@test.pl');

INSERT INTO Historia_Hasel (ID_Uzytkownika, Haslo_Hash)
SELECT ID_Uzytkownika, 'Pass123' FROM Uzytkownik WHERE Login = 'bartosz.nowicki'
  AND NOT EXISTS (SELECT 1 FROM Historia_Hasel h JOIN Uzytkownik u ON h.ID_Uzytkownika = u.ID_Uzytkownika WHERE u.Login = 'bartosz.nowicki');

INSERT INTO Uzytkownik_Uprawnienie (ID_Uzytkownika, ID_Uprawnienia)
SELECT u.ID_Uzytkownika, p.ID_Uprawnienia FROM Uzytkownik u, Uprawnienie p
WHERE u.Login = 'bartosz.nowicki' AND p.Nazwa = 'Brak_roli'
  AND NOT EXISTS (SELECT 1 FROM Uzytkownik_Uprawnienie up WHERE up.ID_Uzytkownika = u.ID_Uzytkownika AND up.ID_Uprawnienia = p.ID_Uprawnienia);

-- Test 9: Monika Szymczak (Admin), ur. 1980-11-11, K, PESEL: 80111133445
INSERT INTO Adres (Miejscowosc, Kod_pocztowy, Ulica, Numer_posesji_domu, Numer_lokalu_mieszkania)
SELECT 'Katowice', '40001', 'Główna', '1', ''
WHERE NOT EXISTS (SELECT 1 FROM Uzytkownik WHERE Adres_email = 'monika.szymczak@test.pl');

INSERT INTO Uzytkownik (ID_Adresu, Login, Imie, Nazwisko, PESEL, Data_urodzenia, Plec, Adres_email, Numer_telefonu, Blokada_konta_do, Czy_zapomniany, Data_zapomnienia, ID_Kto_Zapomnial)
SELECT last_insert_rowid(), 'monika.szymczak', 'Monika', 'Szymczak', '80111133445', '1980-11-11', 'K', 'monika.szymczak@test.pl', '505900100', NULL, 0, NULL, NULL
WHERE NOT EXISTS (SELECT 1 FROM Uzytkownik WHERE Adres_email = 'monika.szymczak@test.pl');

INSERT INTO Historia_Hasel (ID_Uzytkownika, Haslo_Hash)
SELECT ID_Uzytkownika, 'Pass123' FROM Uzytkownik WHERE Login = 'monika.szymczak'
  AND NOT EXISTS (SELECT 1 FROM Historia_Hasel h JOIN Uzytkownik u ON h.ID_Uzytkownika = u.ID_Uzytkownika WHERE u.Login = 'monika.szymczak');

INSERT INTO Uzytkownik_Uprawnienie (ID_Uzytkownika, ID_Uprawnienia)
SELECT u.ID_Uzytkownika, p.ID_Uprawnienia FROM Uzytkownik u, Uprawnienie p
WHERE u.Login = 'monika.szymczak' AND p.Nazwa = 'Admin'
  AND NOT EXISTS (SELECT 1 FROM Uzytkownik_Uprawnienie up WHERE up.ID_Uzytkownika = u.ID_Uzytkownika AND up.ID_Uprawnienia = p.ID_Uprawnienia);";
            createTableCmd.ExecuteNonQuery();


        }
    }
}
