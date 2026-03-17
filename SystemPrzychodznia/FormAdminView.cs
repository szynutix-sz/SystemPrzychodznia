using SystemPrzychodznia.Data;
using SystemPrzychodznia.Services;

namespace SystemPrzychodznia
{

    public partial class FormAdminView : Form
    {
        private UserService _userService = new UserService();
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

            // Opcjonalne formatowanie kolumn
            dgvUsers.Columns["Login"].HeaderText = "Login";
            dgvUsers.Columns["FirstName"].HeaderText = "Imię";
            dgvUsers.Columns["LastName"].HeaderText = "Nazwisko";
            dgvUsers.Columns["Email"].HeaderText = "Email";
            dgvUsers.Columns["PESEL"].HeaderText = "PESEL";
            dgvUsers.AutoResizeColumns();
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

        private void dgvUsers_CellDoubleClick (object sender, DataGridViewCellEventArgs e)
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

    }
}
