namespace SystemPrzychodznia.UI
{
    partial class FormRecoverPass
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
            label1 = new Label();
            label2 = new Label();
            txtLogin = new TextBox();
            txtEmail = new TextBox();
            btnRecover = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(196, 145);
            label1.Name = "label1";
            label1.Size = new Size(37, 15);
            label1.TabIndex = 0;
            label1.Text = "Login";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(192, 238);
            label2.Name = "label2";
            label2.Size = new Size(41, 15);
            label2.TabIndex = 1;
            label2.Text = "E-mail";
            // 
            // txtLogin
            // 
            txtLogin.Location = new Point(331, 145);
            txtLogin.Name = "txtLogin";
            txtLogin.Size = new Size(208, 23);
            txtLogin.TabIndex = 2;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(331, 230);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(208, 23);
            txtEmail.TabIndex = 3;
            // 
            // btnRecover
            // 
            btnRecover.Location = new Point(356, 311);
            btnRecover.Name = "btnRecover";
            btnRecover.Size = new Size(147, 23);
            btnRecover.TabIndex = 4;
            btnRecover.Text = "Odzyskaj Hasło";
            btnRecover.UseVisualStyleBackColor = true;
            btnRecover.Click += btnRecover_Click;
            // 
            // FormRecoverPass
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnRecover);
            Controls.Add(txtEmail);
            Controls.Add(txtLogin);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "FormRecoverPass";
            Text = "Odzyskiwanie hasła";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtLogin;
        private TextBox txtEmail;
        private Button btnRecover;
    }
}