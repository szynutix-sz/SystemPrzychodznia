using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemPrzychodznia.Data
{
    internal partial class UserRepository
    {
        public void ZmieńSpecjalizacje(int userId, List<Specjalizacja> noweSpecjalizacje)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();

            // Najpierw usuwamy wszystkie obecne specjalizacje użytkownika
            var deleteCommand = connection.CreateCommand();
            deleteCommand.CommandText = @"
DELETE FROM Lekarz_Specjalizacja
WHERE ID_Uzytkownika = $userId;";
            deleteCommand.Parameters.AddWithValue("$userId", userId);
            deleteCommand.ExecuteNonQuery();


            // Następnie dodajemy nowe specjalizacje
            if (noweSpecjalizacje.Exists(s => s.Posiadane == true))
                foreach (Specjalizacja specjalizacja in noweSpecjalizacje)
                {
                    if (specjalizacja.Posiadane == false)
                        continue; // pomijamy specjalizacje, które nie są zaznaczone jako posiadane
                    var insertCommand = connection.CreateCommand();
                    insertCommand.CommandText = @"
    INSERT INTO Lekarz_Specjalizacja (ID_Uzytkownika, ID_Specjalizacji)
    VALUES ( $userId , $specjalizacjaId);
    ";
                    insertCommand.Parameters.AddWithValue("$userId", userId);
                    insertCommand.Parameters.AddWithValue("$specjalizacjaId", specjalizacja.Id);
                    insertCommand.ExecuteNonQuery();
                }
        }
    }
}
