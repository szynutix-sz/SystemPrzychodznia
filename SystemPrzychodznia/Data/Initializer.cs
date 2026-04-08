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

            // Tłumaczymy bazę prowadzącego/kumpla na angielski, zachowując jej strukturę relacyjną!
            createTableCmd.CommandText = @"
PRAGMA foreign_keys = ON;

-- ============================================================
-- 1. Tabela Addresses (Projekt kumpla: Adres)
-- ============================================================
CREATE TABLE IF NOT EXISTS Addresses (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Locality TEXT NOT NULL,
    PostalCode TEXT NOT NULL,
    Street TEXT NOT NULL,
    PropertyNumber TEXT NOT NULL,
    HouseUnitNumber TEXT
);

-- ============================================================
-- 2. Tabela Users (Projekt kumpla: Uzytkownik)
-- ============================================================
CREATE TABLE IF NOT EXISTS Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    AddressId INTEGER NOT NULL,
    Login TEXT UNIQUE NOT NULL,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL,
    PESEL TEXT UNIQUE NOT NULL,
    BirthDate TEXT NOT NULL,
    Gender TEXT NOT NULL CHECK (Gender IN ('K', 'M', 'Inna')),
    Email TEXT UNIQUE NOT NULL,
    Phone TEXT NOT NULL,
    BlockedUntil TEXT,
    IsForgotten INTEGER NOT NULL DEFAULT 0 CHECK (IsForgotten IN (0,1)),
    ForgottenDate TEXT,
    ForgottenBy INTEGER,
    FOREIGN KEY (AddressId) REFERENCES Addresses(Id) ON DELETE RESTRICT,
    FOREIGN KEY (ForgottenBy) REFERENCES Users(Id) ON DELETE SET NULL
);

