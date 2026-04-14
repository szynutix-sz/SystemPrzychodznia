using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemPrzychodznia.Data
{
    internal partial class UserRepository
    {
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

        public void EditUser(UserFull user)
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

    }
}
