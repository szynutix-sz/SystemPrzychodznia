using System;
using System.Windows.Forms;
using SystemPrzychodznia.Services;

namespace SystemPrzychodznia.UI
{
    public partial class FormRecoverPass : Form
    {
        private readonly UserService _userService;

        public FormRecoverPass(UserService userService)
        {
            InitializeComponent();
            _userService = userService;
            this.Text = "Odzyskiwanie hasła";
            this.StartPosition = FormStartPosition.CenterParent;
        }

        // Metoda obsługująca kliknięcie przycisku "Odzyskaj Hasło"
        private void btnRecover_Click(object sender, EventArgs e)
        {
            // 1. Pobranie danych z pól tekstowych
            string login = txtLogin.Text.Trim();
            string email = txtEmail.Text.Trim();

            // 2. Wstępna walidacja czy pola nie są puste
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Proszę podać login oraz adres e-mail.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 3. Wywołanie logiki biznesowej z UserService
            // Zgodnie z wymaganiami: generuje nowe hasło, zapisuje w bazie i wysyła maila
            var result = _userService.RecoverPassword(login, email);

            if (result.success)
            {
                // Wyświetlenie komunikatu o sukcesie (Wymaganie nr 2)
                MessageBox.Show(result.message, "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Zamknięcie okna po pomyślnym wysłaniu
            }
            else
            {
                // Wyświetlenie ogólnego komunikatu o błędzie (Wymaganie nr 3 i 4)
                // Nie informujemy, czy błędny był login czy email
                MessageBox.Show(result.message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}