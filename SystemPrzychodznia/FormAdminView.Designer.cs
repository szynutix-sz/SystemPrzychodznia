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
            UserList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
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
            buttonSearchUser.Location = new Point(173, 15);
            buttonSearchUser.Name = "buttonSearchUser";
            buttonSearchUser.Size = new Size(163, 91);
            buttonSearchUser.TabIndex = 2;
            buttonSearchUser.Text = "Wyszukaj Użytkownika";
            buttonSearchUser.UseVisualStyleBackColor = true;
            buttonSearchUser.Click += buttonSearchUser_Click;
            // 
            // FormAdminView
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1718, 599);
            Controls.Add(buttonSearchUser);
            Controls.Add(buttonAddUser);
            Controls.Add(UserList);
            Name = "FormAdminView";
            Text = "System Przychodnia";
            Activated += FormAdminView_Activated;
            Load += FormAdminView_Load;
            UserList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox UserList;
        private DataGridView dgvUsers;
        private Button buttonAddUser;
        private Button buttonSearchUser;
    }
}
