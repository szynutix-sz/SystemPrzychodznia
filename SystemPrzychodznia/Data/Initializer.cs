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
            var createTablesCmd = connection.CreateCommand();
            createTablesCmd.CommandText = @"
                -- Słowniki
                CREATE TABLE IF NOT EXISTS Roles (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL UNIQUE
                );

                CREATE TABLE IF NOT EXISTS Specializations (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL UNIQUE
                );

                CREATE TABLE IF NOT EXISTS Rooms (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL
                );

                -- Użytkownicy
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
                    Status TEXT NOT NULL DEFAULT 'A',
                    BlockedUntil TEXT, 
                    ForgottenDate TEXT,
                    ForgottenBy INTEGER,
                    FOREIGN KEY (ForgottenBy) REFERENCES Users(Id)
                );

                CREATE TABLE IF NOT EXISTS UserRoles (
                    UserId INTEGER NOT NULL,
                    RoleId INTEGER NOT NULL,
                    PRIMARY KEY (UserId, RoleId),
                    FOREIGN KEY (UserId) REFERENCES Users(Id),
                    FOREIGN KEY (RoleId) REFERENCES Roles(Id)
                );

                CREATE TABLE IF NOT EXISTS PasswordHistory (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER NOT NULL,
                    PasswordHash TEXT NOT NULL,
                    CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (UserId) REFERENCES Users(Id)
                );

                -- Pacjenci
                CREATE TABLE IF NOT EXISTS Patients (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FirstName TEXT NOT NULL,
                    LastName TEXT NOT NULL,
                    PESEL TEXT NOT NULL UNIQUE,
                    BirthDate TEXT NOT NULL,
                    Gender TEXT NOT NULL,
                    Locality TEXT NOT NULL,
                    PostalCode TEXT NOT NULL,
                    Street TEXT,
                    PropertyNumber TEXT NOT NULL,
                    HouseUnitNumber TEXT,
                    Email TEXT UNIQUE, 
                    Phone TEXT NOT NULL
                );

                -- Lekarze i Wizyty
                CREATE TABLE IF NOT EXISTS Doctors (
                    UserId INTEGER PRIMARY KEY,
                    FOREIGN KEY (UserId) REFERENCES Users(Id)
                );

                CREATE TABLE IF NOT EXISTS DoctorSpecializations (
                    UserId INTEGER NOT NULL,
                    SpecializationId INTEGER NOT NULL,
                    PRIMARY KEY (UserId, SpecializationId),
                    FOREIGN KEY (UserId) REFERENCES Doctors(UserId),
                    FOREIGN KEY (SpecializationId) REFERENCES Specializations(Id)
                );

                CREATE TABLE IF NOT EXISTS Appointments (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    PatientId INTEGER NOT NULL,
                    DoctorId INTEGER NOT NULL,
                    RoomId INTEGER NOT NULL,
                    ScheduledDate TEXT NOT NULL,
                    Status TEXT NOT NULL DEFAULT 'Registered',
                    Symptoms TEXT,
                    Recommendations TEXT,
                    FOREIGN KEY (PatientId) REFERENCES Patients(Id),
                    FOREIGN KEY (DoctorId) REFERENCES Doctors(UserId),
                    FOREIGN KEY (RoomId) REFERENCES Rooms(Id)
                );
            ";
            createTablesCmd.ExecuteNonQuery();

            // 2. INICJALIZACJA DANYCH DOMYŚLNYCH
            var insertInitialDataCmd = connection.CreateCommand();
            insertInitialDataCmd.CommandText = @"
                -- Wstawienie domyślnych ról, jeśli nie istnieją
                INSERT OR IGNORE INTO Roles (Name) VALUES ('Administrator');
                INSERT OR IGNORE INTO Roles (Name) VALUES ('Recepcjonista');
                INSERT OR IGNORE INTO Roles (Name) VALUES ('Lekarz');
                INSERT OR IGNORE INTO Roles (Name) VALUES ('Pacjent');

                -- Wstawienie konta SuperAdmin, jeśli nie istnieje
                INSERT INTO Users (Login, FirstName, LastName, Locality, PostalCode, Street, PropertyNumber, HouseUnitNumber, PESEL, BirthDate, Gender, Email, Phone, Password)
                SELECT 'SuperAdmin', '-', '-', '-', '-', '-', '-', '-', '00000000000', '2026/03/17', '-', 'admin@przychodnia.pl', '000000000', 'AdminPass'
                WHERE NOT EXISTS (SELECT 1 FROM Users WHERE Login = 'SuperAdmin');

                -- Przypisanie roli 'Administrator' (Id=1) do konta 'SuperAdmin' (Id=1)
                INSERT OR IGNORE INTO UserRoles (UserId, RoleId) 
                SELECT 
                    (SELECT Id FROM Users WHERE Login = 'SuperAdmin'),
                    (SELECT Id FROM Roles WHERE Name = 'Administrator')
                WHERE EXISTS (SELECT 1 FROM Users WHERE Login = 'SuperAdmin');
            ";
            insertInitialDataCmd.ExecuteNonQuery();
        }
    }
}