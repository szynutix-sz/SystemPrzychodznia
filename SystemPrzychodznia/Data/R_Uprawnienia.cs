using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemPrzychodznia.Data
{
    internal partial class UserRepository
    {
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
                if (uprawnienie.Posiadane == false)
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

    }
}
