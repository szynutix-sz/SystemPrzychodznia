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

        private readonly UserService _userService;
        private User _user;
        private UserFull uF;
        public FormEditUser(UserService service, User u)
        {
            InitializeComponent();
            _userService = service;
            _user = u;
            uF = _userService.GetUserFull(_user.Login);

        }

        private void buttonEditUser_Click(object sender, EventArgs e)
        {
            UserFull userBeforeValid = new UserFull();

            userBeforeValid.Id = uF.Id;
            userBeforeValid.Login = textBoxLogin.Text.Trim();
            userBeforeValid.FirstName = textBoxFirstName.Text.Trim();
            userBeforeValid.LastName = textBoxLastName.Text.Trim();
            userBeforeValid.Locality = textBoxLocality.Text.Trim();
            userBeforeValid.PostalCode = textBoxPostCode.Text.Trim();
            userBeforeValid.Street = textBoxStreet.Text.Trim();
            userBeforeValid.PropertyNumber = textBoxPropertyNumber.Text.Trim();
            userBeforeValid.HouseUnitNumber = textBoxHouseUnitNumber.Text.Trim();
            userBeforeValid.PESEL = textBoxPESEL.Text.Trim();
            userBeforeValid.BirthDate = (BirthDateTimePicker.Value).ToString("yyyy-MM-dd");
            userBeforeValid.Gender = comboBoxGender.Text;
            userBeforeValid.Email = textBoxEmail.Text.Trim();
            userBeforeValid.Phone = textBoxPhone.Text.Trim();
            userBeforeValid.Password = "DummyPassword"; // Nie jest edytowane, ale musi być przekazane do walidacji, więc ustawiamy na wartość tymczasową


            bool somethingChanged = userBeforeValid.Login != uF.Login || userBeforeValid.FirstName != uF.FirstName;

            somethingChanged = somethingChanged || userBeforeValid.LastName != uF.LastName || userBeforeValid.Locality != uF.Locality;
            somethingChanged = somethingChanged || userBeforeValid.PostalCode != uF.PostalCode || userBeforeValid.Street != uF.Street;
            somethingChanged = somethingChanged || userBeforeValid.PropertyNumber != uF.PropertyNumber || userBeforeValid.HouseUnitNumber != uF.HouseUnitNumber;
            somethingChanged = somethingChanged || userBeforeValid.PESEL != uF.PESEL || userBeforeValid.BirthDate != uF.BirthDate;
            somethingChanged = somethingChanged || userBeforeValid.Gender != uF.Gender;
            somethingChanged = somethingChanged || userBeforeValid.Email != uF.Email || userBeforeValid.Phone != uF.Phone;


            if (somethingChanged == true)
            {
                ValidationResult valRe = _userService.EditUser(userBeforeValid);

                if (valRe.IsValid == true)
                {
                    MessageBox.Show("Zmieniono dane użytkownika", "Informacja");

                }
                else
                {
                    string errorMessage = string.Join(Environment.NewLine, valRe.Errors);
                    MessageBox.Show(errorMessage, "Błąd walidacji");
                }
                
            }
            else
            {
                MessageBox.Show("Nie wprowadzono żadnych zmian", "Informacja");
            }





        }

        private void FormAddUser_Load(object sender, EventArgs e)
        {
            textBoxLogin.Text = uF.Login;
            textBoxFirstName.Text = uF.FirstName;
            textBoxLastName.Text = uF.LastName;
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

            private void buttonForgetUser_Click(object sender, EventArgs e)
            {
                var result = MessageBox.Show(
                    $"Czy na pewno chcesz zapomnieć użytkownika {uF.Login}?",
                    "Potwierdzenie",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    _userService.ForgetUser(uF.Id);
                    MessageBox.Show("Użytkownik został zapomniany.", "Informacja");
                    this.Close();
                }
            }

        }
    }
