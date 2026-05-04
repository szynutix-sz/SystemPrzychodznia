using System.Globalization;
using SystemPrzychodznia.Data;
using SystemPrzychodznia.Services;

namespace SystemPrzychodznia
{
    public class FormPatientEditor : Form
    {
        private readonly UserService _userService;
        private readonly int? _patientId;

        private UserFull? _loadedPatient;

        private readonly TextBox _textBoxFirstName = new();
        private readonly TextBox _textBoxLastName = new();
        private readonly TextBox _textBoxPESEL = new();
        private readonly DateTimePicker _dateTimePickerBirthDate = new();
        private readonly ComboBox _comboBoxGender = new();
        private readonly TextBox _textBoxLocality = new();
        private readonly TextBox _textBoxPostalCode = new();
        private readonly TextBox _textBoxStreet = new();
        private readonly TextBox _textBoxPropertyNumber = new();
        private readonly TextBox _textBoxHouseUnitNumber = new();
        private readonly TextBox _textBoxPhone = new();
        private readonly TextBox _textBoxEmail = new();
        private readonly Button _buttonSave = new();
        private readonly Button _buttonToggleEdit = new();
        private readonly Button _buttonClose = new();

        public FormPatientEditor(UserService userService, int? patientId = null)
        {
            _userService = userService;
            _patientId = patientId;

            InitializePatientEditor();
            Load += FormPatientEditor_Load;
        }

        private void InitializePatientEditor()
        {
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(760, 620);
            MinimumSize = new Size(760, 620);

            var tableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 13,
                Padding = new Padding(16)
            };

            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            for (int i = 0; i < 12; i++)
            {
                tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            }

            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            _dateTimePickerBirthDate.Format = DateTimePickerFormat.Custom;
            _dateTimePickerBirthDate.CustomFormat = "yyyy-MM-dd";

            _comboBoxGender.DropDownStyle = ComboBoxStyle.DropDownList;
            _comboBoxGender.Items.AddRange(new object[] { "M", "K" });

