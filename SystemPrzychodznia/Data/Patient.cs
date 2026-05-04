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
}
