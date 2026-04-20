using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SystemPrzychodznia.Data;
using SystemPrzychodznia.Services;

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

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }

        private void labelLogin_Click(object sender, EventArgs e)
        {

        }

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
                _userID.Id = result.userId;
                this.Close();
            }
            else
            {
                MessageBox.Show(result.message, "Błąd logowania", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBoxLogin.Text.Trim();

            if (string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("Wpisz login, aby odzyskać hasło.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = _userService.RecoverPassword(login);
            MessageBox.Show(result.message, "Odzyskiwanie hasła", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}