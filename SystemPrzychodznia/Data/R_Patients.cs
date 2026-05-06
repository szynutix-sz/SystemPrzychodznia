using Microsoft.Data.Sqlite;
using System.Text;

namespace SystemPrzychodznia.Data
{
    internal partial class UserRepository
    {
        public List<PatientListItem> GetListPatients(SearchTermsPatient searchTerms)
        {
            var patients = new List<PatientListItem>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
SELECT
    p.ID_Pacjenta,
    p.Imie,
    p.Nazwisko,
    p.PESEL,
    a.Miejscowosc,
    a.Kod_pocztowy,
    a.Ulica,
    a.Numer_posesji_domu,
    a.Numer_lokalu_mieszkania,
    p.Numer_telefonu,
    p.Adres_email,
    p.Data_urodzenia
FROM Pacjent p
LEFT JOIN Adres a ON p.ID_Adresu = a.ID_Adresu
WHERE p.Imie LIKE '%' || $firstName || '%'
  AND p.Nazwisko LIKE '%' || $lastName || '%'
  AND p.PESEL LIKE '%' || $pesel || '%'
  AND (
      TRIM(
          COALESCE(a.Miejscowosc, '') || ' ' ||
          COALESCE(a.Ulica, '') || ' ' ||
          COALESCE(a.Numer_posesji_domu, '') || ' ' ||
          COALESCE(a.Numer_lokalu_mieszkania, '') || ' ' ||
          COALESCE(a.Kod_pocztowy, '')
      )
  ) LIKE '%' || $address || '%'
  AND p.Numer_telefonu LIKE '%' || $phone || '%'
ORDER BY p.Nazwisko, p.Imie;";

            command.Parameters.AddWithValue("$firstName", searchTerms.FirstName.Trim());
            command.Parameters.AddWithValue("$lastName", searchTerms.LastName.Trim());
            command.Parameters.AddWithValue("$pesel", searchTerms.PESEL.Trim());
            command.Parameters.AddWithValue("$address", searchTerms.Address.Trim());
            command.Parameters.AddWithValue("$phone", searchTerms.Phone.Trim());

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                patients.Add(new PatientListItem
                {
                    Id = reader.GetInt32(0),
                    FullName = $"{reader.GetString(1)} {reader.GetString(2)}".Trim(),
                    PESEL = reader.GetString(3),
                    Address = ComposeAddress(
                        ReadNullablePatientString(reader, 4),
                        ReadNullablePatientString(reader, 5),
                        ReadNullablePatientString(reader, 6),
                        ReadNullablePatientString(reader, 7),
                        ReadNullablePatientString(reader, 8)),
                    Phone = ReadNullablePatientString(reader, 9),
                    Email = ReadNullablePatientString(reader, 10),
                    BirthDate = ReadNullablePatientString(reader, 11)
                });
            }

            return patients;
        }

        public PatientFull? GetPatientFull(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
SELECT
    p.ID_Pacjenta,
    p.Imie,
    p.Nazwisko,
    p.PESEL,
    p.Data_urodzenia,
    p.Plec,
    p.Adres_email,
    p.Numer_telefonu,
    a.Miejscowosc,
    a.Kod_pocztowy,
    a.Ulica,
    a.Numer_posesji_domu,
    a.Numer_lokalu_mieszkania
FROM Pacjent p
LEFT JOIN Adres a ON p.ID_Adresu = a.ID_Adresu
WHERE p.ID_Pacjenta = $id;";
            command.Parameters.AddWithValue("$id", id);

            using var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            return new PatientFull
            {
                Id = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                PESEL = reader.GetString(3),
                BirthDate = reader.GetString(4),
                Gender = reader.GetString(5),
                Email = ReadNullablePatientString(reader, 6),
                Phone = ReadNullablePatientString(reader, 7),
                Locality = ReadNullablePatientString(reader, 8),
                PostalCode = ReadNullablePatientString(reader, 9),
                Street = ReadNullablePatientString(reader, 10),
                PropertyNumber = ReadNullablePatientString(reader, 11),
                HouseUnitNumber = ReadNullablePatientString(reader, 12)
            };
        }

        public void AddPatient(PatientFull patient)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            var addressCommand = connection.CreateCommand();
            addressCommand.Transaction = transaction;
            addressCommand.CommandText = @"
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
);";

            addressCommand.Parameters.AddWithValue("$locality", patient.Locality);
            addressCommand.Parameters.AddWithValue("$postalCode", patient.PostalCode);
            addressCommand.Parameters.AddWithValue("$street", string.IsNullOrWhiteSpace(patient.Street) ? DBNull.Value : patient.Street);
            addressCommand.Parameters.AddWithValue("$propertyNumber", patient.PropertyNumber);
            addressCommand.Parameters.AddWithValue("$houseUnitNumber", string.IsNullOrWhiteSpace(patient.HouseUnitNumber) ? DBNull.Value : patient.HouseUnitNumber);
            addressCommand.ExecuteNonQuery();

            long addressId = ReadLastInsertRowId(connection, transaction);

            var patientCommand = connection.CreateCommand();
            patientCommand.Transaction = transaction;
            patientCommand.CommandText = @"
