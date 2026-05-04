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

        public ValidationResult ValidateUserFull(UserFull user, bool editing, bool validatePassword = true)
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


            if (validatePassword)
            {
                errors.AddRange(CzyPoprawneHaslo(user.Id, user.Password));
            }
            // sprawdzenie czy hasło jest takie samo jak potwierdzenie hasła odbywa się w kontrolerze, więc nie trzeba tego sprawdzać tutaj


            // Walidacja unikalności loginu, email, PESEL
            if (!string.IsNullOrWhiteSpace(user.Login) &&
                _repository.CzyIstniejeDanyUżytkowik(user.Login,UserRepository.VAL_LOGIN, editing, user.Id))
                errors.Add("Podany Login jest już zajęty");

            if (!string.IsNullOrWhiteSpace(user.Email) && 
                _repository.CzyIstniejeDanyUżytkowik(user.Email, UserRepository.VAL_EMAIL, editing, user.Id))
                errors.Add("Podany Email jest już zajęty");

            if (!string.IsNullOrWhiteSpace(user.PESEL) && user.PESEL.Length == 11 &&
                _repository.CzyIstniejeDanyUżytkowik(user.PESEL, UserRepository.VAL_PESEL, editing, user.Id))
                errors.Add("Podany PESEL jest już w systemie");



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

            // Walidacja płeć zgadza się z płcią zakodowaną w PESEL
            if (!string.IsNullOrWhiteSpace(user.Gender)
                && (user.Gender == "M" || user.Gender == "K")
                && !string.IsNullOrWhiteSpace(user.PESEL)
                && CzySameLiczby(user.PESEL))
            {
                if (!CzyPlecZgadzaSieZPesel(user.Gender, user.PESEL))
                {
                    errors.Add("Płeć nie zgadza się z płcią zakodowaną w PESEL");
                }
            }

             
            return new ValidationResult(errors.Count == 0) { Errors = errors };
        }

        public ValidationResult ValidatePatientFull(UserFull patient, bool editing)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(patient.Login))
                errors.Add("Nie udało się przygotować identyfikatora technicznego pacjenta");

            if (string.IsNullOrWhiteSpace(patient.FirstName))
                errors.Add("Imię jest wymagane");

            if (string.IsNullOrWhiteSpace(patient.LastName))
                errors.Add("Nazwisko jest wymagane");

            if (string.IsNullOrWhiteSpace(patient.PESEL))
                errors.Add("PESEL jest wymagany");
            else if (patient.PESEL.Length != 11)
                errors.Add("PESEL musi mieć 11 cyfr");
            else if (!CzySameLiczby(patient.PESEL))
                errors.Add("PESEL musi zawierać tylko cyfry");

            if (string.IsNullOrWhiteSpace(patient.Locality))
                errors.Add("Miejscowość jest wymagana");

            if (string.IsNullOrWhiteSpace(patient.PostalCode))
                errors.Add("Kod pocztowy jest wymagany");
            else if (patient.PostalCode.Length != 5)
                errors.Add("Kod pocztowy musi mieć 5 cyfr");
            else if (!CzySameLiczby(patient.PostalCode))
                errors.Add("Kod pocztowy musi zawierać tylko cyfry");

            if (string.IsNullOrWhiteSpace(patient.PropertyNumber))
                errors.Add("Numer nieruchomości jest wymagany");

            if (string.IsNullOrWhiteSpace(patient.BirthDate))
                errors.Add("Data urodzenia jest wymagana");
            else if (!CzyPoprawnaDates(patient.BirthDate))
                errors.Add("Data urodzenia musi być w formacie YYYY-MM-DD");

            if (string.IsNullOrWhiteSpace(patient.Gender))
                errors.Add("Płeć jest wymagana");
            else if (patient.Gender != "M" && patient.Gender != "K")
                errors.Add("Płeć musi być 'M' lub 'K'");

            if (string.IsNullOrWhiteSpace(patient.Phone))
                errors.Add("Numer telefonu jest wymagany");
            else if (patient.Phone.Length != 9)
                errors.Add("Numer telefonu musi mieć 9 cyfr");
            else if (!CzySameLiczby(patient.Phone))
                errors.Add("Numer telefonu musi zawierać tylko cyfry");

            if (!string.IsNullOrWhiteSpace(patient.Email))
            {
                if (!patient.Email.Contains("@") || !patient.Email.Contains("."))
                {
                    errors.Add("Email musi zawierać @ i .");
                }
            }

            if (!string.IsNullOrWhiteSpace(patient.Login) &&
                _repository.CzyIstniejeDanyUżytkowik(patient.Login, UserRepository.VAL_LOGIN, editing, patient.Id))
            {
                errors.Add("Podany pacjent już istnieje w systemie");
            }

            if (!string.IsNullOrWhiteSpace(patient.Email) &&
                _repository.CzyIstniejeDanyUżytkowik(patient.Email, UserRepository.VAL_EMAIL, editing, patient.Id))
            {
                errors.Add("Podany Email jest już zajęty");
            }

            if (!string.IsNullOrWhiteSpace(patient.PESEL) && patient.PESEL.Length == 11 &&
                _repository.CzyIstniejeDanyUżytkowik(patient.PESEL, UserRepository.VAL_PESEL, editing, patient.Id))
            {
                errors.Add("Podany PESEL jest już w systemie");
            }

            if (!string.IsNullOrWhiteSpace(patient.FirstName) && patient.FirstName.Length > 255)
                errors.Add("Imię nie może być dłuższe niż 255 znaków");

            if (!string.IsNullOrWhiteSpace(patient.LastName) && patient.LastName.Length > 255)
                errors.Add("Nazwisko nie może być dłuższe niż 255 znaków");

            if (!string.IsNullOrWhiteSpace(patient.Email) && patient.Email.Length > 255)
                errors.Add("Email nie może być dłuższy niż 255 znaków");

            if (!string.IsNullOrWhiteSpace(patient.Locality) && patient.Locality.Length > 255)
                errors.Add("Miejscowość nie może być dłuższa niż 255 znaków");

            if (!string.IsNullOrWhiteSpace(patient.Street) && patient.Street.Length > 255)
                errors.Add("Ulica nie może być dłuższa niż 255 znaków");

            if (!string.IsNullOrWhiteSpace(patient.PropertyNumber) && patient.PropertyNumber.Length > 255)
                errors.Add("Numer nieruchomości nie może być dłuższy niż 255 znaków");

            if (!string.IsNullOrWhiteSpace(patient.HouseUnitNumber) && patient.HouseUnitNumber.Length > 255)
                errors.Add("Numer lokalu nie może być dłuższy niż 255 znaków");

            if (!string.IsNullOrWhiteSpace(patient.PESEL) && patient.PESEL.Length == 11
                && CzySameLiczby(patient.PESEL)
                && !string.IsNullOrWhiteSpace(patient.BirthDate)
                && CzyPoprawnaDates(patient.BirthDate))
            {
                string dataZPesel = WyciagnijDateZPesel(patient.PESEL);
                if (dataZPesel != patient.BirthDate)
                    errors.Add("Data urodzenia nie zgadza się z datą zakodowaną w PESEL");
            }

            if (!string.IsNullOrWhiteSpace(patient.Gender)
                && (patient.Gender == "M" || patient.Gender == "K")
                && !string.IsNullOrWhiteSpace(patient.PESEL)
                && CzySameLiczby(patient.PESEL))
            {
                if (!CzyPlecZgadzaSieZPesel(patient.Gender, patient.PESEL))
                {
                    errors.Add("Płeć nie zgadza się z płcią zakodowaną w PESEL");
                }
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

        private string WyciagnijPlecZPesel(string pesel)
        {
            int plecKod = int.Parse(pesel.Substring(9, 1));
            string plec = (plecKod % 2 == 0) ? "K" : "M";
            return plec;
        }

        private bool CzyPlecZgadzaSieZPesel(string plec, string pesel)
        {
            if (string.IsNullOrWhiteSpace(plec) || string.IsNullOrWhiteSpace(pesel) || pesel.Length != 11 || !CzySameLiczby(pesel))
                return false;
            string plecZPesel = WyciagnijPlecZPesel(pesel);
            return plec == plecZPesel;
        }

        public List<string> CzyPoprawneHaslo(int userID, string haslo)
        {
            List<string> errros = new List<string>();

            if (string.IsNullOrWhiteSpace(haslo))
            {
                errros.Add("Hasło nie może być puste");
            }
            else
            {
                if (haslo.Length < 8)
                {
                    errros.Add("Hasło musi mieć co najmniej 8 znaków");
                }
                else if (haslo.Length > 15)
                {
                    errros.Add("Hasło nie może być dłuższe niż 15 znaków");
                }

                if (!haslo.Any(char.IsUpper))
                {
                    errros.Add("Hasło musi zawierać co najmniej jedną wielką literę");
                }

                if (!haslo.Any(char.IsLower))
                {
                    errros.Add("Hasło musi zawierać co najmniej jedną małą literę");
                }

                if (!haslo.Any(char.IsDigit))
                {
                    errros.Add("Hasło musi zawierać co najmniej jedną cyfrę");
                }

                if (!haslo.Any(ch => "!@#$%".Contains(ch)))
                {
                    errros.Add("Hasło musi zawierać co najmniej jeden ze znaków specjalnych @!#$%");
                }

                List<string> history = _repository.GetUserPasswordHistory(userID);
                // histora hasła jest od najnowszego do najstarszego

                for(int i = 0; i < 3 && i < history.Count; i++)
                {
                    if (haslo == history[i])
                    {
                        errros.Add($"Hasło nie może być takie samo jak jedno z 3 ostatnich haseł");
                        break;
                    }
                }


            }
               
            return errros;
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
