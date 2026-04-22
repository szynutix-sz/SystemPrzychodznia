using System;
using System.Drawing;
using System.Windows.Forms;
using SystemPrzychodznia.Services;

namespace SystemPrzychodznia.UI // <--- TUTAJ DODALIŚMY .UI
{
    public partial class FormChangePassword : Form
    {
        private readonly UserService _userService;
        private readonly int _userId;

        private Label lblNew = new Label { Text = "Nowe hasło:", Location = new Point(30, 30), AutoSize = true };
        private TextBox txtNew = new TextBox { Location = new Point(30, 50), Width = 200, UseSystemPasswordChar = true };
        private Label lblConfirm = new Label { Text = "Powtórz hasło:", Location = new Point(30, 90), AutoSize = true };
        private TextBox txtConfirm = new TextBox { Location = new Point(30, 110), Width = 200, UseSystemPasswordChar = true };
        private Button btnSave = new Button { Text = "Zapisz i zaloguj", Location = new Point(30, 150), Width = 200 };

        public FormChangePassword(UserService userService, int userId)
        {
            _userService = userService;
            _userId = userId;

            this.Text = "Wymuszona zmiana hasła";
            this.Size = new Size(280, 240);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.Controls.Add(lblNew);
            this.Controls.Add(txtNew);
            this.Controls.Add(lblConfirm);
            this.Controls.Add(txtConfirm);
            this.Controls.Add(btnSave);

            btnSave.Click += BtnSave_Click;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (txtNew.Text.Length < 5)
            {
                MessageBox.Show("Hasło musi mieć co najmniej 5 znaków.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtNew.Text != txtConfirm.Text)
            {
                MessageBox.Show("Hasła nie są identyczne!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Zapis nowego hasła i usunięcie flagi wymagań zmiany z bazy
            _userService.ChangeUserPassword(_userId, txtNew.Text);

            MessageBox.Show("Hasło zostało pomyślnie zmienione.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}