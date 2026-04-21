namespace SystemPrzychodznia
{
    partial class FormLogin
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
            labelLogin = new Label();
            labelPassword = new Label();
            textBoxLogin = new TextBox();
            textBoxPassword = new TextBox();
            buttonLogin = new Button();
            btnForgotPassword = new Button();
            SuspendLayout();
            // 
            // labelLogin
            // 
            labelLogin.AutoSize = true;
            labelLogin.Location = new Point(84, 55);
            labelLogin.Margin = new Padding(2, 0, 2, 0);
            labelLogin.Name = "labelLogin";
            labelLogin.Size = new Size(37, 15);
            labelLogin.TabIndex = 0;
            labelLogin.Text = "Login";
            labelLogin.Click += labelLogin_Click;
            // 
            // labelPassword
            // 
            labelPassword.AutoSize = true;
            labelPassword.Location = new Point(84, 83);
            labelPassword.Margin = new Padding(2, 0, 2, 0);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(37, 15);
            labelPassword.TabIndex = 1;
            labelPassword.Text = "Hasło";
            // 
            // textBoxLogin
            // 
            textBoxLogin.Location = new Point(148, 51);
            textBoxLogin.Margin = new Padding(2);
            textBoxLogin.Name = "textBoxLogin";
            textBoxLogin.Size = new Size(157, 23);
            textBoxLogin.TabIndex = 2;
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new Point(148, 79);
            textBoxPassword.Margin = new Padding(2);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.PasswordChar = '*';
            textBoxPassword.Size = new Size(157, 23);
            textBoxPassword.TabIndex = 3;
            textBoxPassword.TextChanged += textBoxPassword_TextChanged;
            // 
            // buttonLogin
            // 
            buttonLogin.Location = new Point(226, 109);
            buttonLogin.Margin = new Padding(2);
            buttonLogin.Name = "buttonLogin";
            buttonLogin.Size = new Size(79, 25);
            buttonLogin.TabIndex = 4;
            buttonLogin.Text = "Zaloguj";
            buttonLogin.UseVisualStyleBackColor = true;
            buttonLogin.Click += buttonLogin_Click;
            // 
            // btnForgotPassword
            // 
            btnForgotPassword.Location = new Point(140, 109);
            btnForgotPassword.Name = "btnForgotPassword";
            btnForgotPassword.Size = new Size(81, 23);
            btnForgotPassword.TabIndex = 5;
            btnForgotPassword.Text = "Przypomnij haslo";
            btnForgotPassword.UseVisualStyleBackColor = true;
            btnForgotPassword.Click += button1_Click;
            // 
            // FormLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(394, 165);
            Controls.Add(btnForgotPassword);
            Controls.Add(buttonLogin);
            Controls.Add(textBoxPassword);
            Controls.Add(textBoxLogin);
            Controls.Add(labelPassword);
            Controls.Add(labelLogin);
            Margin = new Padding(2);
            Name = "FormLogin";
            Text = "Logowanie";
            Load += FormLogin_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelLogin;
        private Label labelPassword;
        private TextBox textBoxLogin;
        private TextBox textBoxPassword;
        private Button buttonLogin;
        private Button btnForgotPassword;
    }
}