            AddField(tableLayout, 0, "Imię", _textBoxFirstName);
            AddField(tableLayout, 1, "Nazwisko", _textBoxLastName);
            AddField(tableLayout, 2, "PESEL", _textBoxPESEL);
            AddField(tableLayout, 3, "Data urodzenia", _dateTimePickerBirthDate);
            AddField(tableLayout, 4, "Płeć", _comboBoxGender);
            AddField(tableLayout, 5, "Miejscowość", _textBoxLocality);
            AddField(tableLayout, 6, "Kod pocztowy", _textBoxPostalCode);
            AddField(tableLayout, 7, "Ulica", _textBoxStreet);
            AddField(tableLayout, 8, "Numer posesji", _textBoxPropertyNumber);
            AddField(tableLayout, 9, "Numer lokalu", _textBoxHouseUnitNumber);
            AddField(tableLayout, 10, "Numer telefonu", _textBoxPhone);
            AddField(tableLayout, 11, "Adres e-mail", _textBoxEmail);

            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 12, 0, 0)
            };

            _buttonClose.Text = "Zamknij";
            _buttonClose.AutoSize = true;
            _buttonClose.Click += (_, _) => Close();

            _buttonSave.Text = "Zapisz";
            _buttonSave.AutoSize = true;
            _buttonSave.Click += ButtonSave_Click;

            _buttonToggleEdit.Text = "Odblokuj edycję";
            _buttonToggleEdit.AutoSize = true;
            _buttonToggleEdit.Click += ButtonToggleEdit_Click;

            buttonPanel.Controls.Add(_buttonClose);
            buttonPanel.Controls.Add(_buttonSave);
            buttonPanel.Controls.Add(_buttonToggleEdit);

            tableLayout.Controls.Add(buttonPanel, 0, 12);
            tableLayout.SetColumnSpan(buttonPanel, 2);

            Controls.Add(tableLayout);
        }

        private void AddField(TableLayoutPanel tableLayout, int rowIndex, string labelText, Control control)
        {
            var label = new Label
            {
                Text = labelText,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            control.Dock = DockStyle.Fill;

            tableLayout.Controls.Add(label, 0, rowIndex);
            tableLayout.Controls.Add(control, 1, rowIndex);
        }

        private void FormPatientEditor_Load(object? sender, EventArgs e)
        {
            if (_patientId.HasValue)
            {
                _loadedPatient = _userService.GetUserFull(_patientId.Value);
                if (_loadedPatient is null)
                {
                    MessageBox.Show("Nie udało się wczytać danych pacjenta.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }

                ApplyPatientToForm(_loadedPatient);
                Text = $"Podgląd pacjenta: {_loadedPatient.FirstName} {_loadedPatient.LastName}";
                SetEditingState(false);
            }
            else
            {
                Text = "Rejestracja pacjenta";
                _buttonToggleEdit.Visible = false;
                _buttonSave.Text = "Zarejestruj pacjenta";
                _comboBoxGender.SelectedIndex = 0;
            }
        }

        private void ApplyPatientToForm(UserFull patient)
        {
            _textBoxFirstName.Text = patient.FirstName;
            _textBoxLastName.Text = patient.LastName;
            _textBoxPESEL.Text = patient.PESEL;
            _textBoxLocality.Text = patient.Locality;
            _textBoxPostalCode.Text = patient.PostalCode;
            _textBoxStreet.Text = patient.Street ?? string.Empty;
            _textBoxPropertyNumber.Text = patient.PropertyNumber;
            _textBoxHouseUnitNumber.Text = patient.HouseUnitNumber ?? string.Empty;
            _textBoxPhone.Text = patient.Phone;
            _textBoxEmail.Text = patient.Email;
            _comboBoxGender.Text = patient.Gender;

            if (DateTime.TryParseExact(patient.BirthDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var birthDate))
            {
                _dateTimePickerBirthDate.Value = birthDate;
            }
        }

        private UserFull CollectPatientFromForm()
        {
            return new UserFull
            {
                Id = _loadedPatient?.Id ?? 0,
                Login = _loadedPatient?.Login ?? string.Empty,
                FirstName = _textBoxFirstName.Text.Trim(),
                LastName = _textBoxLastName.Text.Trim(),
                PESEL = _textBoxPESEL.Text.Trim(),
                BirthDate = _dateTimePickerBirthDate.Value.ToString("yyyy-MM-dd"),
                Gender = _comboBoxGender.Text,
                Locality = _textBoxLocality.Text.Trim(),
                PostalCode = _textBoxPostalCode.Text.Trim(),
                Street = _textBoxStreet.Text.Trim(),
                PropertyNumber = _textBoxPropertyNumber.Text.Trim(),
                HouseUnitNumber = _textBoxHouseUnitNumber.Text.Trim(),
                Phone = _textBoxPhone.Text.Trim(),
                Email = _textBoxEmail.Text.Trim()
            };
        }

        private void SetEditingState(bool enabled)
        {
            _textBoxFirstName.Enabled = enabled;
            _textBoxLastName.Enabled = enabled;
            _textBoxPESEL.Enabled = enabled;
            _dateTimePickerBirthDate.Enabled = enabled;
            _comboBoxGender.Enabled = enabled;
            _textBoxLocality.Enabled = enabled;
            _textBoxPostalCode.Enabled = enabled;
            _textBoxStreet.Enabled = enabled;
            _textBoxPropertyNumber.Enabled = enabled;
            _textBoxHouseUnitNumber.Enabled = enabled;
            _textBoxPhone.Enabled = enabled;
            _textBoxEmail.Enabled = enabled;
            _buttonSave.Enabled = enabled || !_patientId.HasValue;
            _buttonToggleEdit.Text = enabled ? "Anuluj edycję" : "Odblokuj edycję";
        }

        private void ButtonToggleEdit_Click(object? sender, EventArgs e)
        {
            bool enableEditing = !_textBoxFirstName.Enabled;
            if (!enableEditing && _loadedPatient != null)
            {
                ApplyPatientToForm(_loadedPatient);
            }

            SetEditingState(enableEditing);

            if (enableEditing && _loadedPatient != null)
            {
                Text = $"Edycja pacjenta: {_loadedPatient.FirstName} {_loadedPatient.LastName}";
            }
            else if (_loadedPatient != null)
            {
                Text = $"Podgląd pacjenta: {_loadedPatient.FirstName} {_loadedPatient.LastName}";
            }
        }

        private void ButtonSave_Click(object? sender, EventArgs e)
        {
            try
            {
                var patient = CollectPatientFromForm();
                ValidationResult result = _patientId.HasValue
                    ? _userService.EditPatient(patient)
                    : _userService.RegisterPatient(patient);

                if (result.IsValid)
                {
                    MessageBox.Show(
                        _patientId.HasValue ? "Dane pacjenta zostały zapisane." : "Pacjent został zarejestrowany.",
                        "Informacja",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    if (_patientId.HasValue)
                    {
                        _loadedPatient = _userService.GetUserFull(patient.Id);
                        if (_loadedPatient is not null)
                        {
                            ApplyPatientToForm(_loadedPatient);
                            SetEditingState(false);
                            Text = $"Podgląd pacjenta: {_loadedPatient.FirstName} {_loadedPatient.LastName}";
                        }
                    }
                    else
                    {
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show(string.Join(Environment.NewLine, result.Errors), "Błąd walidacji", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas zapisu pacjenta:{Environment.NewLine}{ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
