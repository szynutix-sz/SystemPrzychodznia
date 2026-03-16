using Microsoft.Data.Sqlite;

namespace SystemPrzychodznia.Data
{
    public static class DatabaseInitializer
    {
        private const string ConnectionString = "Data Source=przychodnia.db";

        public static void Initialize()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            var createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Login TEXT NOT NULL UNIQUE,
                    FirstName TEXT NOT NULL,
                    LastName TEXT NOT NULL,
                    Email TEXT NOT NULL UNIQUE,
                    Phone TEXT,
                    Role TEXT NOT NULL
                )";
            createTableCmd.ExecuteNonQuery();
        }
    }
}