namespace SystemPrzychodznia.Data
{
    public class PatientListItem
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PESEL { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string BirthDate { get; set; } = string.Empty;
    }

    public class PatientFull
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PESEL { get; set; } = string.Empty;
        public string BirthDate { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Locality { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string PropertyNumber { get; set; } = string.Empty;
        public string HouseUnitNumber { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
