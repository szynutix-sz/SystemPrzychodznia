using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using SystemPrzychodznia.Data;
using SystemPrzychodznia.Services;

namespace SystemPrzychodznia
{
    public partial class FormEditUser : Form
    {

        private readonly UserService _userService;
        private UserBasic _user;
        private UserFull uF;
        private UserFull _editingUser;
        public FormEditUser(UserService service, UserBasic u, UserFull editingUser)
        {
            InitializeComponent();
            _userService = service;
            _user = u;
            _editingUser = editingUser;

        }

        private void buttonEditUser_Click(object sender, EventArgs e)
        {
            if (textBoxNewPass.Text.Trim() != "")
            {
                MessageBox.Show("Niedokończona zmiana hasła. By zmienić hasło w zakładce hasło wpisz nowe oraz nacisnij", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxNewPass.Text = "";
            }

            UserFull userBeforeValid = new UserFull();

            userBeforeValid.Uprawnienia = _userService.GetUprawnienia();

            userBeforeValid.Id = uF.Id;
            userBeforeValid.Login = textBoxLogin.Text.Trim();
            userBeforeValid.FirstName = textBoxFirstName.Text.Trim();
            userBeforeValid.LastName = textBoxLastName.Text.Trim();
            userBeforeValid.Locality = textBoxLocality.Text.Trim();
            userBeforeValid.PostalCode = textBoxPostCode.Text.Trim().Replace("-", "");
            userBeforeValid.Street = textBoxStreet.Text.Trim();
            userBeforeValid.PropertyNumber = textBoxPropertyNumber.Text.Trim();
            userBeforeValid.HouseUnitNumber = textBoxHouseUnitNumber.Text.Trim();
            userBeforeValid.PESEL = textBoxPESEL.Text.Trim();
            userBeforeValid.BirthDate = (BirthDateTimePicker.Value).ToString("yyyy-MM-dd");
            userBeforeValid.Gender = comboBoxGender.Text;
            userBeforeValid.Email = textBoxEmail.Text.Trim();
            userBeforeValid.Phone = textBoxPhone.Text.Trim();
            userBeforeValid.Password = "LqyA9Ti9iz$i9iz"; // Nie jest edytowane, ale musi być przekazane do walidacji, więc ustawiamy na wartość tymczasową

            foreach (Uprawnienie u in userBeforeValid.Uprawnienia)
            {
                u.Posiadane = checkedListBoxUprawnienia.CheckedItems.Contains(u.Nazwa);
            }

            bool somethingChanged = !(userBeforeValid == uF);

            if (somethingChanged == true)
            {
                ValidationResult valRe = _userService.EditUser(userBeforeValid);

                if (valRe.IsValid == true)
                {
                    MessageBox.Show("Zmieniono dane użytkownika", "Informacja");
                    FormEditUser_Load(null, null);
                    LockEditing();
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

        private void FormEditUser_Load(object sender, EventArgs e)
        {

            uF = _userService.GetUserFull(_user.Id);
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
            this.Text = $"Podgląd użytkownika: {uF.Login}";

            textBoxNewPass.Text = "";
            textBoxNewPassRepeat.Text = "";

            checkedListBoxUprawnienia.Items.Clear();
            foreach (Uprawnienie uprawnienie in uF.Uprawnienia)
            {
                checkedListBoxUprawnienia.Items.Add(uprawnienie.Nazwa, uprawnienie.Posiadane == true);
            }


            LockEditing();

        }
        private void LockEditing()
        {

            textBoxLogin.Enabled = false;
            textBoxFirstName.Enabled = false;
            textBoxLastName.Enabled = false;
            textBoxLocality.Enabled = false;
            textBoxPostCode.Enabled = false;
            textBoxStreet.Enabled = false;
            textBoxPropertyNumber.Enabled = false;
            textBoxHouseUnitNumber.Enabled = false;
            textBoxPESEL.Enabled = false;
            BirthDateTimePicker.Enabled = false;
            comboBoxGender.Enabled = false;
            textBoxEmail.Enabled = false;
            textBoxPhone.Enabled = false;

            buttonEditUser.Enabled = false;
            buttonForgetUser.Enabled = false;

            checkedListBoxUprawnienia.Enabled = false;

            textBoxNewPass.Enabled = false;
            textBoxNewPassRepeat.Enabled = false;
            buttonChangePass.Enabled = false;

            buttonUnlockEditing.Text = "Odblokuj edycję";
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
                try
                {
                    ValidationResult valRe = _userService.ForgetUser(uF, _editingUser.Id);

                    if (valRe.IsValid == true)
                    {
                        MessageBox.Show("Użytkownik został zapomniany.", "Informacja");
                    }
                    else
                    {
                        string errorMessage = string.Join(Environment.NewLine, valRe.Errors);
                        errorMessage += "\n Spróbuj ponownie. Mogł się wylosować już istniejący login";
                        MessageBox.Show(errorMessage, "Błąd walidacji");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Wystąpił błąd podczas zapominania użytkownika:\n" + ex.Message, "Błąd");
                }
                this.Close();
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonUnlockEditing_Click(object sender, EventArgs e)
        {
            if (buttonEditUser.Enabled == true)
            {
                uF = _userService.GetUserFull(_user.Id);
                FormEditUser_Load(null, null); // przywracamy dane z bazy, by anulować wprowadzone zmiany
                LockEditing();

            }
            else
            {
                textBoxLogin.Enabled = true;
                textBoxFirstName.Enabled = true;
                textBoxLastName.Enabled = true;
                textBoxLocality.Enabled = true;
                textBoxPostCode.Enabled = true;
                textBoxStreet.Enabled = true;
                textBoxPropertyNumber.Enabled = true;
                textBoxHouseUnitNumber.Enabled = true;
                textBoxPESEL.Enabled = true;
                BirthDateTimePicker.Enabled = true;
                comboBoxGender.Enabled = true;
                textBoxEmail.Enabled = true;
                textBoxPhone.Enabled = true;

                buttonEditUser.Enabled = true;
                buttonForgetUser.Enabled = true;

                checkedListBoxUprawnienia.Enabled = true;

                textBoxNewPass.Enabled = true;
                textBoxNewPassRepeat.Enabled = true;
                buttonChangePass.Enabled = true;

                buttonUnlockEditing.Text = "Zrezygnuj z edycji";
                this.Text = $"Edytujesz użytkownika: {uF.Login}";
            }

        }

        private void buttonChangePass_Click(object sender, EventArgs e)
        {
            ValidationResult valRe = _userService.ChangeUserPassword(uF.Id, textBoxNewPass.Text.Trim());

            if (valRe.IsValid == true)
            {
                MessageBox.Show("Zmieniono hasło użytkownika", "Informacja");
                textBoxNewPass.Text = "";
            }
            else
            {
                string errorMessage = string.Join(Environment.NewLine, valRe.Errors);
                MessageBox.Show(errorMessage, "Błąd walidacji");
            }

            
        }
    }
}
