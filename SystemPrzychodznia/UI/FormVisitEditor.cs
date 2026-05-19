using SystemPrzychodznia.Services;

namespace SystemPrzychodznia
{
    internal class FormVisitEditor : Form
    {
        private readonly List<FormUserView.PatientChoice> _patients;
        private readonly List<FormUserView.DoctorChoice> _doctors;
        private readonly List<FormUserView.OfficeChoice> _offices;
        private readonly List<FormUserView.SpecializationChoice> _specializations;
        private List<FormUserView.DoctorChoice> _visibleDoctors = new();

        private readonly ComboBox _comboBoxPatient = new();
        private readonly ComboBox _comboBoxSpecialization = new();
        private readonly ComboBox _comboBoxDoctor = new();
        private readonly DateTimePicker _dateTimePickerDate = new();
        private readonly DateTimePicker _dateTimePickerTime = new();
        private readonly ComboBox _comboBoxOffice = new();
        private readonly TextBox _textBoxStatus = new();
        private readonly Button _buttonSave = new();
        private readonly Button _buttonClose = new();
        private readonly UserService _userService;

        public (bool success, string message) result = (false, "Nie zapisano wizyty.");
        public DateTime visitTime = DateTime.MinValue;

        internal (int PatientId, int DoctorId, int OfficeId, DateTime StartDateTime)? CreatedVisit { get; private set; }

        internal FormVisitEditor(
            List<FormUserView.PatientChoice> patients,
            List<FormUserView.DoctorChoice> doctors,
            List<FormUserView.OfficeChoice> offices,
            List<FormUserView.SpecializationChoice> specializations,
            UserService service)
        {
            _patients = patients;
            _doctors = doctors;
            _offices = offices;
            _specializations = specializations;
            _userService = service;

            InitializeVisitEditor();
            Load += FormVisitEditor_Load;
            
        }

        private void InitializeVisitEditor()
        {
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(760, 430);
            MinimumSize = new Size(760, 430);
            Text = "Rejestracja wizyty";

            var tableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 8,
                Padding = new Padding(16)
            };

            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            for (int i = 0; i < 7; i++)
            {
                tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            }

            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            ConfigureComboBox(_comboBoxPatient);
            ConfigureComboBox(_comboBoxSpecialization);
            ConfigureComboBox(_comboBoxDoctor);
            ConfigureComboBox(_comboBoxOffice);
            _comboBoxSpecialization.SelectedIndexChanged += (_, _) => PopulateDoctorsForSelectedSpecialization();

            _dateTimePickerDate.Format = DateTimePickerFormat.Custom;
            _dateTimePickerDate.CustomFormat = "yyyy-MM-dd";
            _dateTimePickerDate.Dock = DockStyle.Fill;

            _dateTimePickerTime.Format = DateTimePickerFormat.Custom;
            _dateTimePickerTime.CustomFormat = "HH:mm";
            _dateTimePickerTime.ShowUpDown = true;
            _dateTimePickerTime.Dock = DockStyle.Fill;

            _textBoxStatus.Dock = DockStyle.Fill;
            _textBoxStatus.ReadOnly = true;
            _textBoxStatus.Text = "Zarejestrowana";

