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
        private bool _canLoadUsers = false;

        public FormUserView(IdHolder userID, UserService userService)
        {

            _userService = userService;
            _currentUser = _userService.GetUserFull(userID.Id);


            if (_currentUser.Uprawnienia.Exists(u =>
               (u.Id == 1 && u.Posiadane == true) ||
               (u.Id == 2 && u.Posiadane == true)
               )
              )
            {
                _canLoadUsers = true;
            }


            InitializeComponent();

            tabControlUserView.TabPages.Clear();


            if (_currentUser.Uprawnienia.Exists(u =>
                (u.Id == 1 && u.Posiadane == true) ||
                (u.Id == 2 && u.Posiadane == true)
                )
               )
            {
                tabControlUserView.TabPages.Add(tabPageAdminViewUsers);
                tabControlUserView.TabPages.Add(tabPageAdminViewForgotten);
            }

            tabControlUserView.TabPages.Add(tabPageAbout);


            populateUprawnienia();
            LoadUsers();
            LoadForgottenUsers();

            this.Text = $"System Psychodnia - Zalogowano jako: {_currentUser.Login}";
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

        private void LoadUsers()
        {
            if (_canLoadUsers == false) return;
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

            dgvUsers.Columns["Id"].Visible = false;

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
            if (_canLoadUsers == false) return;

            var users = _userService.GetListForgottenUsers();

            _bindingSourceForgotten.DataSource = users;
            dgvForgotten.DataSource = _bindingSourceForgotten;

            dgvForgotten.Columns["Id"].Visible = false;
            dgvForgotten.Columns["PESEL"].Visible = false;
            dgvForgotten.Columns["BirthDate"].Visible = false;
            dgvForgotten.Columns["Gender"].Visible = false;

            dgvForgotten.Columns["Login"].HeaderText = "Zaszyfrowany Login";
            dgvForgotten.Columns["Login"].Width = 220;

            dgvForgotten.Columns["FirstName"].HeaderText = "Zaszyfrowane Imię";
            dgvForgotten.Columns["FirstName"].Width = 180;

            dgvForgotten.Columns["LastName"].HeaderText = "Zaszyfrowane Nazwisko";
            dgvForgotten.Columns["LastName"].Width = 180;

            dgvForgotten.Columns["DateForgotten"].HeaderText = "Data zapomnienia";
            dgvForgotten.Columns["DateForgotten"].Width = 200;

            dgvForgotten.Columns["ForgottenBy"].HeaderText = "ID Administratora";
            dgvForgotten.Columns["ForgottenBy"].Width = 150;

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

        private void buttonRoles_Click(object sender, EventArgs e)
        {
            using (Form rolesForm = new FormRoles(_userService))
            {
                rolesForm.ShowDialog();
            }
        }

        private void dgvUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedUser = (User)_bindingSourceUsers[e.RowIndex];
                using (Form EditUser = new FormEditUser(_userService, selectedUser, _currentUser))
                {
                    EditUser.ShowDialog();
                }
                LoadUsers();
                LoadForgottenUsers();
            }
        }

    }
}
