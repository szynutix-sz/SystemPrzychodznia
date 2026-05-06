using Microsoft.VisualBasic.ApplicationServices;
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
                errors.Add(RequiredFieldMessage("Adres e-mail"));
            else
                errors.AddRange(ValidateEmail(user.Email));

            // Walidacja PESEL - musi mieć 11 znaków i być liczbą
            if (string.IsNullOrWhiteSpace(user.PESEL))
                errors.Add(RequiredFieldMessage("PESEL"));
            else if (user.PESEL.Length != 11 || !CzySameLiczby(user.PESEL))
                errors.Add("Nieprawidłowy PESEL. Ilość znaków jest nieprawidłowa. Prawidłowy numer PESEL powinien zawierać 11 znaków");

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
                errors.Add(RequiredFieldMessage("Numer telefonu"));
            else if (user.Phone.Length != 9 || !CzySameLiczby(user.Phone))
                errors.Add("Nieprawidłowy numer telefonu. Ilość cyfr telefonu nie jest zgodna. Prawidłowa ilość to 9 cyfr");


            if (validatePassword)
            {
                errors.AddRange(CzyPoprawneHaslo(user.Id, user.Password));
            }
            // sprawdzenie czy hasło jest takie samo jak potwierdzenie hasła odbywa się w kontrolerze, więc nie trzeba tego sprawdzać tutaj


            // Walidacja unikalności loginu, email, PESEL
            if (!string.IsNullOrWhiteSpace(user.Login) &&
                _repository.CzyIstniejeDanyUżytkowik(user.Login,UserRepository.VAL_LOGIN, editing, user.Id))
                errors.Add("Login jest zajęty");

            if (!string.IsNullOrWhiteSpace(user.Email) &&
                ValidateEmail(user.Email).Count == 0 &&
                _repository.CzyIstniejeDanyUżytkowik(user.Email, UserRepository.VAL_EMAIL, editing, user.Id))
                errors.Add("Nieprawidłowy adres e-mail. E-mail jest już zarejestrowany w systemie");

            if (!string.IsNullOrWhiteSpace(user.PESEL) && user.PESEL.Length == 11 &&
                _repository.CzyIstniejeDanyUżytkowik(user.PESEL, UserRepository.VAL_PESEL, editing, user.Id))
                errors.Add("Nieprawidłowy PESEL. Podany numer PESEL już widnieje w systemie");



