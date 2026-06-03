# SystemPrzychodznia

Efekt pracy na zaliczenie przedmiotu Testowanie Oprogramowania

System Przychodznia to aplikacja, która umożliwia zarządzanie danymi pacjentów, lekarzy oraz wizytami w przychodni. Aplikacja została stworzona w celu ułatwienia pracy personelu medycznego oraz poprawy organizacji pracy przychodni.

## Setup

### Z pliku .exe

Aby uruchomić aplikację, należy pobrać plik wykonywalny "SystemPrzychodznia.exe" z katalogu "bin/Debug/net10.0-windows" i uruchomić go na komputerze z systemem Windows.

### Build

Aby uruchomić aplikację, należy posiadać zainstalowane środowisko programistyczne Visual Studio 2026 oraz .NET 10.0 SDK.

W menu głównym Visual Studio 2026 wybierz "File" -> "Clone Repository" -> Wprowadź adres URL repozytorium: https://github.com/szynutix-sz/SystemPrzychodznia.git
Po sklonowwaniu "Debug" -> "Start without debugging" (lub naciśnij Ctrl + F5) aby uruchomić aplikację.
Po uruchomieniu aplikacji, zostanie wyświetlone okno logowania, które od razu trzeba zamknąć.

Pliki programu znajdują się w katalogu "SystemPrzychodznia" w folderze "bin/Debug/net10.0-windows". Plik wykonywalny to "SystemPrzychodznia.exe".

## Baza danych do testowania

Aby przetestować aplikację, należy użyć bazy danych o nazwie "baza_testowa.db". Znajduje sie on w katalogu głównym projektu. 
Baza danych zawiera przykładowe dane pacjentów, lekarzy oraz wizyt, które można wykorzystać do testowania funkcjonalności aplikacji.

### Setup bazy danych do testowania

Po zbudowaniu projektu, baza danych "baza_testowa.db" skopiować ją (/) 
Następnie zmienić nazwę pliku "przychodznia.db"  (/SystemPrzychodznia/bin/Debug/net10.0-windows) na "przychodznia_backup.db" (jeśli istnieje) i skopiować "baza_testowa.db" do katalogu wyjściowego, zmieniając jego nazwę na "przychodznia.db".