namespace SystemPrzychodznia.Data
{
    public class SearchTerms
    {
        public string Email = "";
        public string FirstName = "";
        public string LastName = "";
        public string Login = "";
        public string PESEL = "";

        public List<Uprawnienie> Uprawnienia { get; set; } = new List<Uprawnienie>();
    }
}