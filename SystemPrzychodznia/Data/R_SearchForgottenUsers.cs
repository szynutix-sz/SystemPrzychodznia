using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemPrzychodznia.Data
{
    internal partial class UserRepository
    {
        private static string ReadNullableString(SqliteDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);
        }

        public List<ForgottenUser> GetListForgottenUsers(SearchTermsForgotten searchTerms)
        {
            searchTerms ??= new SearchTermsForgotten();

            var users = new List<ForgottenUser>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
SELECT 
    u.Login, u.Imie AS FirstName, u.Nazwisko AS LastName, u.Data_zapomnienia AS DateForgotten, 
    u.ID_Kto_Zapomnial AS ForgottenBy, u.Id_Uzytkownika, COALESCE(admin.Login, 'Nieznany') AS ForgottenByLogin,
    u.Zapomniany_Login_Enc, u.Zapomniane_Imie_Enc, u.Zapomniane_Nazwisko_Enc
FROM Uzytkownik u
LEFT JOIN Uzytkownik admin ON u.ID_Kto_Zapomnial = admin.ID_Uzytkownika
WHERE u.Czy_zapomniany = 1
    AND u.Data_zapomnienia LIKE '%' || $dateForgotten || '%' COLLATE NOCASE";

            if (searchTerms.Id.HasValue)
                command.CommandText += " AND u.Id_Uzytkownika = $id";

            if (searchTerms.ForgottenBy.HasValue)
                command.CommandText += " AND u.ID_Kto_Zapomnial = $forgottenBy";

            command.CommandText += ";";

            command.Parameters.AddWithValue("$dateForgotten", searchTerms.DateForgotten);

            if (searchTerms.Id.HasValue)
                command.Parameters.AddWithValue("$id", searchTerms.Id.Value);

            if (searchTerms.ForgottenBy.HasValue)
                command.Parameters.AddWithValue("$forgottenBy", searchTerms.ForgottenBy.Value);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var user = new ForgottenUser
                {
                    Login = ReadNullableString(reader, 0),
                    FirstName = ReadNullableString(reader, 1),
                    LastName = ReadNullableString(reader, 2),
                    DateForgotten = ReadNullableString(reader, 3),
                    ForgottenBy = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                    Id = reader.GetInt32(5),
                    ForgottenByLogin = ReadNullableString(reader, 6)
                };

                string originalLogin = DecryptForgottenSearchValue(ReadNullableString(reader, 7));
                string originalFirstName = DecryptForgottenSearchValue(ReadNullableString(reader, 8));
                string originalLastName = DecryptForgottenSearchValue(ReadNullableString(reader, 9));

                string loginSearchSource = string.IsNullOrWhiteSpace(originalLogin) ? user.Login : originalLogin;
                string firstNameSearchSource = string.IsNullOrWhiteSpace(originalFirstName) ? user.FirstName : originalFirstName;
                string lastNameSearchSource = string.IsNullOrWhiteSpace(originalLastName) ? user.LastName : originalLastName;
                string fullNameSearchSource = $"{firstNameSearchSource} {lastNameSearchSource}".Trim();

                if (!ContainsForgottenSearchText(loginSearchSource, searchTerms.Login))
                    continue;

                // Główne filtrowanie zgodne z UC: po "imieniu i nazwisku po zapomnieniu".
                if (!ContainsForgottenSearchText(fullNameSearchSource, searchTerms.FullName))
                    continue;

                if (!ContainsForgottenSearchText(firstNameSearchSource, searchTerms.FirstName))
                    continue;

                if (!ContainsForgottenSearchText(lastNameSearchSource, searchTerms.LastName))
                    continue;

                users.Add(user);
            }
            return users;
        }
    }
}
