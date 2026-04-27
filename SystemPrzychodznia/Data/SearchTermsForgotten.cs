namespace SystemPrzychodznia.Data
{
    public class SearchTermsForgotten
    {
        public string Login { get; set; } = "";
        public int? Id { get; set; } = null;
        public string FullName { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string DateForgotten { get; set; } = "";
        public int? ForgottenBy { get; set; } = null;
    }
}
