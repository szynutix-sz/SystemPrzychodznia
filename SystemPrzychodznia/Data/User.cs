using System.CodeDom;

namespace SystemPrzychodznia.Data
{
    public class BaseUser
    {
        public int Id { get; set; } // Unikalny identyfikator użytkownika (autogenerowany)
        public string Login { get; set; } = string.Empty; // Unkialny login użytkownika
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
    }

    public class ForgottenUser : BaseUser
    {
        public string DateForgotten { get; set; } = string.Empty ;

        public int ForgottenBy { get; set; } = 0;
    }

    public class User : BaseUser
    {

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

        public static bool operator ==(UserFull lhs, UserFull rhs)
        {
            bool value = lhs.Login == rhs.Login && lhs.FirstName == rhs.FirstName;

            value = value && lhs.LastName == rhs.LastName && lhs.Locality == rhs.Locality;
            value = value && lhs.PostalCode == rhs.PostalCode && lhs.Street == rhs.Street;
            value = value && lhs.PropertyNumber == rhs.PropertyNumber && lhs.HouseUnitNumber == rhs.HouseUnitNumber;
            value = value && lhs.PESEL == rhs.PESEL && lhs.BirthDate == rhs.BirthDate;
            value = value && lhs.Gender == rhs.Gender;
            value = value && lhs.Email == rhs.Email && lhs.Phone == rhs.Phone;


            foreach(Uprawnienie ulhs in lhs.Uprawnienia)
            {
                Uprawnienie urhs = rhs.Uprawnienia.Find(u => u.Id == ulhs.Id);
                if (urhs == null)
                {
                    value = false;
                    break;
                }
                value = value && (ulhs.Posiadane == urhs.Posiadane);
            }

            return value;

        }

        public static bool operator !=(UserFull lhs, UserFull rhs)
        {
            return !(lhs == rhs);
        }

    }
}