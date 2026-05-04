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
    u.ID_Uzytkownika,
    u.Imie,
    u.Nazwisko,
    u.PESEL,
    a.Miejscowosc,
    a.Kod_pocztowy,
    a.Ulica,
    a.Numer_posesji_domu,
    a.Numer_lokalu_mieszkania,
    u.Numer_telefonu,
    u.Adres_email,
    u.Data_urodzenia
FROM Uzytkownik u
LEFT JOIN Adres a ON u.ID_Adresu = a.ID_Adresu
WHERE u.Czy_zapomniany = 0
  AND u.ID_Uzytkownika IN (
      SELECT up.ID_Uzytkownika
      FROM Uzytkownik_Uprawnienie up
      WHERE up.ID_Uprawnienia = 5
  )
  AND u.Imie LIKE '%' || $firstName || '%'
  AND u.Nazwisko LIKE '%' || $lastName || '%'
  AND u.PESEL LIKE '%' || $pesel || '%'
  AND (
      TRIM(
          COALESCE(a.Miejscowosc, '') || ' ' ||
          COALESCE(a.Ulica, '') || ' ' ||
          COALESCE(a.Numer_posesji_domu, '') || ' ' ||
          COALESCE(a.Numer_lokalu_mieszkania, '') || ' ' ||
          COALESCE(a.Kod_pocztowy, '')
      )
  ) LIKE '%' || $address || '%'
  AND u.Numer_telefonu LIKE '%' || $phone || '%'
ORDER BY u.Nazwisko, u.Imie;";

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

        private static string ReadNullablePatientString(SqliteDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);
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