INSERT INTO Pacjent (
    ID_Adresu,
    Imie,
    Nazwisko,
    PESEL,
    Adres_email,
    Numer_telefonu,
    Data_urodzenia,
    Plec
)
VALUES (
    $addressId,
    $firstName,
    $lastName,
    $pesel,
    $email,
    $phone,
    $birthDate,
    $gender
);";

            patientCommand.Parameters.AddWithValue("$addressId", addressId);
            patientCommand.Parameters.AddWithValue("$firstName", patient.FirstName);
            patientCommand.Parameters.AddWithValue("$lastName", patient.LastName);
            patientCommand.Parameters.AddWithValue("$pesel", patient.PESEL);
            patientCommand.Parameters.AddWithValue("$email", string.IsNullOrWhiteSpace(patient.Email) ? DBNull.Value : patient.Email);
            patientCommand.Parameters.AddWithValue("$phone", patient.Phone);
            patientCommand.Parameters.AddWithValue("$birthDate", patient.BirthDate);
            patientCommand.Parameters.AddWithValue("$gender", patient.Gender);
            patientCommand.ExecuteNonQuery();

            patient.Id = (int)ReadLastInsertRowId(connection, transaction);
            transaction.Commit();
        }

        public void EditPatient(PatientFull patient)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            var addressCommand = connection.CreateCommand();
            addressCommand.Transaction = transaction;
            addressCommand.CommandText = @"
UPDATE Adres
SET
    Miejscowosc = $locality,
    Kod_pocztowy = $postalCode,
    Ulica = $street,
    Numer_posesji_domu = $propertyNumber,
    Numer_lokalu_mieszkania = $houseUnitNumber
WHERE ID_Adresu = (
    SELECT ID_Adresu
    FROM Pacjent
    WHERE ID_Pacjenta = $id
);";

            addressCommand.Parameters.AddWithValue("$id", patient.Id);
            addressCommand.Parameters.AddWithValue("$locality", patient.Locality);
            addressCommand.Parameters.AddWithValue("$postalCode", patient.PostalCode);
            addressCommand.Parameters.AddWithValue("$street", string.IsNullOrWhiteSpace(patient.Street) ? DBNull.Value : patient.Street);
            addressCommand.Parameters.AddWithValue("$propertyNumber", patient.PropertyNumber);
            addressCommand.Parameters.AddWithValue("$houseUnitNumber", string.IsNullOrWhiteSpace(patient.HouseUnitNumber) ? DBNull.Value : patient.HouseUnitNumber);
            addressCommand.ExecuteNonQuery();

            var patientCommand = connection.CreateCommand();
            patientCommand.Transaction = transaction;
            patientCommand.CommandText = @"
UPDATE Pacjent
SET
    Imie = $firstName,
    Nazwisko = $lastName,
    PESEL = $pesel,
    Adres_email = $email,
    Numer_telefonu = $phone,
    Data_urodzenia = $birthDate,
    Plec = $gender
WHERE ID_Pacjenta = $id;";

            patientCommand.Parameters.AddWithValue("$id", patient.Id);
            patientCommand.Parameters.AddWithValue("$firstName", patient.FirstName);
            patientCommand.Parameters.AddWithValue("$lastName", patient.LastName);
            patientCommand.Parameters.AddWithValue("$pesel", patient.PESEL);
            patientCommand.Parameters.AddWithValue("$email", string.IsNullOrWhiteSpace(patient.Email) ? DBNull.Value : patient.Email);
            patientCommand.Parameters.AddWithValue("$phone", patient.Phone);
            patientCommand.Parameters.AddWithValue("$birthDate", patient.BirthDate);
            patientCommand.Parameters.AddWithValue("$gender", patient.Gender);
            patientCommand.ExecuteNonQuery();

            transaction.Commit();
        }

        public bool PatientPeselExists(string pesel, bool exclude = false, int excludeId = -1)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            if (exclude)
            {
                command.CommandText = "SELECT COUNT(*) FROM Pacjent WHERE PESEL = $pesel AND ID_Pacjenta != $excludeId;";
                command.Parameters.AddWithValue("$excludeId", excludeId);
            }
            else
            {
                command.CommandText = "SELECT COUNT(*) FROM Pacjent WHERE PESEL = $pesel;";
            }

            command.Parameters.AddWithValue("$pesel", pesel);
            long count = (long)command.ExecuteScalar()!;
            return count > 0;
        }

        public bool PatientEmailExists(string email, bool exclude = false, int excludeId = -1)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            if (exclude)
            {
                command.CommandText = "SELECT COUNT(*) FROM Pacjent WHERE Adres_email = $email AND ID_Pacjenta != $excludeId;";
                command.Parameters.AddWithValue("$excludeId", excludeId);
            }
            else
            {
                command.CommandText = "SELECT COUNT(*) FROM Pacjent WHERE Adres_email = $email;";
            }

            command.Parameters.AddWithValue("$email", email);
            long count = (long)command.ExecuteScalar()!;
            return count > 0;
        }

        private static string ReadNullablePatientString(SqliteDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);
        }

        private static long ReadLastInsertRowId(SqliteConnection connection, SqliteTransaction transaction)
        {
            var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = "SELECT last_insert_rowid();";
            return (long)command.ExecuteScalar()!;
        }

        private static string ComposeAddress(string locality, string postalCode, string street, string propertyNumber, string houseUnitNumber)
        {
            var builder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(locality))
            {
                builder.Append(locality.Trim());
            }

            if (!string.IsNullOrWhiteSpace(postalCode))
            {
                if (builder.Length > 0)
                {
                    builder.Append(", ");
                }

                builder.Append(postalCode.Trim());
            }

            if (!string.IsNullOrWhiteSpace(street))
            {
                if (builder.Length > 0)
                {
                    builder.Append(", ");
                }

                builder.Append(street.Trim());
            }

            if (!string.IsNullOrWhiteSpace(propertyNumber))
            {
                if (!string.IsNullOrWhiteSpace(street))
                {
                    builder.Append(' ');
                }
                else if (builder.Length > 0)
                {
                    builder.Append(", ");
                }

                builder.Append(propertyNumber.Trim());
            }

            if (!string.IsNullOrWhiteSpace(houseUnitNumber))
            {
                builder.Append('/');
                builder.Append(houseUnitNumber.Trim());
            }

            return builder.ToString();
        }
    }
}
