namespace SystemPrzychodznia
{
    partial class FormAdminView
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
            tableLayoutPanel1 = new TableLayoutPanel();
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
            btnLoadForgotten = new Button();
            btnForget = new Button();
            comboBoxRoleSearches = new ComboBox();
            label1 = new Label();
            UserList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // UserList
            // 
            UserList.Controls.Add(dgvUsers);
            UserList.Location = new Point(8, 80);
            UserList.Margin = new Padding(2);
            UserList.Name = "UserList";
            UserList.Padding = new Padding(2);
            UserList.Size = new Size(1186, 272);
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
            dgvUsers.Location = new Point(2, 18);
            dgvUsers.Margin = new Padding(2);
            dgvUsers.Name = "dgvUsers";
            dgvUsers.ReadOnly = true;
            dgvUsers.RowHeadersWidth = 62;
            dgvUsers.Size = new Size(1182, 252);
            dgvUsers.TabIndex = 0;
            dgvUsers.CellDoubleClick += dgvUsers_CellDoubleClick;
            // 
            // buttonAddUser
            // 
            buttonAddUser.Location = new Point(13, 9);
            buttonAddUser.Margin = new Padding(2);
            buttonAddUser.Name = "buttonAddUser";
            buttonAddUser.Size = new Size(104, 55);
            buttonAddUser.TabIndex = 1;
            buttonAddUser.Text = "Dodaj Użytkownika";
            buttonAddUser.UseVisualStyleBackColor = true;
            buttonAddUser.Click += buttonAddUser_Click;
            // 
            // buttonSearchUser
            // 
            buttonSearchUser.Location = new Point(1080, 38);
            buttonSearchUser.Margin = new Padding(2);
            buttonSearchUser.Name = "buttonSearchUser";
            buttonSearchUser.Size = new Size(114, 26);
            buttonSearchUser.TabIndex = 2;
            buttonSearchUser.Text = "Wyszukaj";
            buttonSearchUser.UseVisualStyleBackColor = true;
            buttonSearchUser.Click += buttonSearchUser_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Controls.Add(labelLogin, 0, 0);
            tableLayoutPanel1.Controls.Add(labelFirstName, 1, 0);
            tableLayoutPanel1.Controls.Add(labelLastName, 2, 0);
            tableLayoutPanel1.Controls.Add(labelPESEL, 3, 0);
            tableLayoutPanel1.Controls.Add(labelEmail, 4, 0);
            tableLayoutPanel1.Controls.Add(textBoxLogin, 0, 1);
            tableLayoutPanel1.Controls.Add(textBoxFirstName, 1, 1);
            tableLayoutPanel1.Controls.Add(textBoxLastName, 2, 1);
            tableLayoutPanel1.Controls.Add(textBoxPESEL, 3, 1);
            tableLayoutPanel1.Controls.Add(textBoxEmail, 4, 1);
            tableLayoutPanel1.Location = new Point(125, 9);
            tableLayoutPanel1.Margin = new Padding(2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(951, 55);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // labelLogin
            // 
            labelLogin.AutoSize = true;
            labelLogin.Location = new Point(2, 0);
            labelLogin.Margin = new Padding(2, 0, 2, 0);
            labelLogin.Name = "labelLogin";
            labelLogin.Size = new Size(37, 15);
            labelLogin.TabIndex = 0;
            labelLogin.Text = "Login";
            // 
            // labelFirstName
            // 
            labelFirstName.AutoSize = true;
            labelFirstName.Location = new Point(192, 0);
            labelFirstName.Margin = new Padding(2, 0, 2, 0);
            labelFirstName.Name = "labelFirstName";
            labelFirstName.Size = new Size(30, 15);
            labelFirstName.TabIndex = 1;
            labelFirstName.Text = "Imię";
            // 
            // labelLastName
            // 
            labelLastName.AutoSize = true;
            labelLastName.Location = new Point(382, 0);
            labelLastName.Margin = new Padding(2, 0, 2, 0);
            labelLastName.Name = "labelLastName";
            labelLastName.Size = new Size(57, 15);
            labelLastName.TabIndex = 2;
            labelLastName.Text = "Nazwisko";
            // 
            // labelPESEL
            // 
            labelPESEL.AutoSize = true;
            labelPESEL.Location = new Point(572, 0);
            labelPESEL.Margin = new Padding(2, 0, 2, 0);
            labelPESEL.Name = "labelPESEL";
            labelPESEL.Size = new Size(38, 15);
            labelPESEL.TabIndex = 3;
            labelPESEL.Text = "PESEL";
            // 
            // labelEmail
            // 
            labelEmail.AutoSize = true;
            labelEmail.Location = new Point(762, 0);
            labelEmail.Margin = new Padding(2, 0, 2, 0);
            labelEmail.Name = "labelEmail";
            labelEmail.Size = new Size(36, 15);
            labelEmail.TabIndex = 4;
            labelEmail.Text = "Email";
            // 
            // textBoxLogin
            // 
            textBoxLogin.Location = new Point(2, 29);
            textBoxLogin.Margin = new Padding(2);
            textBoxLogin.Name = "textBoxLogin";
            textBoxLogin.Size = new Size(186, 23);
            textBoxLogin.TabIndex = 5;
            // 
            // textBoxFirstName
            // 
            textBoxFirstName.Location = new Point(192, 29);
            textBoxFirstName.Margin = new Padding(2);
            textBoxFirstName.Name = "textBoxFirstName";
            textBoxFirstName.Size = new Size(186, 23);
            textBoxFirstName.TabIndex = 6;
            // 
            // textBoxLastName
            // 
            textBoxLastName.Location = new Point(382, 29);
            textBoxLastName.Margin = new Padding(2);
            textBoxLastName.Name = "textBoxLastName";
            textBoxLastName.Size = new Size(186, 23);
            textBoxLastName.TabIndex = 7;
            // 
            // textBoxPESEL
            // 
            textBoxPESEL.Location = new Point(572, 29);
            textBoxPESEL.Margin = new Padding(2);
            textBoxPESEL.Name = "textBoxPESEL";
            textBoxPESEL.Size = new Size(186, 23);
            textBoxPESEL.TabIndex = 8;
            // 
            // textBoxEmail
            // 
            textBoxEmail.Location = new Point(762, 29);
            textBoxEmail.Margin = new Padding(2);
            textBoxEmail.Name = "textBoxEmail";
            textBoxEmail.Size = new Size(187, 23);
            textBoxEmail.TabIndex = 9;
            // 
            // buttonClearSearch
            // 
            buttonClearSearch.Location = new Point(1080, 8);
            buttonClearSearch.Margin = new Padding(2);
            buttonClearSearch.Name = "buttonClearSearch";
            buttonClearSearch.Size = new Size(112, 26);
            buttonClearSearch.TabIndex = 4;
            buttonClearSearch.Text = "Wyczyść";
            buttonClearSearch.UseVisualStyleBackColor = true;
            buttonClearSearch.Click += buttonClearSearch_Click;
            // 
            // btnLoadForgotten
            // 
            btnLoadForgotten.Location = new Point(351, 63);
            btnLoadForgotten.Name = "btnLoadForgotten";
            btnLoadForgotten.Size = new Size(186, 23);
            btnLoadForgotten.TabIndex = 5;
            btnLoadForgotten.Text = "Wyszukaj zapomnianych";
            btnLoadForgotten.UseVisualStyleBackColor = true;
            btnLoadForgotten.Click += btnLoadForgotten_Click;
            // 
            // btnForget
            // 
            btnForget.Location = new Point(543, 63);
            btnForget.Name = "btnForget";
            btnForget.Size = new Size(186, 23);
            btnForget.TabIndex = 6;
            btnForget.Text = "Zapomnij uzytkownika";
            btnForget.UseVisualStyleBackColor = true;
            btnForget.Click += btnForget_Click;
            // 
            // comboBoxRoleSearches
            // 
            comboBoxRoleSearches.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxRoleSearches.FormattingEnabled = true;
            comboBoxRoleSearches.Items.AddRange(new object[] { "Administrator", "Recepcja", "Lekarz", "Pacjent" });
            comboBoxRoleSearches.Location = new Point(224, 63);
            comboBoxRoleSearches.Name = "comboBoxRoleSearches";
            comboBoxRoleSearches.Size = new Size(121, 23);
            comboBoxRoleSearches.TabIndex = 9;
            comboBoxRoleSearches.SelectedIndexChanged += comboBoxRoleSearches_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(125, 63);
            label1.Name = "label1";
            label1.Size = new Size(93, 15);
            label1.TabIndex = 10;
            label1.Text = "Wyszukaj po roli";
            label1.Click += label1_Click_1;
            // 
            // FormAdminView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1203, 359);
            Controls.Add(label1);
            Controls.Add(comboBoxRoleSearches);
            Controls.Add(btnForget);
            Controls.Add(btnLoadForgotten);
            Controls.Add(buttonClearSearch);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(buttonSearchUser);
            Controls.Add(buttonAddUser);
            Controls.Add(UserList);
            Margin = new Padding(2);
            Name = "FormAdminView";
            Text = "System Przychodnia";
            Activated += FormAdminView_Activated;
            Load += FormAdminView_Load;
            UserList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox UserList;
        private DataGridView dgvUsers;
        private Button buttonAddUser;
        private Button buttonSearchUser;
        private TableLayoutPanel tableLayoutPanel1;
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
        private Button btnLoadForgotten;
        private Button btnForget;
        private ComboBox comboBoxRoleSearches;
        private Label label1;
    }
}
