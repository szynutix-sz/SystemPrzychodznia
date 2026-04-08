using System;
using System.Windows.Forms;
using SystemPrzychodznia.Data;
using SystemPrzychodznia.Services;

namespace SystemPrzychodznia
{
    public partial class FormAdminView : Form
    {
        private readonly UserService _userService = new UserService();
        private BindingSource _bindingSource = new BindingSource();

        public FormAdminView()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void ClearSearchFields()
        {
            textBoxLogin.Text = "";
            textBoxFirstName.Text = "";
            textBoxLastName.Text = "";
            textBoxPESEL.Text = "";
            textBoxEmail.Text = "";
            LoadUsers();
            UserList.Text = "Lista użytkowników";
        }

        
        private void FormatGridColumnsForUsers()
        {
            if (dgvUsers.Columns.Contains("Login")) dgvUsers.Columns["Login"].HeaderText = "Login";
            if (dgvUsers.Columns.Contains("FirstName")) dgvUsers.Columns["FirstName"].HeaderText = "Imię";
            if (dgvUsers.Columns.Contains("LastName")) dgvUsers.Columns["LastName"].HeaderText = "Nazwisko";
            if (dgvUsers.Columns.Contains("Email")) dgvUsers.Columns["Email"].HeaderText = "Email";
            if (dgvUsers.Columns.Contains("PESEL")) dgvUsers.Columns["PESEL"].HeaderText = "PESEL";
            dgvUsers.AutoResizeColumns();
        }

        private void LoadUsers()
        {
            SearchTerms s = new SearchTerms();
            s.Login = textBoxLogin.Text;
            s.FirstName = textBoxFirstName.Text;
            s.LastName = textBoxLastName.Text;
            s.PESEL = textBoxPESEL.Text;
            s.Email = textBoxEmail.Text;
            var users = _userService.GetListUsers(s);
            _bindingSource.DataSource = users;
            dgvUsers.DataSource = _bindingSource;

            FormatGridColumnsForUsers();
        }

        private void FormAdminView_Load(object sender, EventArgs e)
        {

        }

        private void buttonAddUser_Click(object sender, EventArgs e)
        {
            using (Form AddUser = new FormAddUser(_userService))
            {
                AddUser.ShowDialog();
            }
        }

        private void FormAdminView_Activated(object sender, EventArgs e)
        {
            ClearSearchFields();
        }

        private void buttonSearchUser_Click(object sender, EventArgs e)
        {
            LoadUsers();
            UserList.Text = "Lista użytkowników (przefiltrowana)";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void buttonClearSearch_Click(object sender, EventArgs e)
        {
            ClearSearchFields();
        }

        private void dgvUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Zabezpieczenie: Edycja działa tylko jeśli kliknięto w normalnego użytkownika
                if (_bindingSource[e.RowIndex] is User selectedUser)
                {
                    using (Form EditUser = new FormEditUser(_userService, selectedUser))
                    {
                        EditUser.ShowDialog();
                    }
                }
            }
        }

        // ========================================================
        // NOWE FUNKCJE - RODO I WYSZUKIWANIE PO UPRAWNIENIACH
        // ========================================================

        private void btnForget_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow != null)
            {
                // Upewniamy się, że zaznaczono zwykłego usera, a nie tego z listy zapomnianych
                if (_bindingSource.Current is User currentUser)
                {
                    // Pobieramy pełne dane, żeby poznać ID użytkownika 
                    var fullUser = _userService.GetUserFull(currentUser.Login);

                    // ZABEZPIECZENIE: Blokada usuwania SuperAdmina
                    if (fullUser.Login == "SuperAdmin")
                    {
                        MessageBox.Show("Nie możesz zanonimizować konta SuperAdmina!", "Blokada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Przerwij działanie metody!
                    }

                    int currentAdminId = 1; // UWAGA: Zakładamy że Admin ma Id = 1.

                    var confirmResult = MessageBox.Show("Czy na pewno chcesz zanonimizować tego użytkownika? Tej operacji nie można cofnąć.",
                                                 "Potwierdzenie RODO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (confirmResult == DialogResult.Yes)
                    {
                        _userService.ForgetSystemUser(fullUser.Id, currentAdminId);
                        MessageBox.Show("Zgodnie z RODO, użytkownik został zanonimizowany.");
                        LoadUsers(); // Odświeżamy listę, by usunąć z niej zapomnianego
                    }
                }
                else
                {
                    MessageBox.Show("Ten użytkownik jest już zanonimizowany!");
                }
            }
            else
            {
                MessageBox.Show("Najpierw zaznacz użytkownika na liście!");
            }
        }

        private void btnLoadForgotten_Click(object sender, EventArgs e)
        {
            var forgotten = _userService.GetForgottenUsers(null);
            _bindingSource.DataSource = forgotten;
            dgvUsers.DataSource = _bindingSource;
            UserList.Text = "Lista zapomnianych użytkowników";

            // Formatowanie kolumn specjalnie dla zapomnianych
            if (dgvUsers.Columns.Contains("ImieINazwiskoPoZapomnieniu"))
            {
                dgvUsers.Columns["Identyfikator"].HeaderText = "ID";
                dgvUsers.Columns["ImieINazwiskoPoZapomnieniu"].HeaderText = "Imię i Nazwisko (Anonim)";
                dgvUsers.Columns["DataZapomnienia"].HeaderText = "Data zapomnienia";
                dgvUsers.Columns["IdKtoZapomnial"].HeaderText = "ID Admina RODO";
                dgvUsers.AutoResizeColumns();
            }
        }

        private void comboBoxRoleSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (sender is ComboBox comboBox && !string.IsNullOrEmpty(comboBox.Text))
            {
                string selectedRole = comboBox.Text;
                var usersWithRole = _userService.GetUsersByRole(selectedRole);
                _bindingSource.DataSource = usersWithRole;
                dgvUsers.DataSource = _bindingSource;
                UserList.Text = $"Lista użytkowników (Rola: {selectedRole})";

                FormatGridColumnsForUsers();
            }
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void comboBoxRoleSearches_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox comboBox && !string.IsNullOrEmpty(comboBox.Text))
            {
                string selectedRole = comboBox.Text;

                // Odpytujemy bazę o użytkowników z tą rolą
                var usersWithRole = _userService.GetUsersByRole(selectedRole);

                // Aktualizujemy tabelę na ekranie
                _bindingSource.DataSource = usersWithRole;
                dgvUsers.DataSource = _bindingSource;

                // Zmieniamy tytuł nad tabelą
                UserList.Text = $"Lista użytkowników (Rola: {selectedRole})";

                // Przywracamy ładne nazwy kolumn
                FormatGridColumnsForUsers();
            }
        }
    }
}