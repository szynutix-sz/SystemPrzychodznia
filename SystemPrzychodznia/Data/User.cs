namespace PrzychodniaApp.Data
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; } // Opcjonalnie
        public string Role { get; set; } = string.Empty; // np. "Admin", "Doctor", "Receptionist"
    }
}