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

            if (noweUprawnienia.Exists(u =>u.Posiadane == true && u.Id != 5))
            {
                // Jeśli użytkownik ma uprawnienie inne niż "Brak roli" usuwamy "Brak roli" na poziome aplikacja
                noweUprawnienia.Find(u => u.Id == 5).Posiadane = false;
            }


            // Następnie dodajemy nowe uprawnienia
            if (noweUprawnienia.Exists(u => u.Posiadane == true))
                foreach (Uprawnienie uprawnienie in noweUprawnienia)
                {
                    if (uprawnienie.Posiadane == false)
                        continue; // pomijamy uprawnienia, które nie są zaznaczone jako posiadane lub jest to uprawnienie Brak_roli)
                    var insertCommand = connection.CreateCommand();
                    insertCommand.CommandText = @"
    INSERT INTO Uzytkownik_Uprawnienie (ID_Uzytkownika, ID_Uprawnienia)
    VALUES ( $userId , $uprawnienieId);
    ";
                    insertCommand.Parameters.AddWithValue("$userId", userId);
                    insertCommand.Parameters.AddWithValue("$uprawnienieId", uprawnienie.Id);
                    insertCommand.ExecuteNonQuery();
                }
            else // Jeśli użytkownik nie ma żadnych uprawnień, dodajemy uprawnienie "Brak roli"
            {
                var insertCommand = connection.CreateCommand();
                insertCommand.CommandText = @"
    INSERT INTO Uzytkownik_Uprawnienie (ID_Uzytkownika, ID_Uprawnienia)
    VALUES ( $userId , 5);
    ";
                insertCommand.Parameters.AddWithValue("$userId", userId);
                insertCommand.ExecuteNonQuery();
            }
        }

    }
}
