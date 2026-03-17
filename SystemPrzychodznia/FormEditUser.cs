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
    public partial class FormEditUser : Form
    {

        private UserService _userService;
        private User _user;
        private UserFull uF;
        public FormEditUser(UserService service, User u)
        {
            InitializeComponent();
            _userService = service;
            _user = u;
            
        }

        private void buttonSendUser_Click(object sender, EventArgs e)
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

            // trzeba zmienić na edycję użytkownika, a nie dodawanie nowego, ale to już później
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
            }
            else
            {
                MessageBox.Show(valRe.getMessage(), "OK");
            }



        }

        private void FormAddUser_Load(object sender, EventArgs e)
        {
            uF = _userService.GetUserFull(_user.Login);

            textBoxLogin.Text = uF.Login;
            textBoxFirstName.Text = uF.FirstName;
            textBoxLastName.Text = uF.Login;
            textBoxLocality.Text = uF.Locality;
            textBoxPostCode.Text = uF.PostalCode;
            textBoxStreet.Text = uF.Street;
            textBoxPropertyNumber.Text = uF.PropertyNumber;
            textBoxHouseUnitNumber.Text = uF.HouseUnitNumber;
            textBoxPESEL.Text = uF.PESEL;
            BirthDateTimePicker.Value = DateTime.Parse(uF.BirthDate);
            comboBoxGender.Text = uF.Gender;
            textBoxEmail.Text = uF.Email;
            textBoxPhone.Text = uF.Phone;
        }
    }
}