            AddField(tableLayout, 0, "Pacjent", _comboBoxPatient);
            AddField(tableLayout, 1, "Specjalizacja", _comboBoxSpecialization);
            AddField(tableLayout, 2, "Lekarz", _comboBoxDoctor);
            AddField(tableLayout, 3, "Data wizyty", _dateTimePickerDate);
            AddField(tableLayout, 4, "Godzina rozpoczęcia", _dateTimePickerTime);
            AddField(tableLayout, 5, "Gabinet", _comboBoxOffice);
            AddField(tableLayout, 6, "Status", _textBoxStatus);

            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 12, 0, 0)
            };

            _buttonClose.Text = "Zamknij";
            _buttonClose.AutoSize = true;
            _buttonClose.Click += (_, _) => Close();

            _buttonSave.Text = "Potwierdź zapis";
            _buttonSave.AutoSize = true;
            _buttonSave.Click += ButtonSave_Click;

            buttonPanel.Controls.Add(_buttonClose);
            buttonPanel.Controls.Add(_buttonSave);

            tableLayout.Controls.Add(buttonPanel, 0, 7);
            tableLayout.SetColumnSpan(buttonPanel, 2);

            Controls.Add(tableLayout);
        }

        private void FormVisitEditor_Load(object? sender, EventArgs e)
        {
            _comboBoxPatient.Items.Clear();
            _comboBoxPatient.Items.AddRange(_patients
                .Select(p => p.DisplayName)
                .Cast<object>()
                .ToArray());

            _comboBoxSpecialization.Items.Clear();
            _comboBoxSpecialization.Items.AddRange(_specializations
                .Select(s => s.DisplayName)
                .Cast<object>()
                .ToArray());

            _comboBoxOffice.Items.Clear();
            _comboBoxOffice.Items.AddRange(_offices
                .Select(o => o.DisplayName)
                .Cast<object>()
                .ToArray());

            if (_comboBoxPatient.Items.Count > 0)
            {
                _comboBoxPatient.SelectedIndex = 0;
            }

            if (_comboBoxSpecialization.Items.Count > 0)
            {
                _comboBoxSpecialization.SelectedIndex = 0;
            }
            else
            {
                PopulateDoctorsForSelectedSpecialization();
            }

            if (_comboBoxOffice.Items.Count > 0)
            {
                _comboBoxOffice.SelectedIndex = 0;
            }

            _dateTimePickerTime.Value = DateTime.Today.AddHours(8);
        }

        private void ButtonSave_Click(object? sender, EventArgs e)
        {
            if (_comboBoxPatient.SelectedIndex < 0)
            {
                MessageBox.Show("Wybierz pacjenta.", "Błąd walidacji", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_comboBoxSpecialization.SelectedIndex < 0)
            {
                MessageBox.Show("Wybierz specjalizację.", "Błąd walidacji", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_comboBoxDoctor.SelectedIndex < 0)
            {
                MessageBox.Show("Wybierz lekarza.", "Błąd walidacji", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_comboBoxOffice.SelectedIndex < 0)
            {
                MessageBox.Show("Wybierz gabinet.", "Błąd walidacji", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var patient = _patients.First(p => p.DisplayName == _comboBoxPatient.Text);
            var doctor = _visibleDoctors.First(d => d.DisplayName == _comboBoxDoctor.Text);
            var office = _offices.First(o => o.DisplayName == _comboBoxOffice.Text);
            var service = _userService;

            CreatedVisit = (
                patient.Id,
                doctor.Id,
                office.Id,
                _dateTimePickerDate.Value.Date.Add(_dateTimePickerTime.Value.TimeOfDay));

            
            result = service.AddWizyta(
                    patient.Id,
                    doctor.Id,
                    office.Id,
                    _dateTimePickerDate.Value.Date.Add(_dateTimePickerTime.Value.TimeOfDay));

            MessageBox.Show(
                result.message,
                result.success ? "Informacja" : "Błąd",
                MessageBoxButtons.OK,
                result.success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (result.success)
            {
                visitTime = _dateTimePickerDate.Value.Date.Add(_dateTimePickerTime.Value.TimeOfDay);
                Close();
            }
        }

        private void PopulateDoctorsForSelectedSpecialization()
        {
            FormUserView.SpecializationChoice? selectedSpecialization = _specializations
                .FirstOrDefault(s => s.DisplayName == _comboBoxSpecialization.Text);

            _visibleDoctors = selectedSpecialization == null
                ? new List<FormUserView.DoctorChoice>()
                : _doctors
                    .Where(d => d.SpecializationIds.Contains(selectedSpecialization.Id))
                    .OrderBy(d => d.DisplayName)
                    .ToList();

            _comboBoxDoctor.Items.Clear();
            _comboBoxDoctor.Items.AddRange(_visibleDoctors
                .Select(d => d.DisplayName)
                .Cast<object>()
                .ToArray());

            if (_comboBoxDoctor.Items.Count > 0)
            {
                _comboBoxDoctor.SelectedIndex = 0;
            }
        }

        private static void ConfigureComboBox(ComboBox comboBox)
        {
            comboBox.Dock = DockStyle.Fill;
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private static void AddField(TableLayoutPanel tableLayout, int rowIndex, string labelText, Control control)
        {
            var label = new Label
            {
                Text = labelText,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            tableLayout.Controls.Add(label, 0, rowIndex);
            tableLayout.Controls.Add(control, 1, rowIndex);
        }
    }
}
