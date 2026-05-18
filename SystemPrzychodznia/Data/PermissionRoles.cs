using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemPrzychodznia.Data
{
    internal static class PermissionRoles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string Lekarz = "Lekarz";
        public const string Recepcja = "Recepcja";
        public const string BrakRoli = "Brak_roli";
    }

    internal static class PermissionRoleExtensions
    {
        public static bool HasRole(this IEnumerable<Uprawnienie>? permissions, string roleName)
        {
            if (permissions == null)
            {
                return false;
            }

            return permissions.Any(permission =>
                permission.Posiadane == true &&
                string.Equals(permission.Nazwa, roleName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