-- ============================================================
-- 3. Tabela PasswordHistory (Projekt kumpla: Historia_Hasel)
-- ============================================================
CREATE TABLE IF NOT EXISTS PasswordHistory (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER NOT NULL,
    PasswordHash TEXT NOT NULL,
    CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- ============================================================
-- 4. Tabela Roles (Projekt kumpla: Uprawnienie)
-- ============================================================
CREATE TABLE IF NOT EXISTS Roles (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT UNIQUE NOT NULL
);

-- ============================================================
-- 5. Tabela UserRoles (Projekt kumpla: Uzytkownik_Uprawnienie)
-- ============================================================
CREATE TABLE IF NOT EXISTS UserRoles (
    UserId INTEGER NOT NULL,
    RoleId INTEGER NOT NULL,
    PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY (RoleId) REFERENCES Roles(Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Specializations (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL UNIQUE);
CREATE TABLE IF NOT EXISTS Rooms (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL);
CREATE TABLE IF NOT EXISTS Doctors (UserId INTEGER PRIMARY KEY, FOREIGN KEY (UserId) REFERENCES Users(Id));
CREATE TABLE IF NOT EXISTS DoctorSpecializations (UserId INTEGER NOT NULL, SpecializationId INTEGER NOT NULL, PRIMARY KEY (UserId, SpecializationId), FOREIGN KEY (UserId) REFERENCES Doctors(UserId), FOREIGN KEY (SpecializationId) REFERENCES Specializations(Id));
CREATE TABLE IF NOT EXISTS Patients (Id INTEGER PRIMARY KEY AUTOINCREMENT, FirstName TEXT NOT NULL, LastName TEXT NOT NULL, PESEL TEXT NOT NULL UNIQUE, BirthDate TEXT NOT NULL, Gender TEXT NOT NULL, AddressId INTEGER NOT NULL, Email TEXT UNIQUE, Phone TEXT NOT NULL, FOREIGN KEY (AddressId) REFERENCES Addresses(Id) ON DELETE RESTRICT);
CREATE TABLE IF NOT EXISTS Appointments (Id INTEGER PRIMARY KEY AUTOINCREMENT, PatientId INTEGER NOT NULL, DoctorId INTEGER NOT NULL, RoomId INTEGER NOT NULL, ScheduledDate TEXT NOT NULL, Status TEXT NOT NULL DEFAULT 'Registered', Symptoms TEXT, Recommendations TEXT, FOREIGN KEY (PatientId) REFERENCES Patients(Id), FOREIGN KEY (DoctorId) REFERENCES Doctors(UserId), FOREIGN KEY (RoomId) REFERENCES Rooms(Id));
";
            createTableCmd.ExecuteNonQuery();

            // ============================================================
            // WSTAWIANIE DANYCH DOMYŚLNYCH I KONT TESTOWYCH
            // ============================================================
            var insertDataCmd = connection.CreateCommand();
            insertDataCmd.CommandText = @"
-- ROLE
INSERT OR IGNORE INTO Roles (Name) VALUES ('SuperAdmin'), ('Administrator'), ('Lekarz'), ('Recepcja'), ('Brak_roli');

-- ==========================================
-- 1. SUPERADMIN (Zmartwychwstanie i inicjalizacja)
-- ==========================================
UPDATE Users SET FirstName = '-', LastName = '-', PESEL = '00000000000', BirthDate = '2026-03-17', Gender = 'Inna', Email = 'customer_service@ict_supplier.com', Phone = '000000000', IsForgotten = 0, ForgottenDate = NULL, ForgottenBy = NULL WHERE Login = 'SuperAdmin' AND IsForgotten = 1;

INSERT INTO Addresses (Id, Locality, PostalCode, Street, PropertyNumber, HouseUnitNumber) SELECT 1, '-', '-', '-', '-', '-' WHERE NOT EXISTS (SELECT 1 FROM Addresses WHERE Id = 1);

INSERT INTO Users (Id, AddressId, Login, FirstName, LastName, PESEL, BirthDate, Gender, Email, Phone, BlockedUntil, IsForgotten)
SELECT 1, 1, 'SuperAdmin', '-', '-', '-', '2026-03-17', 'Inna', 'customer_service@ict_supplier.com', '000000000', NULL, 0
WHERE NOT EXISTS (SELECT 1 FROM Users WHERE Login = 'SuperAdmin');

INSERT INTO PasswordHistory (UserId, PasswordHash) SELECT 1, 'AdminPass' WHERE NOT EXISTS (SELECT 1 FROM PasswordHistory WHERE UserId = 1);

-- DODANIE RÓL DLA SUPERADMINA (SuperAdmin oraz Administrator)
INSERT OR IGNORE INTO UserRoles (UserId, RoleId) SELECT 1, (SELECT Id FROM Roles WHERE Name = 'SuperAdmin');
INSERT OR IGNORE INTO UserRoles (UserId, RoleId) SELECT 1, (SELECT Id FROM Roles WHERE Name = 'Administrator');

-- ==========================================
-- 2. KONTO TESTOWE: RECEPCJONISTA
-- ==========================================
INSERT INTO Addresses (Id, Locality, PostalCode, Street, PropertyNumber, HouseUnitNumber) SELECT 2, 'Warszawa', '00-001', 'Kwiatowa', '1', '2' WHERE NOT EXISTS (SELECT 1 FROM Addresses WHERE Id = 2);

INSERT INTO Users (Id, AddressId, Login, FirstName, LastName, PESEL, BirthDate, Gender, Email, Phone, BlockedUntil, IsForgotten)
SELECT 2, 2, 'Recepcja1', 'Anna', 'Nowak', '11111111111', '1990-05-20', 'K', 'recepcja@przychodnia.pl', '111222333', NULL, 0
WHERE NOT EXISTS (SELECT 1 FROM Users WHERE Login = 'Recepcja1');

INSERT INTO PasswordHistory (UserId, PasswordHash) SELECT 2, 'RecPass123' WHERE NOT EXISTS (SELECT 1 FROM PasswordHistory WHERE UserId = 2);

INSERT OR IGNORE INTO UserRoles (UserId, RoleId) SELECT 2, (SELECT Id FROM Roles WHERE Name = 'Recepcja');

-- ==========================================
-- 3. KONTO TESTOWE: LEKARZ
-- ==========================================
INSERT INTO Addresses (Id, Locality, PostalCode, Street, PropertyNumber, HouseUnitNumber) SELECT 3, 'Kraków', '30-002', 'Lekarska', '10', '' WHERE NOT EXISTS (SELECT 1 FROM Addresses WHERE Id = 3);

INSERT INTO Users (Id, AddressId, Login, FirstName, LastName, PESEL, BirthDate, Gender, Email, Phone, BlockedUntil, IsForgotten)
SELECT 3, 3, 'Lekarz1', 'Jan', 'Kowalski', '22222222222', '1985-10-10', 'M', 'lekarz@przychodnia.pl', '444555666', NULL, 0
WHERE NOT EXISTS (SELECT 1 FROM Users WHERE Login = 'Lekarz1');

INSERT INTO PasswordHistory (UserId, PasswordHash) SELECT 3, 'LekPass123' WHERE NOT EXISTS (SELECT 1 FROM PasswordHistory WHERE UserId = 3);

INSERT OR IGNORE INTO UserRoles (UserId, RoleId) SELECT 3, (SELECT Id FROM Roles WHERE Name = 'Lekarz');

INSERT OR IGNORE INTO Doctors (UserId) SELECT 3;
";
            insertDataCmd.ExecuteNonQuery();
        }
    }
}