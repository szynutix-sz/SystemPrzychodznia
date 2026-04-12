using SystemPrzychodznia.Data;

namespace SystemPrzychodznia
{
    partial class FormUserView
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            UserList = new GroupBox();
            dgvUsers = new DataGridView();
            buttonAddUser = new Button();
            buttonSearchUser = new Button();
            tableLayoutPanelUsers = new TableLayoutPanel();
            labelLogin = new Label();
            labelFirstName = new Label();
            labelLastName = new Label();
            labelPESEL = new Label();
            labelEmail = new Label();
            textBoxLogin = new TextBox();
            textBoxFirstName = new TextBox();
            textBoxLastName = new TextBox();
            textBoxPESEL = new TextBox();
            textBoxEmail = new TextBox();
            buttonClearSearch = new Button();
            tabControlUserView = new TabControl();
            tabPageAdminViewUsers = new TabPage();
            groupBoxRoles = new GroupBox();
            flowLayoutPanelUprawnienia = new FlowLayoutPanel();
            tabPageAdminViewForgotten = new TabPage();
            groupBoxForgotten = new GroupBox();
            dgvForgotten = new DataGridView();
            tableLayoutPanelForgotten = new TableLayoutPanel();
            tabPageAbout = new TabPage();
            UserList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            tableLayoutPanelUsers.SuspendLayout();
            tabControlUserView.SuspendLayout();
            tabPageAdminViewUsers.SuspendLayout();
            groupBoxRoles.SuspendLayout();
            tabPageAdminViewForgotten.SuspendLayout();
            groupBoxForgotten.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvForgotten).BeginInit();
            SuspendLayout();
            // 
            // UserList
            // 
            UserList.Controls.Add(dgvUsers);
            UserList.Location = new Point(6, 206);
            UserList.Name = "UserList";
            UserList.Size = new Size(1671, 699);
            UserList.TabIndex = 0;
            UserList.TabStop = false;
            UserList.Text = "Lista Użytkowników";
            // 
            // dgvUsers
            // 
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsers.Dock = DockStyle.Fill;
            dgvUsers.Location = new Point(3, 27);
            dgvUsers.Name = "dgvUsers";
            dgvUsers.ReadOnly = true;
            dgvUsers.RowHeadersWidth = 62;
            dgvUsers.Size = new Size(1665, 669);
            dgvUsers.TabIndex = 0;
            dgvUsers.CellDoubleClick += dgvUsers_CellDoubleClick;
            // 
            // buttonAddUser
            // 
            buttonAddUser.Location = new Point(6, 6);
            buttonAddUser.Name = "buttonAddUser";
            buttonAddUser.Size = new Size(149, 91);
            buttonAddUser.TabIndex = 1;
            buttonAddUser.Text = "Dodaj Użytkownika";
            buttonAddUser.UseVisualStyleBackColor = true;
            buttonAddUser.Click += buttonAddUser_Click;
            // 
            // buttonSearchUser
            // 
            buttonSearchUser.Location = new Point(1531, 54);
            buttonSearchUser.Name = "buttonSearchUser";
            buttonSearchUser.Size = new Size(146, 43);
            buttonSearchUser.TabIndex = 2;
            buttonSearchUser.Text = "Wyszukaj";
            buttonSearchUser.UseVisualStyleBackColor = true;
            buttonSearchUser.Click += buttonSearchUser_Click;
            // 
            // tableLayoutPanelUsers
            // 
            tableLayoutPanelUsers.ColumnCount = 5;
            tableLayoutPanelUsers.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanelUsers.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanelUsers.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanelUsers.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanelUsers.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanelUsers.Controls.Add(labelLogin, 0, 0);
            tableLayoutPanelUsers.Controls.Add(labelFirstName, 1, 0);
            tableLayoutPanelUsers.Controls.Add(labelLastName, 2, 0);
            tableLayoutPanelUsers.Controls.Add(labelPESEL, 3, 0);
            tableLayoutPanelUsers.Controls.Add(labelEmail, 4, 0);
            tableLayoutPanelUsers.Controls.Add(textBoxLogin, 0, 1);
            tableLayoutPanelUsers.Controls.Add(textBoxFirstName, 1, 1);
            tableLayoutPanelUsers.Controls.Add(textBoxLastName, 2, 1);
            tableLayoutPanelUsers.Controls.Add(textBoxPESEL, 3, 1);
            tableLayoutPanelUsers.Controls.Add(textBoxEmail, 4, 1);
            tableLayoutPanelUsers.Location = new Point(166, 6);
            tableLayoutPanelUsers.Name = "tableLayoutPanelUsers";
            tableLayoutPanelUsers.RowCount = 2;
            tableLayoutPanelUsers.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanelUsers.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanelUsers.Size = new Size(1359, 91);
            tableLayoutPanelUsers.TabIndex = 3;
            // 
            // labelLogin
            // 
            labelLogin.AutoSize = true;
            labelLogin.Location = new Point(3, 0);
            labelLogin.Name = "labelLogin";
            labelLogin.Size = new Size(56, 25);
            labelLogin.TabIndex = 0;
            labelLogin.Text = "Login";
            // 
            // labelFirstName
            // 
            labelFirstName.AutoSize = true;
            labelFirstName.Location = new Point(274, 0);
            labelFirstName.Name = "labelFirstName";
            labelFirstName.Size = new Size(46, 25);
            labelFirstName.TabIndex = 1;
            labelFirstName.Text = "Imię";
            // 
            // labelLastName
            // 
            labelLastName.AutoSize = true;
            labelLastName.Location = new Point(545, 0);
            labelLastName.Name = "labelLastName";
            labelLastName.Size = new Size(87, 25);
            labelLastName.TabIndex = 2;
            labelLastName.Text = "Nazwisko";
            // 
            // labelPESEL
            // 
            labelPESEL.AutoSize = true;
            labelPESEL.Location = new Point(816, 0);
            labelPESEL.Name = "labelPESEL";
            labelPESEL.Size = new Size(58, 25);
            labelPESEL.TabIndex = 3;
            labelPESEL.Text = "PESEL";
            // 
            // labelEmail
            // 
            labelEmail.AutoSize = true;
            labelEmail.Location = new Point(1087, 0);
            labelEmail.Name = "labelEmail";
            labelEmail.Size = new Size(54, 25);
            labelEmail.TabIndex = 4;
            labelEmail.Text = "Email";
            // 
            // textBoxLogin
            // 
            textBoxLogin.Location = new Point(3, 48);
            textBoxLogin.Name = "textBoxLogin";
            textBoxLogin.Size = new Size(265, 31);
            textBoxLogin.TabIndex = 5;
            // 
            // textBoxFirstName
            // 
            textBoxFirstName.Location = new Point(274, 48);
            textBoxFirstName.Name = "textBoxFirstName";
            textBoxFirstName.Size = new Size(265, 31);
            textBoxFirstName.TabIndex = 6;
            // 
            // textBoxLastName
            // 
            textBoxLastName.Location = new Point(545, 48);
            textBoxLastName.Name = "textBoxLastName";
            textBoxLastName.Size = new Size(265, 31);
            textBoxLastName.TabIndex = 7;
            // 
            // textBoxPESEL
            // 
            textBoxPESEL.Location = new Point(816, 48);
            textBoxPESEL.Name = "textBoxPESEL";
            textBoxPESEL.Size = new Size(265, 31);
            textBoxPESEL.TabIndex = 8;
            // 
            // textBoxEmail
            // 
            textBoxEmail.Location = new Point(1087, 48);
            textBoxEmail.Name = "textBoxEmail";
            textBoxEmail.Size = new Size(269, 31);
            textBoxEmail.TabIndex = 9;
            // 
            // buttonClearSearch
            // 
            buttonClearSearch.Location = new Point(1531, 5);
            buttonClearSearch.Name = "buttonClearSearch";
            buttonClearSearch.Size = new Size(146, 43);
            buttonClearSearch.TabIndex = 4;
            buttonClearSearch.Text = "Wyczyść";
            buttonClearSearch.UseVisualStyleBackColor = true;
            buttonClearSearch.Click += buttonClearSearch_Click;
            // 
            // tabControlUserView
            // 
            tabControlUserView.Controls.Add(tabPageAdminViewUsers);
            tabControlUserView.Controls.Add(tabPageAdminViewForgotten);
            tabControlUserView.Controls.Add(tabPageAbout);
            tabControlUserView.Location = new Point(12, 12);
            tabControlUserView.Name = "tabControlUserView";
            tabControlUserView.SelectedIndex = 0;
            tabControlUserView.Size = new Size(1691, 956);
            tabControlUserView.TabIndex = 5;
            // 
            // tabPageAdminViewUsers
            // 
            tabPageAdminViewUsers.Controls.Add(groupBoxRoles);
            tabPageAdminViewUsers.Controls.Add(buttonAddUser);
            tabPageAdminViewUsers.Controls.Add(UserList);
            tabPageAdminViewUsers.Controls.Add(buttonClearSearch);
            tabPageAdminViewUsers.Controls.Add(buttonSearchUser);
            tabPageAdminViewUsers.Controls.Add(tableLayoutPanelUsers);
            tabPageAdminViewUsers.Location = new Point(4, 34);
            tabPageAdminViewUsers.Name = "tabPageAdminViewUsers";
            tabPageAdminViewUsers.Padding = new Padding(3);
            tabPageAdminViewUsers.Size = new Size(1683, 918);
            tabPageAdminViewUsers.TabIndex = 0;
            tabPageAdminViewUsers.Text = "Użytkownicy";
            tabPageAdminViewUsers.UseVisualStyleBackColor = true;
            // 
            // groupBoxRoles
            // 
            groupBoxRoles.Controls.Add(flowLayoutPanelUprawnienia);
            groupBoxRoles.Location = new Point(9, 103);
            groupBoxRoles.Name = "groupBoxRoles";
            groupBoxRoles.Size = new Size(1665, 102);
            groupBoxRoles.TabIndex = 5;
            groupBoxRoles.TabStop = false;
            groupBoxRoles.Text = "Uprawnienia";
            // 
            // flowLayoutPanelUprawnienia
            // 
            flowLayoutPanelUprawnienia.Location = new Point(10, 28);
            flowLayoutPanelUprawnienia.Name = "flowLayoutPanelUprawnienia";
            flowLayoutPanelUprawnienia.Size = new Size(1655, 68);
            flowLayoutPanelUprawnienia.TabIndex = 0;
            // 
            // tabPageAdminViewForgotten
            // 
            tabPageAdminViewForgotten.Controls.Add(groupBoxForgotten);
            tabPageAdminViewForgotten.Controls.Add(tableLayoutPanelForgotten);
            tabPageAdminViewForgotten.Location = new Point(4, 34);
            tabPageAdminViewForgotten.Name = "tabPageAdminViewForgotten";
            tabPageAdminViewForgotten.Size = new Size(1683, 918);
            tabPageAdminViewForgotten.TabIndex = 2;
            tabPageAdminViewForgotten.Text = "Zapomnieni";
            tabPageAdminViewForgotten.UseVisualStyleBackColor = true;
            // 
            // groupBoxForgotten
            // 
            groupBoxForgotten.Controls.Add(dgvForgotten);
            groupBoxForgotten.Location = new Point(3, 156);
            groupBoxForgotten.Name = "groupBoxForgotten";
            groupBoxForgotten.Size = new Size(1677, 759);
            groupBoxForgotten.TabIndex = 1;
            groupBoxForgotten.TabStop = false;
            groupBoxForgotten.Text = "Zapomnieni Użytkownicy";
            // 
            // dgvForgotten
            // 
            dgvForgotten.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvForgotten.Location = new Point(6, 30);
            dgvForgotten.Name = "dgvForgotten";
            dgvForgotten.RowHeadersWidth = 62;
            dgvForgotten.Size = new Size(1665, 723);
            dgvForgotten.TabIndex = 0;
            // 
            // tableLayoutPanelForgotten
            // 
            tableLayoutPanelForgotten.ColumnCount = 2;
            tableLayoutPanelForgotten.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanelForgotten.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanelForgotten.Location = new Point(3, 3);
            tableLayoutPanelForgotten.Name = "tableLayoutPanelForgotten";
            tableLayoutPanelForgotten.RowCount = 2;
            tableLayoutPanelForgotten.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanelForgotten.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanelForgotten.Size = new Size(1677, 150);
            tableLayoutPanelForgotten.TabIndex = 0;
            // 
            // tabPageAbout
            // 
            tabPageAbout.Location = new Point(4, 34);
            tabPageAbout.Name = "tabPageAbout";
            tabPageAbout.Padding = new Padding(3);
            tabPageAbout.Size = new Size(1683, 918);
            tabPageAbout.TabIndex = 1;
            tabPageAbout.Text = "O Programie";
            tabPageAbout.UseVisualStyleBackColor = true;
            // 
            // FormUserView
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1711, 980);
            Controls.Add(tabControlUserView);
            Name = "FormUserView";
            Text = "System Przychodnia";
            Activated += FormAdminView_Activated;
            Load += FormAdminView_Load;
            UserList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            tableLayoutPanelUsers.ResumeLayout(false);
            tableLayoutPanelUsers.PerformLayout();
            tabControlUserView.ResumeLayout(false);
            tabPageAdminViewUsers.ResumeLayout(false);
            groupBoxRoles.ResumeLayout(false);
            tabPageAdminViewForgotten.ResumeLayout(false);
            groupBoxForgotten.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvForgotten).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox UserList;
        private DataGridView dgvUsers;
        private Button buttonAddUser;
        private Button buttonSearchUser;
        private TableLayoutPanel tableLayoutPanelUsers;
        private Label labelLogin;
        private Label labelFirstName;
        private Label labelLastName;
        private Label labelPESEL;
        private Label labelEmail;
        private TextBox textBoxLogin;
        private TextBox textBoxFirstName;
        private TextBox textBoxLastName;
        private TextBox textBoxPESEL;
        private TextBox textBoxEmail;
        private Button buttonClearSearch;
        private TabControl tabControlUserView;
        private TabPage tabPageAdminViewUsers;
        private TabPage tabPageAbout;
        private TabPage tabPageAdminViewForgotten;
        private GroupBox groupBoxRoles;
        private FlowLayoutPanel flowLayoutPanelUprawnienia;
        private TableLayoutPanel tableLayoutPanelForgotten;
        private GroupBox groupBoxForgotten;
        private DataGridView dgvForgotten;
    }
}
