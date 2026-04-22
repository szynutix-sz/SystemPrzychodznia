using System;
using System.Windows.Forms;
using SystemPrzychodznia.Data;
using SystemPrzychodznia.Services;
using SystemPrzychodznia.UI;

namespace SystemPrzychodznia
{
    public partial class FormLogin : Form
    {
        private readonly UserService _userService;
        private IdHolder _userID;

        public FormLogin(IdHolder id, UserService userService)
        {
            _userID = id;
            _userService = userService;
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e) { }
        private void labelLogin_Click(object sender, EventArgs e) { }
        private void textBoxPassword_TextChanged(object sender, EventArgs e) { }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string login = textBoxLogin.Text.Trim();
            string password = textBoxPassword.Text;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Podaj login i hasło.", "Błąd logowania", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = _userService.AttemptLogin(login, password);

            if (result.success)
            {
                // SPRAWDZENIE: Czy hasło zostało wymuszone do zmiany przez zresetowanie e-mailem?
                if (result.requiresPasswordChange)
                {
                    MessageBox.Show("Logujesz się za pomocą hasła wygenerowanego przez system.\nZe względów bezpieczeństwa musisz ustalić swoje własne hasło.", "Wymiana hasła", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    using (var changePassForm = new FormChangePassword(_userService, result.userId))
                    {
                        if (changePassForm.ShowDialog() != DialogResult.OK)
                        {
                            // Użytkownik nie zmienił hasła (np. wcisnął krzyżyk) - zablokuj dostęp!
                            MessageBox.Show("Musisz zmienić hasło, aby się zalogować do systemu!", "Odmowa dostępu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                _userID.Id = result.userId;
                _userID.loggedIn = true;
                this.Close();
            }
            else
            {
                MessageBox.Show(result.message, "Błąd logowania", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var recoverForm = new FormRecoverPass(_userService))
            {
                recoverForm.ShowDialog();
            }
        }
    }
}