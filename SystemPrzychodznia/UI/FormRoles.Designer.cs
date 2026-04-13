namespace SystemPrzychodznia
{
    partial class FormRoles
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            splitContainer = new SplitContainer();
            labelRolesHeader = new Label();
            listBoxRoles = new ListBox();
            panelRight = new Panel();
            labelUserCount = new Label();
            dgvRoleUsers = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            panelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRoleUsers).BeginInit();
            SuspendLayout();
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(0, 0);
            splitContainer.Name = "splitContainer";
            splitContainer.Size = new Size(1000, 560);
            splitContainer.SplitterDistance = 200;
            splitContainer.TabIndex = 0;
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(listBoxRoles);
            splitContainer.Panel1.Controls.Add(labelRolesHeader);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(panelRight);
            // 
            // labelRolesHeader
            // 
            labelRolesHeader.Dock = DockStyle.Top;
            labelRolesHeader.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            labelRolesHeader.Location = new Point(0, 0);
            labelRolesHeader.Name = "labelRolesHeader";
            labelRolesHeader.Size = new Size(200, 32);
            labelRolesHeader.TabIndex = 0;
            labelRolesHeader.Text = "Role w systemie";
            labelRolesHeader.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // listBoxRoles
            // 
            listBoxRoles.Dock = DockStyle.Fill;
            listBoxRoles.Font = new Font("Segoe UI", 10F);
            listBoxRoles.FormattingEnabled = true;
            listBoxRoles.ItemHeight = 28;
            listBoxRoles.Location = new Point(0, 32);
            listBoxRoles.Name = "listBoxRoles";
            listBoxRoles.Size = new Size(200, 528);
            listBoxRoles.TabIndex = 1;
            listBoxRoles.SelectedIndexChanged += listBoxRoles_SelectedIndexChanged;
            // 
            // panelRight
            // 
            panelRight.Controls.Add(dgvRoleUsers);
            panelRight.Controls.Add(labelUserCount);
            panelRight.Dock = DockStyle.Fill;
            panelRight.Location = new Point(0, 0);
            panelRight.Name = "panelRight";
            panelRight.Size = new Size(796, 560);
            panelRight.TabIndex = 0;
            // 
            // labelUserCount
            // 
            labelUserCount.Dock = DockStyle.Top;
            labelUserCount.Font = new Font("Segoe UI", 9F);
            labelUserCount.Location = new Point(0, 0);
            labelUserCount.Name = "labelUserCount";
            labelUserCount.Size = new Size(796, 28);
            labelUserCount.TabIndex = 0;
            labelUserCount.Text = "Wybierz rolę z listy";
            labelUserCount.TextAlign = ContentAlignment.MiddleLeft;
            labelUserCount.Padding = new Padding(6, 0, 0, 0);
            // 
            // dgvRoleUsers
            // 
            dgvRoleUsers.AllowUserToAddRows = false;
            dgvRoleUsers.AllowUserToDeleteRows = false;
            dgvRoleUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRoleUsers.Dock = DockStyle.Fill;
            dgvRoleUsers.Location = new Point(0, 28);
            dgvRoleUsers.Name = "dgvRoleUsers";
            dgvRoleUsers.ReadOnly = true;
            dgvRoleUsers.RowHeadersWidth = 62;
            dgvRoleUsers.Size = new Size(796, 532);
            dgvRoleUsers.TabIndex = 1;
            // 
            // FormRoles
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 560);
            Controls.Add(splitContainer);
            MinimumSize = new Size(700, 400);
            Name = "FormRoles";
            Text = "Przegląd ról";
            Load += FormRoles_Load;
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            splitContainer.ResumeLayout(false);
            panelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvRoleUsers).EndInit();
            ResumeLayout(false);
        }

        private SplitContainer splitContainer;
        private Label labelRolesHeader;
        private ListBox listBoxRoles;
        private Panel panelRight;
        private Label labelUserCount;
        private DataGridView dgvRoleUsers;
    }
}
