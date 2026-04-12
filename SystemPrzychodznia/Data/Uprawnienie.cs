using System;
using System.Collections.Generic;
using System.Text;

namespace SystemPrzychodznia.Data
{
    public class Uprawnienie
    {
        public int Id { get; set; } = 0;
        public string Nazwa { get; set; } = string.Empty;

        public bool? Posiadane { get; set; } = false;
    }
}
