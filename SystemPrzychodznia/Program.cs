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

            DatabaseInitializer.Initialize();

            UserService _userService = new UserService();
            IdHolder _userID = new IdHolder();

            ApplicationConfiguration.Initialize();
            
            Application.Run(new FormLogin(_userID, _userService));
            Application.Run(new FormUserView(_userID, _userService));
        }
    }
}