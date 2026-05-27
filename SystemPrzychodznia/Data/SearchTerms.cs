using System;
using System.Collections.Generic;

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

    // Nowa klasa do zaawansowanego filtrowania wizyt
    public class SearchTermsWizyta
    {
        public DateTime? DataOd { get; set; }
        public DateTime? DataDo { get; set; }
        public int? IdLekarza { get; set; }
        public int? IdPacjenta { get; set; }
        public string Status { get; set; }
    }
}