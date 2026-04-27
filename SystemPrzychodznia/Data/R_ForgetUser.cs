using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemPrzychodznia.Data
{
    internal partial class UserRepository
    {
        public void ForgetUser(ForgottenUser user, int forgottenBy)
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
UPDATE Uzytkownik 
SET 
    Login = $randLogin,
    Imie = $randFirstName,
    Nazwisko = $randLastName,
    Zapomniany_Login_Enc = $originalLogin,
    Zapomniane_Imie_Enc = $originalFirstName,
    Zapomniane_Nazwisko_Enc = $originalLastName,
    PESEL = $pesel,
    Data_urodzenia = $randDate,
    Plec = $randGender,
    Czy_zapomniany = 1,
    Data_zapomnienia = CURRENT_TIMESTAMP,
    ID_Kto_Zapomnial = $adminId
WHERE ID_Uzytkownika = $id;";

                cmdUser.Parameters.AddWithValue("$randLogin", user.Login);
                cmdUser.Parameters.AddWithValue("$randFirstName", user.FirstName);
                cmdUser.Parameters.AddWithValue("$randLastName", user.LastName);
                cmdUser.Parameters.AddWithValue("$originalLogin", EncryptForgottenSearchValue(user.OriginalLogin));
                cmdUser.Parameters.AddWithValue("$originalFirstName", EncryptForgottenSearchValue(user.OriginalFirstName));
                cmdUser.Parameters.AddWithValue("$originalLastName", EncryptForgottenSearchValue(user.OriginalLastName));
                cmdUser.Parameters.AddWithValue("$pesel", user.PESEL);
                cmdUser.Parameters.AddWithValue("$randDate", user.BirthDate);
                cmdUser.Parameters.AddWithValue("$randGender", user.Gender);
                cmdUser.Parameters.AddWithValue("$adminId", forgottenBy);
                cmdUser.Parameters.AddWithValue("$id", user.Id);
                cmdUser.ExecuteNonQuery();

                //Usunięcie uprawnień
                var deleteCommand = connection.CreateCommand();
                deleteCommand.Transaction = transaction;
                deleteCommand.CommandText = @"
DELETE FROM Uzytkownik_Uprawnienie 
WHERE ID_Uzytkownika = $userId
    AND ID_Uprawnienia != 
        (SELECT ID_Uprawnienia FROM Uprawnienie WHERE Nazwa = 'SuperAdmin');";
                deleteCommand.Parameters.AddWithValue("$userId", user.Id);
                deleteCommand.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (SqliteException) { transaction.Rollback(); throw; }
            catch (Exception) { transaction.Rollback(); throw; }
        }
    }
}
