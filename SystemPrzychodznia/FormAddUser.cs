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
    public partial class FormAddUser : Form
    {

        private UserService _userService;
        public FormAddUser(UserService service)
        {
            InitializeComponent();
            _userService = service;
        }

        private void buttonSendUser_Click(object sender, EventArgs e)
        {
            if (textBoxPass.Text == textBoxPassRepeat.Text)
            {
                UserFull userBeforeValid = new UserFull();

                userBeforeValid.Login = textBoxLogin.Text.Trim();
                userBeforeValid.FirstName = textBoxFirstName.Text.Trim();
                userBeforeValid.LastName = textBoxLastName.Text.Trim();
                userBeforeValid.Locality = textBoxLocality.Text.Trim();
                userBeforeValid.PostalCode = textBoxPostCode.Text.Trim();
                userBeforeValid.Street = textBoxStreet.Text.Trim();
                userBeforeValid.PropertyNumber = textBoxPropertyNumber.Text.Trim();
                userBeforeValid.HouseUnitNumber = textBoxHouseUnitNumber.Text.Trim();
                userBeforeValid.PESEL = textBoxPESEL.Text.Trim();
                userBeforeValid.BirthDate = (BirthDateTimePicker.Value.Date).ToString();
                userBeforeValid.Gender = comboBoxGender.Text;
                userBeforeValid.Email = textBoxEmail.Text.Trim();
                userBeforeValid.Phone = textBoxPhone.Text.Trim();
                userBeforeValid.Password = textBoxPass.Text;

                ValidationResult valRe = _userService.AddUser(userBeforeValid);

                if (valRe.getResult() == true)
                {
                    MessageBox.Show("Dodano użytkownika", "OK");
                    textBoxLogin.Clear();
                    textBoxFirstName.Clear();
                    textBoxLastName.Clear();
                    textBoxLocality.Clear();
                    textBoxPostCode.Clear();
                    textBoxStreet.Clear();
                    textBoxPropertyNumber.Clear();
                    textBoxHouseUnitNumber.Clear();
                    textBoxPESEL.Clear();
                    BirthDateTimePicker.Value = DateTime.Today;
                    comboBoxGender.SelectedIndex = -1;
                    textBoxEmail.Clear();
                    textBoxPhone.Clear();
                    textBoxPass.Clear();
                    textBoxPassRepeat.Clear();
                }
                else
                {
                    MessageBox.Show(valRe.getMessage(), "OK");
                }

            }
            else
            {
                MessageBox.Show("Hasło i jego powtórzenie muszą być takie same", "OK");
                textBoxPass.Clear();
                textBoxPassRepeat.Clear();
            }


        }

        private void FormAddUser_Load(object sender, EventArgs e)
        {

        }
    }
}
