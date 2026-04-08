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
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // buttonEditUser
            // 
            buttonEditUser.Location = new Point(278, 379);
            buttonEditUser.Margin = new Padding(2, 2, 2, 2);
            buttonEditUser.Name = "buttonEditUser";
            buttonEditUser.Size = new Size(262, 26);
            buttonEditUser.TabIndex = 1;
            buttonEditUser.Text = "Potwierdz Dane";
            buttonEditUser.UseVisualStyleBackColor = true;
            buttonEditUser.Click += buttonEditUser_Click;
            // 
            // buttonForgetUser
            // 
            buttonForgetUser.Location = new Point(8, 379);
            buttonForgetUser.Margin = new Padding(2, 2, 2, 2);
            buttonForgetUser.Name = "buttonForgetUser";
            buttonForgetUser.Size = new Size(265, 26);
            buttonForgetUser.TabIndex = 2;
            buttonForgetUser.Text = "Zapomnij Użytkownika";
            buttonForgetUser.UseVisualStyleBackColor = true;
            buttonForgetUser.Click += buttonForgetUser_Click;
            // 
            // textBoxPhone
            // 
            textBoxPhone.Location = new Point(268, 326);
            textBoxPhone.Margin = new Padding(2, 2, 2, 2);
            textBoxPhone.Name = "textBoxPhone";
            textBoxPhone.Size = new Size(263, 23);
            textBoxPhone.TabIndex = 29;
            // 
            // textBoxEmail
            // 
            textBoxEmail.Location = new Point(268, 299);
            textBoxEmail.Margin = new Padding(2, 2, 2, 2);
            textBoxEmail.Name = "textBoxEmail";
            textBoxEmail.Size = new Size(263, 23);
            textBoxEmail.TabIndex = 28;
            // 
            // textBoxPESEL
            // 
            textBoxPESEL.Location = new Point(268, 218);
            textBoxPESEL.Margin = new Padding(2, 2, 2, 2);
            textBoxPESEL.Name = "textBoxPESEL";
            textBoxPESEL.Size = new Size(263, 23);
            textBoxPESEL.TabIndex = 27;
            // 
            // textBoxHouseUnitNumber
            // 
            textBoxHouseUnitNumber.Location = new Point(268, 191);
            textBoxHouseUnitNumber.Margin = new Padding(2, 2, 2, 2);
            textBoxHouseUnitNumber.Name = "textBoxHouseUnitNumber";
            textBoxHouseUnitNumber.Size = new Size(263, 23);
            textBoxHouseUnitNumber.TabIndex = 26;
            // 
            // textBoxPropertyNumber
            // 
            textBoxPropertyNumber.Location = new Point(268, 164);
            textBoxPropertyNumber.Margin = new Padding(2, 2, 2, 2);
            textBoxPropertyNumber.Name = "textBoxPropertyNumber";
            textBoxPropertyNumber.Size = new Size(263, 23);
            textBoxPropertyNumber.TabIndex = 25;
            // 
            // textBoxStreet
            // 
            textBoxStreet.Location = new Point(268, 137);
            textBoxStreet.Margin = new Padding(2, 2, 2, 2);
            textBoxStreet.Name = "textBoxStreet";
            textBoxStreet.Size = new Size(263, 23);
            textBoxStreet.TabIndex = 24;
            // 
            // textBoxPostCode
            // 
            textBoxPostCode.Location = new Point(268, 110);
            textBoxPostCode.Margin = new Padding(2, 2, 2, 2);
            textBoxPostCode.Name = "textBoxPostCode";
            textBoxPostCode.Size = new Size(263, 23);
            textBoxPostCode.TabIndex = 23;
            // 
            // textBoxLocality
            // 
            textBoxLocality.Location = new Point(268, 83);
            textBoxLocality.Margin = new Padding(2, 2, 2, 2);
            textBoxLocality.Name = "textBoxLocality";
            textBoxLocality.Size = new Size(263, 23);
            textBoxLocality.TabIndex = 22;
            // 
            // textBoxLastName
            // 
            textBoxLastName.Location = new Point(268, 56);
            textBoxLastName.Margin = new Padding(2, 2, 2, 2);
            textBoxLastName.Name = "textBoxLastName";
            textBoxLastName.Size = new Size(263, 23);
            textBoxLastName.TabIndex = 21;
            // 
            // textBoxFirstName
            // 
            textBoxFirstName.Location = new Point(268, 29);
            textBoxFirstName.Margin = new Padding(2, 2, 2, 2);
            textBoxFirstName.Name = "textBoxFirstName";
            textBoxFirstName.Size = new Size(263, 23);
            textBoxFirstName.TabIndex = 20;
            // 
            // textBoxLogin
            // 
            textBoxLogin.Location = new Point(268, 2);
            textBoxLogin.Margin = new Padding(2, 2, 2, 2);
            textBoxLogin.Name = "textBoxLogin";
            textBoxLogin.Size = new Size(263, 23);
            textBoxLogin.TabIndex = 17;
            // 
            // comboBoxGender
            // 
            comboBoxGender.FormattingEnabled = true;
            comboBoxGender.Items.AddRange(new object[] { "Mężczyzna", "Kobieta" });
            comboBoxGender.Location = new Point(268, 272);
            comboBoxGender.Margin = new Padding(2, 2, 2, 2);
            comboBoxGender.Name = "comboBoxGender";
            comboBoxGender.Size = new Size(263, 23);
            comboBoxGender.TabIndex = 16;
            // 
            // BirthDateTimePicker
            // 
            BirthDateTimePicker.CustomFormat = "YYYY-MM-DD";
            BirthDateTimePicker.ImeMode = ImeMode.Off;
            BirthDateTimePicker.Location = new Point(268, 245);
            BirthDateTimePicker.Margin = new Padding(2, 2, 2, 2);
            BirthDateTimePicker.Name = "BirthDateTimePicker";
            BirthDateTimePicker.Size = new Size(263, 23);
            BirthDateTimePicker.TabIndex = 15;
            // 
            // labelPhone
            // 
            labelPhone.AutoSize = true;
            labelPhone.Location = new Point(2, 324);
            labelPhone.Margin = new Padding(2, 0, 2, 0);
            labelPhone.Name = "labelPhone";
            labelPhone.Size = new Size(91, 15);
            labelPhone.TabIndex = 12;
            labelPhone.Text = "Numer telefonu";
            // 
            // labelEmail
            // 
            labelEmail.AutoSize = true;
            labelEmail.Location = new Point(2, 297);
            labelEmail.Margin = new Padding(2, 0, 2, 0);
            labelEmail.Name = "labelEmail";
            labelEmail.Size = new Size(74, 15);
            labelEmail.TabIndex = 11;
            labelEmail.Text = "Adres e-mail";
            // 
            // labelGender
            // 
            labelGender.AutoSize = true;
            labelGender.Location = new Point(2, 270);
            labelGender.Margin = new Padding(2, 0, 2, 0);
            labelGender.Name = "labelGender";
            labelGender.Size = new Size(29, 15);
            labelGender.TabIndex = 10;
            labelGender.Text = "Płeć";
            // 
            // labelBirthDate
            // 
            labelBirthDate.AutoSize = true;
            labelBirthDate.Location = new Point(2, 243);
            labelBirthDate.Margin = new Padding(2, 0, 2, 0);
            labelBirthDate.Name = "labelBirthDate";
            labelBirthDate.Size = new Size(87, 15);
            labelBirthDate.TabIndex = 9;
            labelBirthDate.Text = "Data Urodzenia";
            // 
            // labelPESEL
            // 
            labelPESEL.AutoSize = true;
            labelPESEL.Location = new Point(2, 216);
            labelPESEL.Margin = new Padding(2, 0, 2, 0);
            labelPESEL.Name = "labelPESEL";
            labelPESEL.Size = new Size(38, 15);
            labelPESEL.TabIndex = 8;
            labelPESEL.Text = "PESEL";
            // 
            // labelHouseUnitNumber
            // 
            labelHouseUnitNumber.AutoSize = true;
            labelHouseUnitNumber.Location = new Point(2, 189);
            labelHouseUnitNumber.Margin = new Padding(2, 0, 2, 0);
            labelHouseUnitNumber.Name = "labelHouseUnitNumber";
            labelHouseUnitNumber.Size = new Size(79, 15);
            labelHouseUnitNumber.TabIndex = 7;
            labelHouseUnitNumber.Text = "Numer lokalu";
            // 
            // labelPropertyNumber
            // 
            labelPropertyNumber.AutoSize = true;
            labelPropertyNumber.Location = new Point(2, 162);
            labelPropertyNumber.Margin = new Padding(2, 0, 2, 0);
            labelPropertyNumber.Name = "labelPropertyNumber";
            labelPropertyNumber.Size = new Size(83, 15);
            labelPropertyNumber.TabIndex = 6;
            labelPropertyNumber.Text = "Numer Posesji";
            // 
            // labelStreet
            // 
            labelStreet.AutoSize = true;
            labelStreet.Location = new Point(2, 135);
            labelStreet.Margin = new Padding(2, 0, 2, 0);
            labelStreet.Name = "labelStreet";
            labelStreet.Size = new Size(33, 15);
            labelStreet.TabIndex = 5;
            labelStreet.Text = "Ulica";
            // 
            // labelPostalCode
            // 
            labelPostalCode.AutoSize = true;
            labelPostalCode.Location = new Point(2, 108);
            labelPostalCode.Margin = new Padding(2, 0, 2, 0);
            labelPostalCode.Name = "labelPostalCode";
            labelPostalCode.Size = new Size(82, 15);
            labelPostalCode.TabIndex = 4;
            labelPostalCode.Text = "Kod Pocztowy";
            // 
            // labelLocality
            // 
            labelLocality.AutoSize = true;
            labelLocality.Location = new Point(2, 81);
            labelLocality.Margin = new Padding(2, 0, 2, 0);
            labelLocality.Name = "labelLocality";
            labelLocality.Size = new Size(75, 15);
            labelLocality.TabIndex = 3;
            labelLocality.Text = "Miejscowosc";
            // 
            // labelLastName
            // 
            labelLastName.AutoSize = true;
            labelLastName.Location = new Point(2, 54);
            labelLastName.Margin = new Padding(2, 0, 2, 0);
            labelLastName.Name = "labelLastName";
            labelLastName.Size = new Size(57, 15);
            labelLastName.TabIndex = 2;
            labelLastName.Text = "Nazwisko";
            // 
            // labelFirstName
            // 
            labelFirstName.AutoSize = true;
            labelFirstName.Location = new Point(2, 27);
            labelFirstName.Margin = new Padding(2, 0, 2, 0);
            labelFirstName.Name = "labelFirstName";
            labelFirstName.Size = new Size(30, 15);
            labelFirstName.TabIndex = 1;
            labelFirstName.Text = "Imię";
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
            labelLogin.UseWaitCursor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(labelLogin, 0, 0);
            tableLayoutPanel1.Controls.Add(labelFirstName, 0, 3);
            tableLayoutPanel1.Controls.Add(labelLastName, 0, 4);
            tableLayoutPanel1.Controls.Add(labelLocality, 0, 5);
            tableLayoutPanel1.Controls.Add(labelPostalCode, 0, 6);
            tableLayoutPanel1.Controls.Add(labelStreet, 0, 7);
            tableLayoutPanel1.Controls.Add(labelPropertyNumber, 0, 8);
            tableLayoutPanel1.Controls.Add(labelHouseUnitNumber, 0, 9);
            tableLayoutPanel1.Controls.Add(labelPESEL, 0, 10);
            tableLayoutPanel1.Controls.Add(labelBirthDate, 0, 11);
            tableLayoutPanel1.Controls.Add(labelGender, 0, 12);
            tableLayoutPanel1.Controls.Add(labelEmail, 0, 13);
            tableLayoutPanel1.Controls.Add(labelPhone, 0, 14);
            tableLayoutPanel1.Controls.Add(BirthDateTimePicker, 1, 11);
            tableLayoutPanel1.Controls.Add(comboBoxGender, 1, 12);
            tableLayoutPanel1.Controls.Add(textBoxLogin, 1, 0);
            tableLayoutPanel1.Controls.Add(textBoxFirstName, 1, 3);
            tableLayoutPanel1.Controls.Add(textBoxLastName, 1, 4);
            tableLayoutPanel1.Controls.Add(textBoxLocality, 1, 5);
            tableLayoutPanel1.Controls.Add(textBoxPostCode, 1, 6);
            tableLayoutPanel1.Controls.Add(textBoxStreet, 1, 7);
            tableLayoutPanel1.Controls.Add(textBoxPropertyNumber, 1, 8);
            tableLayoutPanel1.Controls.Add(textBoxHouseUnitNumber, 1, 9);
            tableLayoutPanel1.Controls.Add(textBoxPESEL, 1, 10);
            tableLayoutPanel1.Controls.Add(textBoxEmail, 1, 13);
            tableLayoutPanel1.Controls.Add(textBoxPhone, 1, 14);
            tableLayoutPanel1.Location = new Point(8, 7);
            tableLayoutPanel1.Margin = new Padding(2, 2, 2, 2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 15;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(533, 371);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // FormEditUser
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(550, 420);
            Controls.Add(buttonForgetUser);
            Controls.Add(buttonEditUser);
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(2, 2, 2, 2);
            Name = "FormEditUser";
            Text = "Edytuj Użytkownika";
            Load += FormAddUser_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
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
    }
}