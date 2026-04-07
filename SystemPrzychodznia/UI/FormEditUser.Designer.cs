namespace SystemPrzychodznia
{
    partial class FormEditUser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            buttonEditUser = new Button();
            buttonForgetUser = new Button();
            textBoxPhone = new TextBox();
            textBoxEmail = new TextBox();
            textBoxPESEL = new TextBox();
            textBoxHouseUnitNumber = new TextBox();
            textBoxPropertyNumber = new TextBox();
            textBoxStreet = new TextBox();
            textBoxPostCode = new TextBox();
            textBoxLocality = new TextBox();
            textBoxLastName = new TextBox();
            textBoxFirstName = new TextBox();
            textBoxLogin = new TextBox();
            comboBoxGender = new ComboBox();
            BirthDateTimePicker = new DateTimePicker();
            labelPhone = new Label();
            labelEmail = new Label();
            labelGender = new Label();
            labelBirthDate = new Label();
            labelPESEL = new Label();
            labelHouseUnitNumber = new Label();
            labelPropertyNumber = new Label();
            labelStreet = new Label();
            labelPostalCode = new Label();
            labelLocality = new Label();
            labelLastName = new Label();
            labelFirstName = new Label();
            labelLogin = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            buttonUnlockEditing = new Button();
            tabControlEditUser = new TabControl();
            tabPageUserData = new TabPage();
            tabPageUserUpra = new TabPage();
            checkedListBoxUprawnienia = new CheckedListBox();
            tableLayoutPanel1.SuspendLayout();
            tabControlEditUser.SuspendLayout();
            tabPageUserData.SuspendLayout();
            tabPageUserUpra.SuspendLayout();
            SuspendLayout();
            // 
            // buttonEditUser
            // 
            buttonEditUser.Location = new Point(397, 514);
            buttonEditUser.Name = "buttonEditUser";
            buttonEditUser.Size = new Size(381, 44);
            buttonEditUser.TabIndex = 1;
            buttonEditUser.Text = "Potwierdz Dane";
            buttonEditUser.UseVisualStyleBackColor = true;
            buttonEditUser.Click += buttonEditUser_Click;
            // 
            // buttonForgetUser
            // 
            buttonForgetUser.Location = new Point(16, 644);
            buttonForgetUser.Name = "buttonForgetUser";
            buttonForgetUser.Size = new Size(379, 44);
            buttonForgetUser.TabIndex = 2;
            buttonForgetUser.Text = "Zapomnij Użytkownika";
            buttonForgetUser.UseVisualStyleBackColor = true;
            buttonForgetUser.Click += buttonForgetUser_Click;
            // 
            // textBoxPhone
            // 
            textBoxPhone.Location = new Point(384, 423);
            textBoxPhone.Name = "textBoxPhone";
            textBoxPhone.Size = new Size(375, 31);
            textBoxPhone.TabIndex = 29;
            // 
            // textBoxEmail
            // 
            textBoxEmail.Location = new Point(384, 388);
            textBoxEmail.Name = "textBoxEmail";
            textBoxEmail.Size = new Size(375, 31);
            textBoxEmail.TabIndex = 28;
            // 
            // textBoxPESEL
            // 
            textBoxPESEL.Location = new Point(384, 283);
            textBoxPESEL.Name = "textBoxPESEL";
            textBoxPESEL.Size = new Size(375, 31);
            textBoxPESEL.TabIndex = 27;
            // 
            // textBoxHouseUnitNumber
            // 
            textBoxHouseUnitNumber.Location = new Point(384, 248);
            textBoxHouseUnitNumber.Name = "textBoxHouseUnitNumber";
            textBoxHouseUnitNumber.Size = new Size(375, 31);
            textBoxHouseUnitNumber.TabIndex = 26;
            // 
            // textBoxPropertyNumber
            // 
            textBoxPropertyNumber.Location = new Point(384, 213);
            textBoxPropertyNumber.Name = "textBoxPropertyNumber";
            textBoxPropertyNumber.Size = new Size(375, 31);
            textBoxPropertyNumber.TabIndex = 25;
            // 
            // textBoxStreet
            // 
            textBoxStreet.Location = new Point(384, 178);
            textBoxStreet.Name = "textBoxStreet";
            textBoxStreet.Size = new Size(375, 31);
            textBoxStreet.TabIndex = 24;
            // 
            // textBoxPostCode
            // 
            textBoxPostCode.Location = new Point(384, 143);
            textBoxPostCode.Name = "textBoxPostCode";
            textBoxPostCode.Size = new Size(375, 31);
            textBoxPostCode.TabIndex = 23;
            // 
            // textBoxLocality
            // 
            textBoxLocality.Location = new Point(384, 108);
            textBoxLocality.Name = "textBoxLocality";
            textBoxLocality.Size = new Size(375, 31);
            textBoxLocality.TabIndex = 22;
            // 
            // textBoxLastName
            // 
            textBoxLastName.Location = new Point(384, 73);
            textBoxLastName.Name = "textBoxLastName";
            textBoxLastName.Size = new Size(375, 31);
            textBoxLastName.TabIndex = 21;
            // 
            // textBoxFirstName
            // 
            textBoxFirstName.Location = new Point(384, 38);
            textBoxFirstName.Name = "textBoxFirstName";
            textBoxFirstName.Size = new Size(375, 31);
            textBoxFirstName.TabIndex = 20;
            // 
            // textBoxLogin
            // 
            textBoxLogin.Location = new Point(384, 3);
            textBoxLogin.Name = "textBoxLogin";
            textBoxLogin.Size = new Size(375, 31);
            textBoxLogin.TabIndex = 17;
            // 
            // comboBoxGender
            // 
            comboBoxGender.FormattingEnabled = true;
            comboBoxGender.Items.AddRange(new object[] { "Mężczyzna", "Kobieta" });
            comboBoxGender.Location = new Point(384, 353);
            comboBoxGender.Name = "comboBoxGender";
            comboBoxGender.Size = new Size(375, 33);
            comboBoxGender.TabIndex = 16;
            // 
            // BirthDateTimePicker
            // 
            BirthDateTimePicker.CustomFormat = "YYYY-MM-DD";
            BirthDateTimePicker.ImeMode = ImeMode.Off;
            BirthDateTimePicker.Location = new Point(384, 318);
            BirthDateTimePicker.Name = "BirthDateTimePicker";
            BirthDateTimePicker.Size = new Size(375, 31);
            BirthDateTimePicker.TabIndex = 15;
            // 
            // labelPhone
            // 
            labelPhone.AutoSize = true;
            labelPhone.Location = new Point(3, 420);
            labelPhone.Name = "labelPhone";
            labelPhone.Size = new Size(136, 25);
            labelPhone.TabIndex = 12;
            labelPhone.Text = "Numer telefonu";
            // 
            // labelEmail
            // 
            labelEmail.AutoSize = true;
            labelEmail.Location = new Point(3, 385);
            labelEmail.Name = "labelEmail";
            labelEmail.Size = new Size(112, 25);
            labelEmail.TabIndex = 11;
            labelEmail.Text = "Adres e-mail";
            // 
            // labelGender
            // 
            labelGender.AutoSize = true;
            labelGender.Location = new Point(3, 350);
            labelGender.Name = "labelGender";
            labelGender.Size = new Size(44, 25);
            labelGender.TabIndex = 10;
            labelGender.Text = "Płeć";
            // 
            // labelBirthDate
            // 
            labelBirthDate.AutoSize = true;
            labelBirthDate.Location = new Point(3, 315);
            labelBirthDate.Name = "labelBirthDate";
            labelBirthDate.Size = new Size(134, 25);
            labelBirthDate.TabIndex = 9;
            labelBirthDate.Text = "Data Urodzenia";
            // 
            // labelPESEL
            // 
            labelPESEL.AutoSize = true;
            labelPESEL.Location = new Point(3, 280);
            labelPESEL.Name = "labelPESEL";
            labelPESEL.Size = new Size(58, 25);
            labelPESEL.TabIndex = 8;
            labelPESEL.Text = "PESEL";
            // 
            // labelHouseUnitNumber
            // 
            labelHouseUnitNumber.AutoSize = true;
            labelHouseUnitNumber.Location = new Point(3, 245);
            labelHouseUnitNumber.Name = "labelHouseUnitNumber";
            labelHouseUnitNumber.Size = new Size(118, 25);
            labelHouseUnitNumber.TabIndex = 7;
            labelHouseUnitNumber.Text = "Numer lokalu";
            // 
            // labelPropertyNumber
            // 
            labelPropertyNumber.AutoSize = true;
            labelPropertyNumber.Location = new Point(3, 210);
            labelPropertyNumber.Name = "labelPropertyNumber";
            labelPropertyNumber.Size = new Size(124, 25);
            labelPropertyNumber.TabIndex = 6;
            labelPropertyNumber.Text = "Numer Posesji";
            // 
            // labelStreet
            // 
            labelStreet.AutoSize = true;
            labelStreet.Location = new Point(3, 175);
            labelStreet.Name = "labelStreet";
            labelStreet.Size = new Size(49, 25);
            labelStreet.TabIndex = 5;
            labelStreet.Text = "Ulica";
            // 
            // labelPostalCode
            // 
            labelPostalCode.AutoSize = true;
            labelPostalCode.Location = new Point(3, 140);
            labelPostalCode.Name = "labelPostalCode";
            labelPostalCode.Size = new Size(124, 25);
            labelPostalCode.TabIndex = 4;
            labelPostalCode.Text = "Kod Pocztowy";
            // 
            // labelLocality
            // 
            labelLocality.AutoSize = true;
            labelLocality.Location = new Point(3, 105);
            labelLocality.Name = "labelLocality";
            labelLocality.Size = new Size(112, 25);
            labelLocality.TabIndex = 3;
            labelLocality.Text = "Miejscowosc";
            // 
            // labelLastName
            // 
            labelLastName.AutoSize = true;
            labelLastName.Location = new Point(3, 70);
            labelLastName.Name = "labelLastName";
            labelLastName.Size = new Size(87, 25);
            labelLastName.TabIndex = 2;
            labelLastName.Text = "Nazwisko";
            // 
            // labelFirstName
            // 
            labelFirstName.AutoSize = true;
            labelFirstName.Location = new Point(3, 35);
            labelFirstName.Name = "labelFirstName";
            labelFirstName.Size = new Size(46, 25);
            labelFirstName.TabIndex = 1;
            labelFirstName.Text = "Imię";
            // 
            // labelLogin
            // 
            labelLogin.AutoSize = true;
            labelLogin.Location = new Point(3, 0);
            labelLogin.Name = "labelLogin";
            labelLogin.Size = new Size(56, 25);
            labelLogin.TabIndex = 0;
            labelLogin.Text = "Login";
            labelLogin.UseWaitCursor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(labelLogin, 0, 0);
            tableLayoutPanel1.Controls.Add(labelFirstName, 0, 1);
            tableLayoutPanel1.Controls.Add(labelLastName, 0, 2);
            tableLayoutPanel1.Controls.Add(labelLocality, 0, 3);
            tableLayoutPanel1.Controls.Add(labelPostalCode, 0, 4);
            tableLayoutPanel1.Controls.Add(labelStreet, 0, 5);
            tableLayoutPanel1.Controls.Add(labelPropertyNumber, 0, 6);
            tableLayoutPanel1.Controls.Add(labelHouseUnitNumber, 0, 7);
            tableLayoutPanel1.Controls.Add(labelPESEL, 0, 8);
            tableLayoutPanel1.Controls.Add(labelBirthDate, 0, 9);
            tableLayoutPanel1.Controls.Add(labelGender, 0, 10);
            tableLayoutPanel1.Controls.Add(labelEmail, 0, 11);
            tableLayoutPanel1.Controls.Add(labelPhone, 0, 12);
            tableLayoutPanel1.Controls.Add(BirthDateTimePicker, 1, 9);
            tableLayoutPanel1.Controls.Add(comboBoxGender, 1, 10);
            tableLayoutPanel1.Controls.Add(textBoxLogin, 1, 0);
            tableLayoutPanel1.Controls.Add(textBoxFirstName, 1, 1);
            tableLayoutPanel1.Controls.Add(textBoxLastName, 1, 2);
            tableLayoutPanel1.Controls.Add(textBoxLocality, 1, 3);
            tableLayoutPanel1.Controls.Add(textBoxPostCode, 1, 4);
            tableLayoutPanel1.Controls.Add(textBoxStreet, 1, 5);
            tableLayoutPanel1.Controls.Add(textBoxPropertyNumber, 1, 6);
            tableLayoutPanel1.Controls.Add(textBoxHouseUnitNumber, 1, 7);
            tableLayoutPanel1.Controls.Add(textBoxPESEL, 1, 8);
            tableLayoutPanel1.Controls.Add(textBoxEmail, 1, 11);
            tableLayoutPanel1.Controls.Add(textBoxPhone, 1, 12);
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 13;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69230843F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69230747F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69230747F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69230747F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69230747F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69230747F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69230747F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69230747F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69230747F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69230747F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69230747F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69230747F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69230747F));
            tableLayoutPanel1.Size = new Size(762, 459);
            tableLayoutPanel1.TabIndex = 0;
            tableLayoutPanel1.Paint += tableLayoutPanel1_Paint;
            // 
            // buttonUnlockEditing
            // 
            buttonUnlockEditing.Location = new Point(396, 563);
            buttonUnlockEditing.Name = "buttonUnlockEditing";
            buttonUnlockEditing.Size = new Size(382, 41);
            buttonUnlockEditing.TabIndex = 3;
            buttonUnlockEditing.Text = "Edytuj Użytkownika";
            buttonUnlockEditing.UseVisualStyleBackColor = true;
            buttonUnlockEditing.Click += buttonUnlockEditing_Click;
            // 
            // tabControlEditUser
            // 
            tabControlEditUser.Controls.Add(tabPageUserData);
            tabControlEditUser.Controls.Add(tabPageUserUpra);
            tabControlEditUser.Location = new Point(12, 12);
            tabControlEditUser.Name = "tabControlEditUser";
            tabControlEditUser.SelectedIndex = 0;
            tabControlEditUser.Size = new Size(786, 507);
            tabControlEditUser.TabIndex = 4;
            // 
            // tabPageUserData
            // 
            tabPageUserData.Controls.Add(tableLayoutPanel1);
            tabPageUserData.Location = new Point(4, 34);
            tabPageUserData.Name = "tabPageUserData";
            tabPageUserData.Padding = new Padding(3);
            tabPageUserData.Size = new Size(778, 469);
            tabPageUserData.TabIndex = 0;
            tabPageUserData.Text = "Dane";
            tabPageUserData.UseVisualStyleBackColor = true;
            // 
            // tabPageUserUpra
            // 
            tabPageUserUpra.Controls.Add(checkedListBoxUprawnienia);
            tabPageUserUpra.Location = new Point(4, 34);
            tabPageUserUpra.Name = "tabPageUserUpra";
            tabPageUserUpra.Padding = new Padding(3);
            tabPageUserUpra.Size = new Size(778, 469);
            tabPageUserUpra.TabIndex = 1;
            tabPageUserUpra.Text = "Uprawnienia";
            tabPageUserUpra.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxUprawnienia
            // 
            checkedListBoxUprawnienia.FormattingEnabled = true;
            checkedListBoxUprawnienia.Location = new Point(3, 0);
            checkedListBoxUprawnienia.Name = "checkedListBoxUprawnienia";
            checkedListBoxUprawnienia.Size = new Size(769, 452);
            checkedListBoxUprawnienia.TabIndex = 0;
            // 
            // FormEditUser
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(796, 700);
            Controls.Add(tabControlEditUser);
            Controls.Add(buttonUnlockEditing);
            Controls.Add(buttonForgetUser);
            Controls.Add(buttonEditUser);
            Name = "FormEditUser";
            Text = "Edytuj Użytkownika";
            Load += FormEditUser_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tabControlEditUser.ResumeLayout(false);
            tabPageUserData.ResumeLayout(false);
            tabPageUserUpra.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Button buttonEditUser;
        private Button buttonForgetUser;
        private TextBox textBoxPhone;
        private TextBox textBoxEmail;
        private TextBox textBoxPESEL;
        private TextBox textBoxHouseUnitNumber;
        private TextBox textBoxPropertyNumber;
        private TextBox textBoxStreet;
        private TextBox textBoxPostCode;
        private TextBox textBoxLocality;
        private TextBox textBoxLastName;
        private TextBox textBoxFirstName;
        private TextBox textBoxLogin;
        private ComboBox comboBoxGender;
        private DateTimePicker BirthDateTimePicker;
        private Label labelPhone;
        private Label labelEmail;
        private Label labelGender;
        private Label labelBirthDate;
        private Label labelPESEL;
        private Label labelHouseUnitNumber;
        private Label labelPropertyNumber;
        private Label labelStreet;
        private Label labelPostalCode;
        private Label labelLocality;
        private Label labelLastName;
        private Label labelFirstName;
        private Label labelLogin;
        private TableLayoutPanel tableLayoutPanel1;
        private Button buttonUnlockEditing;
        private TabControl tabControlEditUser;
        private TabPage tabPageUserData;
        private TabPage tabPageUserUpra;
        private CheckedListBox checkedListBoxUprawnienia;
    }
}