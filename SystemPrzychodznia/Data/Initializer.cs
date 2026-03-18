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
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Login TEXT NOT NULL UNIQUE,
                    FirstName TEXT NOT NULL,
                    LastName TEXT NOT NULL,
                    Locality TEXT NOT NULL,
                    PostalCode TEXT NOT NULL,
                    Street TEXT,
                    PropertyNumber TEXT NOT NULL,
                    HouseUnitNumber TEXT,
                    PESEL TEXT NOT NULL UNIQUE,
                    BirthDate TEXT NOT NULL,
                    Gender TEXT NOT NULL,
                    Email TEXT NOT NULL UNIQUE,
                    Phone TEXT NOT NULL,
                    Password TEXT NOT NULL,
                    Status TEXT NOT NULL DEFAULT 'A'
                )";
            createTableCmd.ExecuteNonQuery();

            var createAdminCmd = connection.CreateCommand();
            createAdminCmd.CommandText = @"
                INSERT INTO Users (Login, FirstName, LastName, Locality, PostalCode, Street, PropertyNumber, HouseUnitNumber, PESEL,BirthDate, Gender,     Email                               , Phone      , Password)
                SELECT          'SuperAdmin','-',     '-',       '-',      '-',       '-',     '-',             '-',           '-',    '2026/03/17',      '-',     'customer_service@ict_supplier.com', '000000000', 'AdminPass'
                WHERE NOT EXISTS (SELECT 1 FROM Users WHERE Login = 'SuperAdmin');
";
            createAdminCmd.ExecuteNonQuery();


        }
    }
}