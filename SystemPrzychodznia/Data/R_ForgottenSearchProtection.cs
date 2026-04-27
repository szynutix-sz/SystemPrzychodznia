using System;
using System.Security.Cryptography;
using System.Text;

namespace SystemPrzychodznia.Data
{
    internal partial class UserRepository
    {
        private static string EncryptForgottenSearchValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            byte[] rawBytes = Encoding.UTF8.GetBytes(value.Trim());
            byte[] protectedBytes = ProtectedData.Protect(rawBytes, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(protectedBytes);
        }

        private static string DecryptForgottenSearchValue(string? protectedValue)
        {
            if (string.IsNullOrWhiteSpace(protectedValue))
            {
                return string.Empty;
            }

            try
            {
                byte[] protectedBytes = Convert.FromBase64String(protectedValue);
                byte[] rawBytes = ProtectedData.Unprotect(protectedBytes, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(rawBytes);
            }
            catch (FormatException)
            {
                return string.Empty;
            }
            catch (CryptographicException)
            {
                return string.Empty;
            }
        }

        private static bool ContainsForgottenSearchText(string source, string searchValue)
        {
            if (string.IsNullOrWhiteSpace(searchValue))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(source))
            {
                return false;
            }

            return source.Contains(searchValue.Trim(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
