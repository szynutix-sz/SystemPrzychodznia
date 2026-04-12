using SystemPrzychodznia.Data;
using SystemPrzychodznia.Services;

namespace SystemPrzychodznia
{

    public partial class FormUserView : Form
    {
        private readonly UserService _userService = new UserService();
        private BindingSource _bindingSource = new BindingSource();
        public FormUserView()
        {
            InitializeComponent();
            populateUprawnienia();
            LoadUsers();
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

            _bindingSource.DataSource = users;
            dgvUsers.DataSource = _bindingSource;

            if (users.Count == 0)
            {
                MessageBox.Show("Nie znaleziono użytkowników spełniających kryteria wyszukiwania.", "Brak wyników");
            }

            // Opcjonalne formatowanie kolumn
            dgvUsers.Columns["Login"].HeaderText = "Login";
            dgvUsers.Columns["Login"].Width = 250;

            dgvUsers.Columns["FirstName"].HeaderText = "Imię";
            dgvUsers.Columns["FirstName"].Width = 250;

            dgvUsers.Columns["LastName"].HeaderText = "Nazwisko";
            dgvUsers.Columns["LastName"].Width = 250;

            dgvUsers.Columns["Email"].HeaderText = "Email";
            dgvUsers.Columns["Email"].Width = 350;

            dgvUsers.Columns["PESEL"].HeaderText = "PESEL";
            dgvUsers.Columns["PESEL"].Width = 150;


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

        private void dgvUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedUser = (User)_bindingSource[e.RowIndex];
                using (Form EditUser = new FormEditUser(_userService, selectedUser))
                {
                    EditUser.ShowDialog();
                }
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
