namespace SystemPrzychodznia.UI
{
    partial class FormRoleFilter
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
            checkedListBox1 = new CheckedListBox();
            btnApply = new Button();
            SuspendLayout();
            // 
            // checkedListBox1
            // 
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Location = new Point(207, 12);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(297, 328);
            checkedListBox1.TabIndex = 0;
            // 
            // btnApply
            // 
            btnApply.Location = new Point(207, 387);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(297, 23);
            btnApply.TabIndex = 1;
            btnApply.Text = "Zastosuj filtry";
            btnApply.UseVisualStyleBackColor = true;
            btnApply.Click += btnApply_Click;
            // 
            // FormRoleFilter
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnApply);
            Controls.Add(checkedListBox1);
            Name = "FormRoleFilter";
            Text = "CheckedListBox";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private Button btnApply;
    }
}