using SystemPrzychodznia.Data;
using SystemPrzychodznia.Services;

namespace SystemPrzychodznia
{
    public partial class FormRoles : Form
    {
        private readonly UserService _userService;
        private List<Uprawnienie> _roles = new();

        public FormRoles(UserService userService)
        {
            _userService = userService;
            InitializeComponent();
        }

        private void FormRoles_Load(object sender, EventArgs e)
        {
            _roles = _userService.GetUprawnienia();
            listBoxRoles.Items.Clear();
            foreach (var role in _roles)
                listBoxRoles.Items.Add(role.Nazwa);

            if (listBoxRoles.Items.Count > 0)
                listBoxRoles.SelectedIndex = 0;
        }

        private void listBoxRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxRoles.SelectedIndex < 0) return;
            LoadUsersForRole(_roles[listBoxRoles.SelectedIndex].Id);
        }

        private void LoadUsersForRole(int roleId)
        {
            var users = _userService.GetUsersByRole(roleId);
            dgvRoleUsers.DataSource = users;

            dgvRoleUsers.Columns["Id"].Visible = false;
            dgvRoleUsers.Columns["Login"].HeaderText = "Login";
            dgvRoleUsers.Columns["Login"].Width = 180;
            dgvRoleUsers.Columns["FirstName"].HeaderText = "Imię";
            dgvRoleUsers.Columns["FirstName"].Width = 160;
            dgvRoleUsers.Columns["LastName"].HeaderText = "Nazwisko";
            dgvRoleUsers.Columns["LastName"].Width = 160;
            dgvRoleUsers.Columns["Email"].HeaderText = "Email";
            dgvRoleUsers.Columns["Email"].Width = 240;
            dgvRoleUsers.Columns["PESEL"].HeaderText = "PESEL";
            dgvRoleUsers.Columns["PESEL"].Width = 140;

            dgvRoleUsers.RowsDefaultCellStyle.BackColor = Color.White;
            dgvRoleUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
            dgvRoleUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRoleUsers.RowHeadersVisible = false;

            labelUserCount.Text = $"Użytkownicy z rolą: {users.Count}";
        }
    }
}
