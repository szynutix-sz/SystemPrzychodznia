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

        private void LoadUsers()
        {
            var users = _userService.GetListUsers();
            _bindingSource.DataSource = users;
            dgvUsers.DataSource = _bindingSource;

            // Opcjonalne formatowanie kolumn
            dgvUsers.Columns["Login"].HeaderText = "Login";
            dgvUsers.Columns["FirstName"].HeaderText = "Imię";
            dgvUsers.Columns["LastName"].HeaderText = "Nazwisko";
            dgvUsers.Columns["Email"].HeaderText = "Email";
            dgvUsers.Columns["PESEL"].HeaderText = "PESEL";
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
            LoadUsers();
        }
    }
}
