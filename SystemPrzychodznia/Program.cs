using System.Reflection;
using System.Windows.Forms;
using SystemPrzychodznia.Data;
using SystemPrzychodznia.Services;

namespace SystemPrzychodznia
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            DatabaseInitializer.Initialize(false);

            UserService _userService = new UserService();
            

            ApplicationConfiguration.Initialize();

            var version = Assembly.GetExecutingAssembly()
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    ?.InformationalVersion ?? "Unknown";



            while (true)
            {
                IdHolder _userID = new IdHolder();
                Application.Run(new FormLogin(_userID, _userService));

                if (_userID.loggedIn)
                {
                    try
                    {
                        Application.Run(new FormUserView(_userID, _userService, version));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"Wystąpił błąd podczas otwierania widoku użytkownika:{Environment.NewLine}{ex.Message}",
                            "Błąd aplikacji",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        break;
                    }
                    if (_userID.loggedIn) break; //użytkownik wyszedł, ale się nie wylogował
                }
                else
                {
                    break;
                }
            }
        }
    }
}
