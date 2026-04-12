using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SystemPrzychodznia.Data;
using SystemPrzychodznia.Services;

namespace SystemPrzychodznia
{
    public partial class FormLogin : Form
    {
        private readonly UserService _userService;
        private IdHolder _userID;

        public FormLogin(IdHolder id, UserService userService)
        {
            _userID = id;
            _userService = userService;
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }

        private void labelLogin_Click(object sender, EventArgs e)
        {

        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            _userID.Id = 1;
            this.Close();
        }
    }
}
