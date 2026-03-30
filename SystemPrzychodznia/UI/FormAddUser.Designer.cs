namespace SystemPrzychodznia
{
    partial class FormAddUser
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
            tableLayoutPanel1 = new TableLayoutPanel();
            labelLogin = new Label();
            labelFirstName = new Label();
            labelLastName = new Label();
            labelLocality = new Label();
            labelPostalCode = new Label();
            labelStreet = new Label();
            labelPropertyNumber = new Label();
            labelHouseUnitNumber = new Label();
            labelPESEL = new Label();
            labelBirthDate = new Label();
            labelGender = new Label();
            labelEmail = new Label();
            labelPhone = new Label();
            labelPass = new Label();
            labelPassReapeat = new Label();
            BirthDateTimePicker = new DateTimePicker();
            comboBoxGender = new ComboBox();
            textBoxLogin = new TextBox();
            textBoxPass = new TextBox();
            textBoxPassRepeat = new TextBox();
            textBoxFirstName = new TextBox();
            textBoxLastName = new TextBox();
            textBoxLocality = new TextBox();
            textBoxPostCode = new TextBox();
            textBoxStreet = new TextBox();
            textBoxPropertyNumber = new TextBox();
            textBoxHouseUnitNumber = new TextBox();
            textBoxPESEL = new TextBox();
            textBoxEmail = new TextBox();
            textBoxPhone = new TextBox();
            buttonSendUser = new Button();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
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
            tableLayoutPanel1.Controls.Add(labelPass, 0, 1);
            tableLayoutPanel1.Controls.Add(labelPassReapeat, 0, 2);
            tableLayoutPanel1.Controls.Add(BirthDateTimePicker, 1, 11);
            tableLayoutPanel1.Controls.Add(comboBoxGender, 1, 12);
            tableLayoutPanel1.Controls.Add(textBoxLogin, 1, 0);
            tableLayoutPanel1.Controls.Add(textBoxPass, 1, 1);
            tableLayoutPanel1.Controls.Add(textBoxPassRepeat, 1, 2);
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
            tableLayoutPanel1.Size = new Size(533, 460);
            tableLayoutPanel1.TabIndex = 0;
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
            // labelFirstName
            // 
            labelFirstName.AutoSize = true;
            labelFirstName.Location = new Point(2, 81);
            labelFirstName.Margin = new Padding(2, 0, 2, 0);
            labelFirstName.Name = "labelFirstName";
            labelFirstName.Size = new Size(30, 15);
            labelFirstName.TabIndex = 1;
            labelFirstName.Text = "Imię";
            // 
            // labelLastName
            // 
            labelLastName.AutoSize = true;
            labelLastName.Location = new Point(2, 108);
            labelLastName.Margin = new Padding(2, 0, 2, 0);
            labelLastName.Name = "labelLastName";
            labelLastName.Size = new Size(57, 15);
            labelLastName.TabIndex = 2;
            labelLastName.Text = "Nazwisko";
            // 
            // labelLocality
            // 
            labelLocality.AutoSize = true;
            labelLocality.Location = new Point(2, 135);
            labelLocality.Margin = new Padding(2, 0, 2, 0);
            labelLocality.Name = "labelLocality";
            labelLocality.Size = new Size(75, 15);
            labelLocality.TabIndex = 3;
            labelLocality.Text = "Miejscowosc";
            // 
            // labelPostalCode
            // 
            labelPostalCode.AutoSize = true;
            labelPostalCode.Location = new Point(2, 162);
            labelPostalCode.Margin = new Padding(2, 0, 2, 0);
            labelPostalCode.Name = "labelPostalCode";
            labelPostalCode.Size = new Size(82, 15);
            labelPostalCode.TabIndex = 4;
            labelPostalCode.Text = "Kod Pocztowy";
            // 
            // labelStreet
            // 
            labelStreet.AutoSize = true;
            labelStreet.Location = new Point(2, 189);
            labelStreet.Margin = new Padding(2, 0, 2, 0);
            labelStreet.Name = "labelStreet";
            labelStreet.Size = new Size(33, 15);
            labelStreet.TabIndex = 5;
            labelStreet.Text = "Ulica";
            // 
            // labelPropertyNumber
            // 
            labelPropertyNumber.AutoSize = true;
            labelPropertyNumber.Location = new Point(2, 216);
            labelPropertyNumber.Margin = new Padding(2, 0, 2, 0);
            labelPropertyNumber.Name = "labelPropertyNumber";
            labelPropertyNumber.Size = new Size(83, 15);
            labelPropertyNumber.TabIndex = 6;
            labelPropertyNumber.Text = "Numer Posesji";
            // 
            // labelHouseUnitNumber
            // 
            labelHouseUnitNumber.AutoSize = true;
            labelHouseUnitNumber.Location = new Point(2, 243);
            labelHouseUnitNumber.Margin = new Padding(2, 0, 2, 0);
            labelHouseUnitNumber.Name = "labelHouseUnitNumber";
            labelHouseUnitNumber.Size = new Size(79, 15);
            labelHouseUnitNumber.TabIndex = 7;
            labelHouseUnitNumber.Text = "Numer lokalu";
            // 
            // labelPESEL
            // 
            labelPESEL.AutoSize = true;
            labelPESEL.Location = new Point(2, 270);
            labelPESEL.Margin = new Padding(2, 0, 2, 0);
            labelPESEL.Name = "labelPESEL";
            labelPESEL.Size = new Size(38, 15);
            labelPESEL.TabIndex = 8;
            labelPESEL.Text = "PESEL";
            // 
            // labelBirthDate
            // 
            labelBirthDate.AutoSize = true;
            labelBirthDate.Location = new Point(2, 297);
            labelBirthDate.Margin = new Padding(2, 0, 2, 0);
            labelBirthDate.Name = "labelBirthDate";
            labelBirthDate.Size = new Size(87, 15);
            labelBirthDate.TabIndex = 9;
            labelBirthDate.Text = "Data Urodzenia";
            // 
            // labelGender
            // 
            labelGender.AutoSize = true;
            labelGender.Location = new Point(2, 324);
            labelGender.Margin = new Padding(2, 0, 2, 0);
            labelGender.Name = "labelGender";
            labelGender.Size = new Size(29, 15);
            labelGender.TabIndex = 10;
            labelGender.Text = "Płeć";
            // 
            // labelEmail
            // 
            labelEmail.AutoSize = true;
            labelEmail.Location = new Point(2, 351);
            labelEmail.Margin = new Padding(2, 0, 2, 0);
            labelEmail.Name = "labelEmail";
            labelEmail.Size = new Size(74, 15);
            labelEmail.TabIndex = 11;
            labelEmail.Text = "Adres e-mail";
            // 
            // labelPhone
            // 
            labelPhone.AutoSize = true;
            labelPhone.Location = new Point(2, 378);
            labelPhone.Margin = new Padding(2, 0, 2, 0);
            labelPhone.Name = "labelPhone";
            labelPhone.Size = new Size(91, 15);
            labelPhone.TabIndex = 12;
            labelPhone.Text = "Numer telefonu";
            // 
            // labelPass
            // 
            labelPass.AutoSize = true;
            labelPass.Location = new Point(2, 27);
            labelPass.Margin = new Padding(2, 0, 2, 0);
            labelPass.Name = "labelPass";
            labelPass.Size = new Size(37, 15);
            labelPass.TabIndex = 13;
            labelPass.Text = "Hasło";
            // 
            // labelPassReapeat
            // 
            labelPassReapeat.AutoSize = true;
            labelPassReapeat.Location = new Point(2, 54);
            labelPassReapeat.Margin = new Padding(2, 0, 2, 0);
            labelPassReapeat.Name = "labelPassReapeat";
            labelPassReapeat.Size = new Size(81, 15);
            labelPassReapeat.TabIndex = 14;
            labelPassReapeat.Text = "Powtórz hasło";
            // 
            // BirthDateTimePicker
            // 
            BirthDateTimePicker.CustomFormat = "yyyy-MM-dd";
            BirthDateTimePicker.ImeMode = ImeMode.Off;
            BirthDateTimePicker.Location = new Point(268, 299);
            BirthDateTimePicker.Margin = new Padding(2, 2, 2, 2);
            BirthDateTimePicker.Name = "BirthDateTimePicker";
            BirthDateTimePicker.Size = new Size(263, 23);
            BirthDateTimePicker.TabIndex = 15;
            // 
            // comboBoxGender
            // 
            comboBoxGender.FormattingEnabled = true;
            comboBoxGender.Items.AddRange(new object[] { "M", "K" });
            comboBoxGender.Location = new Point(268, 326);
            comboBoxGender.Margin = new Padding(2, 2, 2, 2);
            comboBoxGender.Name = "comboBoxGender";
            comboBoxGender.Size = new Size(263, 23);
            comboBoxGender.TabIndex = 16;
            // 
            // textBoxLogin
            // 
            textBoxLogin.Location = new Point(268, 2);
            textBoxLogin.Margin = new Padding(2, 2, 2, 2);
            textBoxLogin.Name = "textBoxLogin";
            textBoxLogin.Size = new Size(263, 23);
            textBoxLogin.TabIndex = 17;
            // 
            // textBoxPass
            // 
            textBoxPass.Location = new Point(268, 29);
            textBoxPass.Margin = new Padding(2, 2, 2, 2);
            textBoxPass.Name = "textBoxPass";
            textBoxPass.Size = new Size(263, 23);
            textBoxPass.TabIndex = 18;
            // 
            // textBoxPassRepeat
            // 
            textBoxPassRepeat.Location = new Point(268, 56);
            textBoxPassRepeat.Margin = new Padding(2, 2, 2, 2);
            textBoxPassRepeat.Name = "textBoxPassRepeat";
            textBoxPassRepeat.Size = new Size(263, 23);
            textBoxPassRepeat.TabIndex = 19;
            // 
            // textBoxFirstName
            // 
            textBoxFirstName.Location = new Point(268, 83);
            textBoxFirstName.Margin = new Padding(2, 2, 2, 2);
            textBoxFirstName.Name = "textBoxFirstName";
            textBoxFirstName.Size = new Size(263, 23);
            textBoxFirstName.TabIndex = 20;
            // 
            // textBoxLastName
            // 
            textBoxLastName.Location = new Point(268, 110);
            textBoxLastName.Margin = new Padding(2, 2, 2, 2);
            textBoxLastName.Name = "textBoxLastName";
            textBoxLastName.Size = new Size(263, 23);
            textBoxLastName.TabIndex = 21;
            // 
            // textBoxLocality
            // 
            textBoxLocality.Location = new Point(268, 137);
            textBoxLocality.Margin = new Padding(2, 2, 2, 2);
            textBoxLocality.Name = "textBoxLocality";
            textBoxLocality.Size = new Size(263, 23);
            textBoxLocality.TabIndex = 22;
            // 
            // textBoxPostCode
            // 
            textBoxPostCode.Location = new Point(268, 164);
            textBoxPostCode.Margin = new Padding(2, 2, 2, 2);
            textBoxPostCode.Name = "textBoxPostCode";
            textBoxPostCode.Size = new Size(263, 23);
            textBoxPostCode.TabIndex = 23;
            // 
            // textBoxStreet
            // 
            textBoxStreet.Location = new Point(268, 191);
            textBoxStreet.Margin = new Padding(2, 2, 2, 2);
            textBoxStreet.Name = "textBoxStreet";
            textBoxStreet.Size = new Size(263, 23);
            textBoxStreet.TabIndex = 24;
            // 
            // textBoxPropertyNumber
            // 
            textBoxPropertyNumber.Location = new Point(268, 218);
            textBoxPropertyNumber.Margin = new Padding(2, 2, 2, 2);
            textBoxPropertyNumber.Name = "textBoxPropertyNumber";
            textBoxPropertyNumber.Size = new Size(263, 23);
            textBoxPropertyNumber.TabIndex = 25;
            // 
            // textBoxHouseUnitNumber
            // 
            textBoxHouseUnitNumber.Location = new Point(268, 245);
            textBoxHouseUnitNumber.Margin = new Padding(2, 2, 2, 2);
            textBoxHouseUnitNumber.Name = "textBoxHouseUnitNumber";
            textBoxHouseUnitNumber.Size = new Size(263, 23);
            textBoxHouseUnitNumber.TabIndex = 26;
            // 
            // textBoxPESEL
            // 
            textBoxPESEL.Location = new Point(268, 272);
            textBoxPESEL.Margin = new Padding(2, 2, 2, 2);
            textBoxPESEL.Name = "textBoxPESEL";
            textBoxPESEL.Size = new Size(263, 23);
            textBoxPESEL.TabIndex = 27;
            // 
            // textBoxEmail
            // 
            textBoxEmail.Location = new Point(268, 353);
            textBoxEmail.Margin = new Padding(2, 2, 2, 2);
            textBoxEmail.Name = "textBoxEmail";
            textBoxEmail.Size = new Size(263, 23);
            textBoxEmail.TabIndex = 28;
            // 
            // textBoxPhone
            // 
            textBoxPhone.Location = new Point(268, 380);
            textBoxPhone.Margin = new Padding(2, 2, 2, 2);
            textBoxPhone.Name = "textBoxPhone";
            textBoxPhone.Size = new Size(263, 23);
            textBoxPhone.TabIndex = 29;
            // 
            // buttonSendUser
            // 
            buttonSendUser.Location = new Point(276, 472);
            buttonSendUser.Margin = new Padding(2, 2, 2, 2);
            buttonSendUser.Name = "buttonSendUser";
            buttonSendUser.Size = new Size(262, 20);
            buttonSendUser.TabIndex = 1;
            buttonSendUser.Text = "Dodaj Użytkownika";
            buttonSendUser.UseVisualStyleBackColor = true;
            buttonSendUser.Click += buttonSendUser_Click;
            // 
            // FormAddUser
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(550, 535);
            Controls.Add(buttonSendUser);
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(2, 2, 2, 2);
            Name = "FormAddUser";
            Text = "Dodaj Użytkownika";
            Load += FormAddUser_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label labelLogin;
        private Label labelFirstName;
        private Label labelLastName;
        private Label labelLocality;
        private Label labelPostalCode;
        private Label labelStreet;
        private Label labelPropertyNumber;
        private Label labelHouseUnitNumber;
        private Label labelPESEL;
        private Label labelBirthDate;
        private Label labelGender;
        private Label labelEmail;
        private Label labelPhone;
        private Label labelPass;
        private Label labelPassReapeat;
        private DateTimePicker BirthDateTimePicker;
        private ComboBox comboBoxGender;
        private TextBox textBoxLogin;
        private TextBox textBoxPass;
        private TextBox textBoxPassRepeat;
        private TextBox textBoxFirstName;
        private TextBox textBoxLastName;
        private TextBox textBoxLocality;
        private TextBox textBoxPostCode;
        private TextBox textBoxStreet;
        private TextBox textBoxPropertyNumber;
        private TextBox textBoxHouseUnitNumber;
        private TextBox textBoxPESEL;
        private TextBox textBoxEmail;
        private TextBox textBoxPhone;
        private Button buttonSendUser;
    }
}