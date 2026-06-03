# SystemPrzychodznia

Efekt pracy na zaliczenie przedmiotu Testowanie Oprogramowania

System Przychodznia to aplikacja, która umożliwia zarządzanie danymi pacjentów, lekarzy oraz wizytami w przychodni. Aplikacja została stworzona w celu ułatwienia pracy personelu medycznego oraz poprawy organizacji pracy przychodni.

## Setup

W menu głównym Visual Studio 2026 wybierz "File" -> "Clone Repository" -> Wprowadź adres URL repozytorium: https://github.com/szynutix-sz/SystemPrzychodznia.git

## Baza danych do testowania

Aby przetestować aplikację, należy użyć bazy danych o nazwie "baza_testowa.db". Znajduje sie on w katalogu głównym projektu. 
Baza danych zawiera przykładowe dane pacjentów, lekarzy oraz wizyt, które można wykorzystać do testowania funkcjonalności aplikacji.

### Setup bazy danych do testowania

Po zbudowaniu projektu, baza danych "baza_testowa.db" skopiować ją (/) 
Następnie zmienić nazwę pliku "przychodznia.db"  (/bin/Debug/net10.0-windows) na "przychodznia_backup.db" (jeśli istnieje) i skopiować "baza_testowa.db" do katalogu wyjściowego, zmieniając jego nazwę na "przychodznia.db".