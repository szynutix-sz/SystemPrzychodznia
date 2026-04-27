using SQLitePCL;
using SystemPrzychodznia.Data;
using SystemPrzychodznia.Services;

namespace SystemPrzychodznia
{

    public partial class FormUserView : Form
    {
        private readonly UserService _userService;
        private BindingSource _bindingSourceUsers = new BindingSource();
        private BindingSource _bindingSourceForgotten = new BindingSource();
        private UserFull _currentUser;
        private IdHolder _userIDfromLogin;
        private string _version;

        public FormUserView(IdHolder userID, UserService userService, string version)
        {

            _userService = userService;
            _currentUser = _userService.GetUserFull(userID.Id);
            _userIDfromLogin = userID;
            _version = version;

            InitializeComponent();

            tabControlUserView.TabPages.Clear();

            grantTabsToCurrentUser();


            this.Text = $"System Psychodnia - Zalogowano jako: {_currentUser.Login}";
        }

        private void LoadAbout()
        {
            labelVersion.Text = _version;


        }

        private void ClearSearchFields()
        {
            textBoxLogin.Text = "";
            textBoxFirstName.Text = "";
            textBoxLastName.Text = "";
            textBoxPESEL.Text = "";
            textBoxEmail.Text = "";

            populateUprawnienia();

            LoadUsers();
            UserList.Text = "Lista użytkowników";
        }

        private void ClearForgottenSearchFields()
        {
            textBoxForgottenLogin.Text = "";
            textBoxForgottenId.Text = "";
            textBoxForgottenFirstName.Text = "";
            textBoxForgottenLastName.Text = "";
            dateTimePickerForgottenDate.Checked = false;
            textBoxForgottenBy.Text = "";

            LoadForgottenUsers();
            groupBoxForgotten.Text = "Zapomnieni Użytkownicy";
        }

        private void LoadUsers()
        {
            SearchTerms s = new SearchTerms();
            s.Login = textBoxLogin.Text;
            s.FirstName = textBoxFirstName.Text;
            s.LastName = textBoxLastName.Text;
            s.PESEL = textBoxPESEL.Text;
            s.Email = textBoxEmail.Text;

            List<CheckBox> checkBoxes = flowLayoutPanelUprawnienia.Controls.OfType<CheckBox>().ToList();

            foreach (CheckBox box in checkBoxes)
            {
                if (box == null) continue;

                bool? p = null;
                if (box.CheckState == CheckState.Checked) p = true;

                else if (box.CheckState == CheckState.Indeterminate) p = false;
                // z niewiadomych przyczyn Indeterminate jest traktowane jako negatywne, a nie jako nieznane

                s.Uprawnienia.Add(new Uprawnienie { Id = (int)box.Tag, Posiadane = p, Nazwa = box.Text });
            }

            var users = _userService.GetListUsers(s);

            _bindingSourceUsers.DataSource = users;
            dgvUsers.DataSource = _bindingSourceUsers;

            if (users.Count == 0)
            {
                MessageBox.Show("Nie znaleziono użytkowników spełniających kryteria wyszukiwania.", "Brak wyników");
            }

            dgvUsers.Columns["Id"].HeaderText = "ID";
            dgvUsers.Columns["Id"].Width = 70;
            dgvUsers.Columns["Id"].DisplayIndex = 0;

            dgvUsers.Columns["Login"].HeaderText = "Login";
            dgvUsers.Columns["Login"].Width = 180;

            dgvUsers.Columns["FirstName"].HeaderText = "Imię";
            dgvUsers.Columns["FirstName"].Width = 160;

            dgvUsers.Columns["LastName"].HeaderText = "Nazwisko";
            dgvUsers.Columns["LastName"].Width = 160;

            dgvUsers.Columns["Email"].HeaderText = "Email";
            dgvUsers.Columns["Email"].Width = 240;

            dgvUsers.Columns["PESEL"].HeaderText = "PESEL";
            dgvUsers.Columns["PESEL"].Width = 140;

            dgvUsers.RowsDefaultCellStyle.BackColor = Color.White;
            dgvUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.RowHeadersVisible = false;
        }