// Walidacja długości pól - max 255 znaków
            if (!string.IsNullOrWhiteSpace(user.Login) && user.Login.Length > 255)
                errors.Add("Login nie może być dłuższy niż 255 znaków");

            if (!string.IsNullOrWhiteSpace(user.FirstName) && user.FirstName.Length > 255)
                errors.Add("Imię nie może być dłuższe niż 255 znaków");

            if (!string.IsNullOrWhiteSpace(user.LastName) && user.LastName.Length > 255)
                errors.Add("Nazwisko nie może być dłuższe niż 255 znaków");

            // Walidacja czy data urodzenia zgadza się z datą zakodowaną w PESEL oraz sprawdzenie sumy kontronej
            if (!string.IsNullOrWhiteSpace(user.PESEL) && user.PESEL.Length == 11
                && CzySameLiczby(user.PESEL)
                && !string.IsNullOrWhiteSpace(user.BirthDate)
                && CzyPoprawnaDates(user.BirthDate))
            {
                string dataZPesel = WyciagnijDateZPesel(user.PESEL);
                if (dataZPesel != user.BirthDate)
                    errors.Add("Nieprawidłowy PESEL. Data urodzenia nie jest zgodna z zasadami RRMMDD");

                if (!CzyPESELKontrola(user.PESEL))
                    errors.Add("Nieprawidłowy PESEL. Cyfra kontrolna nie zgadza się z zasadami https://www.gov.pl/web/gov/czym-jest-numer-pesel");
            }

            // Walidacja płeć zgadza się z płcią zakodowaną w PESEL
            if (!string.IsNullOrWhiteSpace(user.Gender)
                && (user.Gender == "M" || user.Gender == "K")
                && !string.IsNullOrWhiteSpace(user.PESEL)
                && user.PESEL.Length == 11
                && CzySameLiczby(user.PESEL))
            {
                if (!CzyPlecZgadzaSieZPesel(user.Gender, user.PESEL))
                {
                    errors.Add("Nieprawidłowy PESEL. Płeć nie jest zgodna z zasadami przedostatniej cyfry");
                }
            }



             
            return new ValidationResult(errors.Count == 0) { Errors = errors };
        }

        public ValidationResult ValidatePatientFull(PatientFull patient, bool editing)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(patient.FirstName))
                errors.Add(RequiredFieldMessage("Imię"));

            if (string.IsNullOrWhiteSpace(patient.LastName))
                errors.Add(RequiredFieldMessage("Nazwisko"));

            if (string.IsNullOrWhiteSpace(patient.PESEL))
                errors.Add(RequiredFieldMessage("PESEL"));
            else if (patient.PESEL.Length != 11 || !CzySameLiczby(patient.PESEL))
                errors.Add("Nieprawidłowy PESEL. Ilość znaków jest nieprawidłowa. Prawidłowy numer PESEL powinien zawierać 11 znaków");

            if (string.IsNullOrWhiteSpace(patient.Locality))
                errors.Add(RequiredFieldMessage("Miejscowość"));

            if (string.IsNullOrWhiteSpace(patient.PostalCode))
                errors.Add(RequiredFieldMessage("Kod pocztowy"));
            else if (patient.PostalCode.Length != 5)
                errors.Add("Kod pocztowy musi mieć 5 cyfr");
            else if (!CzySameLiczby(patient.PostalCode))
                errors.Add("Kod pocztowy musi zawierać tylko cyfry");

            if (string.IsNullOrWhiteSpace(patient.PropertyNumber))
                errors.Add(RequiredFieldMessage("Numer domu"));

            if (string.IsNullOrWhiteSpace(patient.BirthDate))
                errors.Add(RequiredFieldMessage("Data urodzenia"));
            else if (!CzyPoprawnaDates(patient.BirthDate))
                errors.Add("Data urodzenia musi być w formacie YYYY-MM-DD");

            if (string.IsNullOrWhiteSpace(patient.Gender))
                errors.Add(RequiredFieldMessage("Płeć"));
            else if (patient.Gender != "M" && patient.Gender != "K")
                errors.Add("Płeć musi być 'M' lub 'K'");

            if (string.IsNullOrWhiteSpace(patient.Phone))
                errors.Add(RequiredFieldMessage("Numer telefonu"));
            else if (patient.Phone.Length != 9 || !CzySameLiczby(patient.Phone))
                errors.Add("Nieprawidłowy numer telefonu. Ilość cyfr telefonu nie jest zgodna. Prawidłowa ilość to 9 cyfr.");

            if (!string.IsNullOrWhiteSpace(patient.Email))
            {
                errors.AddRange(ValidateEmail(patient.Email));
            }

            if (!string.IsNullOrWhiteSpace(patient.PESEL) && patient.PESEL.Length == 11 &&
                _repository.PatientPeselExists(patient.PESEL, editing, patient.Id))
            {
                errors.Add("Nieprawidłowy PESEL. Podany numer PESEL już widnieje w systemie");
            }

            if (!string.IsNullOrWhiteSpace(patient.Email) &&
                ValidateEmail(patient.Email).Count == 0 &&
                _repository.PatientEmailExists(patient.Email, editing, patient.Id))
            {
                errors.Add("Nieprawidłowy adres e-mail. E-mail jest już zarejestrowany w systemie");
            }

            if (!string.IsNullOrWhiteSpace(patient.FirstName) && patient.FirstName.Length > 255)
                errors.Add("Imię nie może być dłuższe niż 255 znaków");

            if (!string.IsNullOrWhiteSpace(patient.LastName) && patient.LastName.Length > 255)
                errors.Add("Nazwisko nie może być dłuższe niż 255 znaków");

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
                    errors.Add("Nieprawidłowy PESEL. Data urodzenia nie jest zgodna z zasadami RRMMDD");

                if (!CzyPESELKontrola(patient.PESEL))
                    errors.Add("Nieprawidłowy PESEL. Cyfra kontrolna nie zgadza się z zasadami https://www.gov.pl/web/gov/czym-jest-numer-pesel");
            }

            if (!string.IsNullOrWhiteSpace(patient.Gender)
                && (patient.Gender == "M" || patient.Gender == "K")
                && !string.IsNullOrWhiteSpace(patient.PESEL)
                && patient.PESEL.Length == 11
                && CzySameLiczby(patient.PESEL))
            {
                if (!CzyPlecZgadzaSieZPesel(patient.Gender, patient.PESEL))
                {
                    errors.Add("Nieprawidłowy PESEL. Płeć nie jest zgodna z zasadami przedostatniej cyfry");
                }
            }

            return new ValidationResult(errors.Count == 0) { Errors = errors };
        }

        private static string RequiredFieldMessage(string fieldName)
        {
            return $"Brak wypełnionego pola ({fieldName})";
        }

        private static List<string> ValidateEmail(string email)
        {
            var errors = new List<string>();

            if (email.Length > 255)
            {
                errors.Add("Nieprawidłowy adres e-mail. Liczba znaków w mailu przekracza próg 255 znaków");
            }

            int atCount = email.Count(c => c == '@');
            if (atCount == 0)
            {
                errors.Add("Nieprawidłowy adres e-mail. E-mail nie zawiera znaku: @");
                return errors;
            }

            if (atCount > 1)
            {
                errors.Add("Nieprawidłowy adres e-mail. Email zawiera więcej niż jeden znak: @");
                return errors;
            }

            int atIndex = email.IndexOf('@');
            if (atIndex == 0 || atIndex == email.Length - 1)
            {
                errors.Add("Nieprawidłowy adres e-mail. Email nie jest zgodny z ustaloną składnią nazwa_użytkownika@nazwa_domeny_serwera_poczty");
                return errors;
            }

            string domain = email[(atIndex + 1)..];
            int lastDotAfterAt = domain.LastIndexOf('.');
            if (lastDotAfterAt <= 0 || lastDotAfterAt == domain.Length - 1)
            {
                errors.Add("Nieprawidłowy adres e-mail. E-mail nie posiada domeny");
            }

            return errors;
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

        // Sprawdza sume kontrolną PESEL
        private bool CzyPESELKontrola(string PESEL)
        {
            int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            int sum = 0;
            for (int i = 0; i < 10; i++) sum += (int.Parse(PESEL.Substring(i, 1))) * weights[i];

            int controlDigit = (10 - (sum % 10)) % 10;

            return controlDigit == int.Parse(PESEL.Substring(10, 1));
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
