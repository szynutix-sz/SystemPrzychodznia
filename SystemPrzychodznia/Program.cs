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
            
            

            while (true)
            {
                IdHolder _userID = new IdHolder();
                Application.Run(new FormLogin(_userID, _userService));

                if (_userID.loggedIn)
                {
                    Application.Run(new FormUserView(_userID, _userService));
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