        private void LoadForgottenUsers()
        {
            SearchTermsForgotten searchTerms = new SearchTermsForgotten
            {
                Login = textBoxForgottenLogin.Text.Trim(),
                FirstName = textBoxForgottenFirstName.Text.Trim(),
                LastName = textBoxForgottenLastName.Text.Trim(),
                DateForgotten = dateTimePickerForgottenDate.Checked
                    ? dateTimePickerForgottenDate.Value.ToString("yyyy-MM-dd")
                    : ""
            };

            if (int.TryParse(textBoxForgottenId.Text.Trim(), out int forgottenUserId))
            {
                searchTerms.Id = forgottenUserId;
            }

            if (int.TryParse(textBoxForgottenBy.Text.Trim(), out int forgottenBy))
            {
                searchTerms.ForgottenBy = forgottenBy;
            }

            var users = _userService.GetListForgottenUsers(searchTerms);

            _bindingSourceForgotten.DataSource = users;
            dgvForgotten.DataSource = _bindingSourceForgotten;

            // Pokaż tylko dane wymagane w przypadku użycia UC_1.6.
            var keep = new[] { "Id", "Login", "FirstName", "LastName", "DateForgotten", "ForgottenBy" };
            foreach (DataGridViewColumn col in dgvForgotten.Columns)
            {
                col.Visible = Array.IndexOf(keep, col.Name) >= 0;
            }

            if (dgvForgotten.Columns.Contains("Id"))
            {
                dgvForgotten.Columns["Id"].HeaderText = "ID";
                dgvForgotten.Columns["Id"].Width = 70;
                dgvForgotten.Columns["Id"].DisplayIndex = 0;
            }

            // Ustaw nagłówki, szerokości i kolejność
            if (dgvForgotten.Columns.Contains("DateForgotten"))
            {
                dgvForgotten.Columns["DateForgotten"].HeaderText = "Data zapomnienia";
                dgvForgotten.Columns["DateForgotten"].Width = 180;
                dgvForgotten.Columns["DateForgotten"].DisplayIndex = 4;
            }

            if (dgvForgotten.Columns.Contains("Login"))
            {
                dgvForgotten.Columns["Login"].HeaderText = "Login po zapomnieniu";
                dgvForgotten.Columns["Login"].Width = 180;
                dgvForgotten.Columns["Login"].DisplayIndex = 1;
            }

            if (dgvForgotten.Columns.Contains("FirstName"))
            {
                dgvForgotten.Columns["FirstName"].HeaderText = "Imię";
                dgvForgotten.Columns["FirstName"].Width = 160;
                dgvForgotten.Columns["FirstName"].DisplayIndex = 2;
            }

            if (dgvForgotten.Columns.Contains("LastName"))
            {
                dgvForgotten.Columns["LastName"].HeaderText = "Nazwisko po zapomnieniu";
                dgvForgotten.Columns["LastName"].Width = 180;
                dgvForgotten.Columns["LastName"].DisplayIndex = 3;
            }

            if (dgvForgotten.Columns.Contains("ForgottenBy"))
            {
                dgvForgotten.Columns["ForgottenBy"].HeaderText = "ID zapominającego";
                dgvForgotten.Columns["ForgottenBy"].Width = 120;
                dgvForgotten.Columns["ForgottenBy"].DisplayIndex = 5;
            }

            dgvForgotten.RowsDefaultCellStyle.BackColor = Color.White;
            dgvForgotten.AlternatingRowsDefaultCellStyle.BackColor = Color.MistyRose;
            dgvForgotten.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvForgotten.RowHeadersVisible = false;
        }
        private void populateUprawnienia()
        {
            flowLayoutPanelUprawnienia.Controls.Clear();
            foreach (Uprawnienie u in _userService.GetUprawnienia())
            {
                CheckBox cBox = new CheckBox();
                cBox.Text = u.Nazwa;
                cBox.ThreeState = true;
                cBox.Width = 110;
                cBox.Height = 40;
                cBox.Tag = u.Id;
                cBox.CheckState = CheckState.Unchecked;
                flowLayoutPanelUprawnienia.Controls.Add(cBox);
            }
        }
        private void FormAdminView_Load(object sender, EventArgs e)
        {
            populateUprawnienia();

        }

        private void buttonAddUser_Click(object sender, EventArgs e)
        {
            using (Form AddUser = new FormAddUser(_userService))
            {
                AddUser.ShowDialog();
            }
            LoadUsers();
        }

        private void FormAdminView_Activated(object sender, EventArgs e)
        {
            //ClearSearchFields();
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

        private void buttonSearchForgotten_Click(object sender, EventArgs e)
        {
            LoadForgottenUsers();
            groupBoxForgotten.Text = "Zapomnieni Użytkownicy (przefiltrowani)";
        }

        private void buttonClearForgottenSearch_Click(object sender, EventArgs e)
        {
            ClearForgottenSearchFields();
        }

        private void buttonRoles_Click(object sender, EventArgs e)
        {
            tabControlUserView.SelectedTab = tabPageRoles;
        }

        private void buttonUsersPerRole_Click(object sender, EventArgs e)
        {
            using (Form rolesForm = new FormRoles(_userService))
            {
                rolesForm.ShowDialog();
            }
        }

        private void LoadRoles()
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("Nazwa roli");
            dt.Columns.Add("Opis");
            dt.Columns.Add("Zakres dostępu");

            dt.Rows.Add("SuperAdmin",
                "Konto systemowe.",
                "Pełny dostęp do wszystkich modułów systemu. Nie może być modyfikowane ani usunięte.");
            dt.Rows.Add("Admin",
                "Administrator systemu.",
                "Zarządzanie użytkownikami: dodawanie, edycja danych, zapominanie kont (RODO). Przeglądanie list użytkowników i historii.");
            dt.Rows.Add("Lekarz",
                "Lekarz prowadzący.",
                "Dostęp do dokumentacji medycznej pacjentów, prowadzenie wizyt, wystawianie skierowań i recept.");
            dt.Rows.Add("Recepcja",
                "Pracownik recepcji.",
                "Rejestracja pacjentów, umawianie i zarządzanie wizytami, obsługa harmonogramu przyjęć.");
            dt.Rows.Add("Brak_roli",
                "Użytkownik bez przypisanej roli.",
                "Dostęp ograniczony wyłącznie do podglądu własnych danych osobowych.");

            dgvRoles.DataSource = dt;

            dgvRoles.Columns[0].Width = 160;
            dgvRoles.Columns[1].Width = 220;
            dgvRoles.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvRoles.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvRoles.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvRoles.RowsDefaultCellStyle.BackColor = Color.White;
            dgvRoles.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgvRoles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRoles.RowHeadersVisible = false;
        }

        private void dgvUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedUser = (UserBasic)_bindingSourceUsers[e.RowIndex];
                using (Form EditUser = new FormEditUser(_userService, selectedUser, _currentUser))
                {
                    EditUser.ShowDialog();
                }
                LoadUsers();
                LoadForgottenUsers();
            }
        }

        private void buttonLogOut_Click(object sender, EventArgs e)
        {
            _userIDfromLogin.loggedIn = false;
            this.Close();
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
