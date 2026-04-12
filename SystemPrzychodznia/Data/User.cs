namespace SystemPrzychodznia.Data
{
    public class User
    {
        public int Id { get; set; } // Unikalny identyfikator użytkownika (autogenerowany)
        public string Login { get; set; } = string.Empty; // Unkialny login użytkownika
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string PESEL { get; set; } = string.Empty; // składnia 11 cyfr

        public string Email { get; set; } = string.Empty; //Unikalny, składnia nazwa_użytkownika@nazwa_domeny_niższa.nazwa_domeny_wyższa” 

       
    }

    public class UserFull : User
    {
        

        public List<Uprawnienie> Uprawnienia { get; set; } = new List<Uprawnienie>(); // Lista uprawnień użytkownika, np. "Admin", "User", "Doctor"
        public string Locality { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty; // składnia 5 cyfr

        public string? Street { get; set; } // Opcjonalnie


        public string PropertyNumber { get; set; } = string.Empty;
        public string HouseUnitNumber { get; set; } = string.Empty; // Opcjonalnie


        public string BirthDate { get; set; } = string.Empty; // składnia YYYY-MM-DD , gdzie Y M i D to cyfry

        public string Gender { get; set; } = string.Empty; // M - mężczyzna, K - kobieta


        public string Phone { get; set; } = string.Empty; // składnia 9 cyfr, np. "123456789"

        public string Password { get; set; } = string.Empty; // W praktyce hasło powinno być przechowywane w formie zaszyfrowanej (hash), ale dla uproszczenia trzymamy je jako tekst
    }
}