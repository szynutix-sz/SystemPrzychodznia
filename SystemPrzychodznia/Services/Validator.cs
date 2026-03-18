using System;
using System.Collections.Generic;
using System.Text;
using SystemPrzychodznia.Data;

namespace SystemPrzychodznia.Services
{
    internal class Validator
    {
        private readonly UserRepository _repository;

        public Validator(UserRepository repository)
        {
            _repository = repository;
        }

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

            // Walidacja unikalności loginu, email, PESEL
            // trzeba chyba osobną walidację zrobić, jak przy edycji loginu/email/PESEL
            // bo przy edycji użytkownik może zostawić te same dane i wtedy nie powinno być błędu o zajętym loginie/emailu/PESELu
            //if (!string.IsNullOrWhiteSpace(user.Login) && _repository.CzyIstniejeLogin(user.Login))
            //    errors.Add("Login jest już zajęty");

            //if (!string.IsNullOrWhiteSpace(user.Email) && _repository.CzyIstniejeEmail(user.Email))
            //    errors.Add("Email jest już zajęty");

            //if (!string.IsNullOrWhiteSpace(user.PESEL) && user.PESEL.Length == 11 && _repository.CzyIstnieje_PESEL(user.PESEL))
            //    errors.Add("PESEL jest już w systemie");

            // Walidacja formatu hasła - minimum 6 znaków
            if (!string.IsNullOrWhiteSpace(user.Password) && user.Password.Length < 6)
    errors.Add("Hasło musi mieć co najmniej 6 znaków");

// Walidacja długości pól - max 255 znaków
            if (!string.IsNullOrWhiteSpace(user.Login) && user.Login.Length > 255)
                errors.Add("Login nie może być dłuższy niż 255 znaków");

            if (!string.IsNullOrWhiteSpace(user.FirstName) && user.FirstName.Length > 255)
                errors.Add("Imię nie może być dłuższe niż 255 znaków");

            if (!string.IsNullOrWhiteSpace(user.LastName) && user.LastName.Length > 255)
                errors.Add("Nazwisko nie może być dłuższe niż 255 znaków");

            if (!string.IsNullOrWhiteSpace(user.Email) && user.Email.Length > 255)
                errors.Add("Email nie może być dłuższy niż 255 znaków");

            // Walidacja czy data urodzenia zgadza się z datą zakodowaną w PESEL
            if (!string.IsNullOrWhiteSpace(user.PESEL) && user.PESEL.Length == 11
                && CzySameLiczby(user.PESEL)
                && !string.IsNullOrWhiteSpace(user.BirthDate)
                && CzyPoprawnaDates(user.BirthDate))
            {
                string dataZPesel = WyciagnijDateZPesel(user.PESEL);
                if (dataZPesel != user.BirthDate)
                    errors.Add("Data urodzenia nie zgadza się z datą zakodowaną w PESEL");
            }

            return new ValidationResult(errors.Count == 0) { Errors = errors };
        }

        // Wyciąga datę urodzenia z PESEL i zwraca w formacie YYYY-MM-DD
        private string WyciagnijDateZPesel(string pesel)
        {
            int rok2    = int.Parse(pesel.Substring(0, 2));
            int miesKod = int.Parse(pesel.Substring(2, 2));
            int dzien   = int.Parse(pesel.Substring(4, 2));

            int rok, miesiac;

            // Kodowanie stulecia w miesiącu PESEL
            if (miesKod >= 81)      { rok = 1800 + rok2; miesiac = miesKod - 80; }
            else if (miesKod >= 61) { rok = 2200 + rok2; miesiac = miesKod - 60; }
            else if (miesKod >= 41) { rok = 2100 + rok2; miesiac = miesKod - 40; }
            else if (miesKod >= 21) { rok = 2000 + rok2; miesiac = miesKod - 20; }
            else                    { rok = 1900 + rok2; miesiac = miesKod; }

            return $"{rok:D4}-{miesiac:D2}-{dzien:D2}";
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
