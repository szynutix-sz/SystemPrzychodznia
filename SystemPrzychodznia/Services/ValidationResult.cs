using System;
using System.Collections.Generic;
using System.Text;
//zmiana
namespace SystemPrzychodznia.Services
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public ValidationResult(bool isValid)
        {
            IsValid = isValid;
        }
    }

}
