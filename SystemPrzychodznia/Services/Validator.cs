using System;
using System.Collections.Generic;
using System.Text;
using SystemPrzychodznia.Data;

namespace SystemPrzychodznia.Services
{
    internal class Validator
    {

        public ValidationResult ValidateUserFull(UserFull user)
        {
            var errors = new List<string>();

            // Walidacja Loginu
            if (string.IsNullOrWhiteSpace(user.Login))
                errors.Add("Login jest wymagany");

            // Walidacja Imienia
            if (string.IsNullOrWhiteSpace(user.FirstName))
                errors.Add("Imię jest wymagane");

            // Walidacja Nazwiska
            if (string.IsNullOrWhiteSpace(user.LastName))
                errors.Add("Nazwisko jest wymagane");

            // Walidacja Email
            if (string.IsNullOrWhiteSpace(user.Email))
                errors.Add("Email jest wymagany");
            else if (!user.Email.Contains("@") || !user.Email.Contains("."))
                errors.Add("Email musi zawierać @ i .");

            // Walidacja PESEL - musi mieć 11 znaków i być liczbą
            if (string.IsNullOrWhiteSpace(user.PESEL))
                errors.Add("PESEL jest wymagany");
            else if (user.PESEL.Length != 11)
                errors.Add("PESEL musi mieć 11 cyfr");
            else if (!CzySameLiczby(user.PESEL))
                errors.Add("PESEL musi zawierać tylko cyfry");

            // Walidacja Miejscowości
            if (string.IsNullOrWhiteSpace(user.Locality))
                errors.Add("Miejscowość jest wymagana");

            // Walidacja Kodu Pocztowego - musi mieć 5 znaków i być liczbą
            if (string.IsNullOrWhiteSpace(user.PostalCode))
                errors.Add("Kod pocztowy jest wymagany");
            else if (user.PostalCode.Length != 5)
                errors.Add("Kod pocztowy musi mieć 5 cyfr");
            else if (!CzySameLiczby(user.PostalCode))
                errors.Add("Kod pocztowy musi zawierać tylko cyfry");

            // Walidacja Numeru Nieruchomości
            if (string.IsNullOrWhiteSpace(user.PropertyNumber))
                errors.Add("Numer nieruchomości jest wymagany");

            // Walidacja Daty Urodzenia
            if (string.IsNullOrWhiteSpace(user.BirthDate))
                errors.Add("Data urodzenia jest wymagana");
            else if (!CzyPoprawnaDates(user.BirthDate))
                errors.Add("Data urodzenia musi być w formacie YYYY-MM-DD");

            // Walidacja Płci - tylko M lub K
            if (string.IsNullOrWhiteSpace(user.Gender))
                errors.Add("Płeć jest wymagana");
            else if (user.Gender != "M" && user.Gender != "K")
                errors.Add("Płeć musi być 'M' lub 'K'");

            // Walidacja Numeru Telefonu - musi mieć 9 znaków i być liczbą
            if (string.IsNullOrWhiteSpace(user.Phone))
                errors.Add("Numer telefonu jest wymagany");
            else if (user.Phone.Length != 9)
                errors.Add("Numer telefonu musi mieć 9 cyfr");
            else if (!CzySameLiczby(user.Phone))
                errors.Add("Numer telefonu musi zawierać tylko cyfry");

            // Walidacja Hasła
            if (string.IsNullOrWhiteSpace(user.Password))
                errors.Add("Hasło jest wymagane");

            // Do zrobienia : walidacje unikalności loginu, email, PESEL 
            // Do zrobienia : walidacja formatu hasła
            // Do zrobienia : walidacja długości pól (np. max 255 znaków)
            // Do zrobienia : walidacja czy data urodzenia i data która wynika z PESEL są zgodne

            return new ValidationResult(errors.Count == 0) { Errors = errors };
        }

        // Sprawdza czy string zawiera tylko cyfry
        private bool CzySameLiczby(string tekst)
        {
            for (int i = 0; i < tekst.Length; i++)
            {
                if (tekst[i] < '0' || tekst[i] > '9')
                    return false;
            }
            return true;
        }

        // Sprawdza czy data ma prawidłowy format YYYY-MM-DD
        private bool CzyPoprawnaDates(string data)
        {
            if (data.Length != 10)
                return false;

            if (data[4] != '-' || data[7] != '-')
                return false;

            // Sprawdzenie czy rok, miesiąc i dzień to liczby
            string rok = data.Substring(0, 4);
            string miesiac = data.Substring(5, 2);
            string dzien = data.Substring(8, 2);

            if (!CzySameLiczby(rok) || !CzySameLiczby(miesiac) || !CzySameLiczby(dzien))
                return false;

            // Sprawdzenie czy liczby są w sensownych zakresach
            try
            {
                int rokNum = int.Parse(rok);
                int miesiacNum = int.Parse(miesiac);
                int dzienNum = int.Parse(dzien);

                if (miesiacNum < 1 || miesiacNum > 12)
                    return false;

                if (dzienNum < 1 || dzienNum > 31)
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
