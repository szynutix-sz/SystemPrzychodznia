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

        private readonly UserService _userService;
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
                userBeforeValid.PostalCode = textBoxPostCode.Text.Trim().Replace("-","");
                userBeforeValid.Street = textBoxStreet.Text.Trim();
                userBeforeValid.PropertyNumber = textBoxPropertyNumber.Text.Trim();
                userBeforeValid.HouseUnitNumber = textBoxHouseUnitNumber.Text.Trim();
                userBeforeValid.PESEL = textBoxPESEL.Text.Trim();
                userBeforeValid.BirthDate = (BirthDateTimePicker.Value).ToString("yyyy-MM-dd");
                userBeforeValid.Gender = comboBoxGender.Text;
                userBeforeValid.Email = textBoxEmail.Text.Trim();
                userBeforeValid.Phone = textBoxPhone.Text.Trim();
                userBeforeValid.Password = textBoxPass.Text;


                foreach (var item in checkedListBoxUprawnienia.CheckedItems)
                {
                    userBeforeValid.Uprawnienia.Add(item.ToString());
                }

                try
                {
                    ValidationResult valRe = _userService.AddUser(userBeforeValid);

                    if (valRe.IsValid == true)
                    {
                        MessageBox.Show("Dodano użytkownika", "Dodano użytkownika");
                        this.Close();
                    }
                    else
                    {
                        string errorMessage = string.Join(Environment.NewLine, valRe.Errors);
                        MessageBox.Show(errorMessage, "Błąd walidacji");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Wystąpił błąd: " + ex.Message, "Błąd");
                }

            }
            else
            {
                MessageBox.Show("Hasło i jego powtórzenie muszą być takie same", "Błąd walidacji");
                textBoxPass.Clear();
                textBoxPassRepeat.Clear();
            }


        }

        private void FormAddUser_Load(object sender, EventArgs e)
        {
            BirthDateTimePicker.Format = DateTimePickerFormat.Custom;
            BirthDateTimePicker.CustomFormat = "yyyy-MM-dd";
            //MessageBox.Show(BirthDateTimePicker.Value.ToString("yyyy-MM-dd"), "OK");

            List<string> uprawnienia = _userService.GetUprawnienia();

            foreach (string item in uprawnienia)
            {
                checkedListBoxUprawnienia.Items.Add(item);
            }
        }

    }
}
