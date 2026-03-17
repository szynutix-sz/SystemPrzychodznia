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
            UserList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // UserList
            // 
            UserList.Controls.Add(dgvUsers);
            UserList.Location = new Point(12, 104);
            UserList.Name = "UserList";
            UserList.Size = new Size(1694, 483);
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
            dgvUsers.Size = new Size(1688, 453);
            dgvUsers.TabIndex = 0;
            // 
            // buttonAddUser
            // 
            buttonAddUser.Location = new Point(18, 15);
            buttonAddUser.Name = "buttonAddUser";
            buttonAddUser.Size = new Size(149, 91);
            buttonAddUser.TabIndex = 1;
            buttonAddUser.Text = "Dodaj Użytkownika";
            buttonAddUser.UseVisualStyleBackColor = true;
            buttonAddUser.Click += buttonAddUser_Click;
            // 
            // buttonSearchUser
            // 
            buttonSearchUser.Location = new Point(1543, 63);
            buttonSearchUser.Name = "buttonSearchUser";
            buttonSearchUser.Size = new Size(163, 43);
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
            tableLayoutPanel1.Location = new Point(178, 15);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(1359, 91);
            tableLayoutPanel1.TabIndex = 3;
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
            buttonClearSearch.Location = new Point(1543, 14);
            buttonClearSearch.Name = "buttonClearSearch";
            buttonClearSearch.Size = new Size(160, 43);
            buttonClearSearch.TabIndex = 4;
            buttonClearSearch.Text = "Wyczyść";
            buttonClearSearch.UseVisualStyleBackColor = true;
            buttonClearSearch.Click += buttonClearSearch_Click;
            // 
            // FormAdminView
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1718, 599);
            Controls.Add(buttonClearSearch);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(buttonSearchUser);
            Controls.Add(buttonAddUser);
            Controls.Add(UserList);
            Name = "FormAdminView";
            Text = "System Przychodnia";
            Activated += FormAdminView_Activated;
            Load += FormAdminView_Load;
            UserList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
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
    }
}
