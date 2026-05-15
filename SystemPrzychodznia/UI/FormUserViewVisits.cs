using SystemPrzychodznia.Data;

namespace SystemPrzychodznia
{
    public partial class FormUserView
    {
        private readonly BindingSource _bindingSourceVisits = new();

        private TabPage? tabPageVisits;
        private GroupBox? groupBoxVisits;
        private DataGridView? dgvVisits;
        private TextBox? textBoxVisitPatient;
        private MaskedTextBox? textBoxVisitPESEL;
        private ComboBox? comboBoxVisitSpecialization;
        private ComboBox? comboBoxVisitDoctor;
        private DateTimePicker? dateTimePickerVisitFrom;
        private DateTimePicker? dateTimePickerVisitTo;
        private Button? buttonSearchVisits;
        private Button? buttonClearVisits;
        private Button? buttonAddVisit;

        private List<PatientChoice> _visitPatients = new();
        private List<DoctorChoice> _visitDoctors = new();
        private List<OfficeChoice> _visitOffices = new();

        private void EnsureVisitModuleVisible()
        {
            EnsureVisitTabInitialized();

            if (!tabControlUserView.TabPages.Contains(tabPageVisits))
            {
                tabControlUserView.TabPages.Add(tabPageVisits!);
                LoadVisitLookups();
                LoadVisits();
            }
        }

        private void EnsureVisitTabInitialized()
        {
            if (tabPageVisits != null)
            {
                return;
            }

            tabPageVisits = new TabPage
            {
                Name = "tabPageVisits",
                Text = "Wizyty",
                UseVisualStyleBackColor = true
            };

            var panelVisitsSearch = new Panel
            {
                Dock = DockStyle.Top,
                Height = 106
            };

            buttonAddVisit = new Button
            {
                Text = "Zarejestruj wizytę"
            };
            buttonAddVisit.Click += ButtonAddVisit_Click;

            buttonClearVisits = new Button
            {
                Text = "Wyczyść"
            };
            buttonClearVisits.Click += ButtonClearVisits_Click;

            buttonSearchVisits = new Button
            {
                Text = "Wyszukaj"
            };
            buttonSearchVisits.Click += ButtonSearchVisits_Click;

            textBoxVisitPatient = CreateVisitTextBox();
            textBoxVisitPESEL = CreateVisitMaskedTextBox("00000000000");
            comboBoxVisitSpecialization = CreateVisitComboBox();
            comboBoxVisitDoctor = CreateVisitComboBox();

            dateTimePickerVisitFrom = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy-MM-dd",
                ShowCheckBox = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Margin = new Padding(6, 0, 6, 0)
            };

            dateTimePickerVisitTo = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy-MM-dd",
                ShowCheckBox = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Margin = new Padding(6, 0, 6, 0)
            };

            var searchLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 8,
                RowCount = 2,
                BackColor = Color.Transparent,
                Padding = new Padding(6, 8, 6, 0)
            };

            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 185F));
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 19F));
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 138F));

            searchLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            searchLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));

            searchLayout.Controls.Add(CreateVisitActionHost(buttonAddVisit), 0, 0);
            searchLayout.SetRowSpan(searchLayout.GetControlFromPosition(0, 0)!, 2);

            searchLayout.Controls.Add(CreateVisitLabel("Pacjent"), 1, 0);
            searchLayout.Controls.Add(CreateVisitLabel("PESEL"), 2, 0);
            searchLayout.Controls.Add(CreateVisitLabel("Specjalizacja"), 3, 0);
            searchLayout.Controls.Add(CreateVisitLabel("Lekarz"), 4, 0);
            searchLayout.Controls.Add(CreateVisitLabel("Data od"), 5, 0);
            searchLayout.Controls.Add(CreateVisitLabel("Data do"), 6, 0);

            searchLayout.Controls.Add(textBoxVisitPatient, 1, 1);
            searchLayout.Controls.Add(textBoxVisitPESEL, 2, 1);
            searchLayout.Controls.Add(comboBoxVisitSpecialization, 3, 1);
            searchLayout.Controls.Add(comboBoxVisitDoctor, 4, 1);
            searchLayout.Controls.Add(dateTimePickerVisitFrom, 5, 1);
            searchLayout.Controls.Add(dateTimePickerVisitTo, 6, 1);

            var actionsLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1
            };
            actionsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            actionsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            actionsLayout.Controls.Add(buttonClearVisits, 0, 0);
            actionsLayout.Controls.Add(buttonSearchVisits, 0, 1);

            searchLayout.Controls.Add(actionsLayout, 7, 0);
            searchLayout.SetRowSpan(actionsLayout, 2);

            panelVisitsSearch.Controls.Add(searchLayout);

            dgvVisits = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false
            };
            dgvVisits.CellDoubleClick += DgvVisits_CellDoubleClick;

            groupBoxVisits = new GroupBox
            {
                Dock = DockStyle.Fill,
                Text = "Lista wizyt"
            };
            groupBoxVisits.Controls.Add(dgvVisits);

            tabPageVisits.Controls.Add(groupBoxVisits);
            tabPageVisits.Controls.Add(panelVisitsSearch);
        }

        private void LoadVisitLookups()
        {
            _visitPatients = _userService.GetListPatients(new SearchTermsPatient())
                .Select(p => new PatientChoice
                {
                    Id = p.Id,
                    FullName = p.FullName,
                    PESEL = p.PESEL
                })
                .OrderBy(p => p.FullName)
                .ToList();

            var doctors = _userService.GetUsersByRole(3);
            _visitDoctors = doctors
                .Select(doctor => new DoctorChoice
                {
                    Id = doctor.Id,
                    FullName = $"{doctor.FirstName} {doctor.LastName}".Trim(),
                    Login = doctor.Login
                })
                .OrderBy(d => d.FullName)
                .ToList();

            _visitOffices = _userService.GetGabinety()
                .Select(gabinet => new OfficeChoice
                {
                    Id = gabinet.Id,
                    Name = gabinet.Nazwa
                })
                .OrderBy(g => g.Name)
                .ToList();

            PopulateVisitSpecializationCombo();
            PopulateVisitDoctorCombo();
        }

        private void PopulateVisitSpecializationCombo()
        {
            if (comboBoxVisitSpecialization == null)
            {
                return;
            }

            comboBoxVisitSpecialization.Items.Clear();
            comboBoxVisitSpecialization.Items.Add(string.Empty);
            comboBoxVisitSpecialization.SelectedIndex = 0;
        }

        private void PopulateVisitDoctorCombo()
        {
            if (comboBoxVisitDoctor == null)
            {
                return;
            }

            string previousSelection = comboBoxVisitDoctor.Text;
            comboBoxVisitDoctor.Items.Clear();
            comboBoxVisitDoctor.Items.Add(string.Empty);
            comboBoxVisitDoctor.Items.AddRange(_visitDoctors
                .Select(d => d.DisplayName)
                .Cast<object>()
                .ToArray());

            if (comboBoxVisitDoctor.Items.Contains(previousSelection))
            {
                comboBoxVisitDoctor.SelectedItem = previousSelection;
            }
            else
            {
                comboBoxVisitDoctor.SelectedIndex = 0;
            }
        }

        private void LoadVisits(bool showNoResults = false)
        {
            if (textBoxVisitPatient == null || textBoxVisitPESEL == null || comboBoxVisitSpecialization == null ||
                comboBoxVisitDoctor == null || dateTimePickerVisitFrom == null || dateTimePickerVisitTo == null ||
                dgvVisits == null || groupBoxVisits == null)
            {
                return;
            }

            var visitRows = _userService.GetWizyty()
                .Select(v => new VisitListItem
                {
                    PatientName = v.NazwaPacjenta,
                    PatientPESEL = _visitPatients.FirstOrDefault(p => p.Id == v.IdPacjenta)?.PESEL ?? string.Empty,
                    DoctorName = v.NazwaLekarza,
                    Office = v.NazwaGabinetu,
                    StartDateTime = v.DataRozpoczecia,
                    Status = v.Status
                })
                .ToList();

            IEnumerable<VisitListItem> filtered = visitRows;

            string patient = textBoxVisitPatient.Text.Trim();
            if (!string.IsNullOrWhiteSpace(patient))
            {
                filtered = filtered.Where(v => v.PatientName.Contains(patient, StringComparison.OrdinalIgnoreCase));
            }

            string pesel = textBoxVisitPESEL.Text.Trim();
            if (!string.IsNullOrWhiteSpace(pesel))
            {
                filtered = filtered.Where(v => v.PatientPESEL.Contains(pesel, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(comboBoxVisitDoctor.Text))
            {
                filtered = filtered.Where(v => v.DoctorName == comboBoxVisitDoctor.Text);
            }

            if (dateTimePickerVisitFrom.Checked)
            {
                DateTime fromDate = dateTimePickerVisitFrom.Value.Date;
                filtered = filtered.Where(v => v.StartDateTime.Date >= fromDate);
            }

            if (dateTimePickerVisitTo.Checked)
            {
                DateTime toDate = dateTimePickerVisitTo.Value.Date;
                filtered = filtered.Where(v => v.StartDateTime.Date <= toDate);
            }

            var rows = filtered
                .OrderBy(v => v.StartDateTime)
                .ToList();

            _bindingSourceVisits.DataSource = rows;
            dgvVisits.DataSource = _bindingSourceVisits;

            ConfigureVisitsGrid();

            if (showNoResults && rows.Count == 0)
            {
                MessageBox.Show("Nie znaleziono wizyt spełniających kryteria wyszukiwania.", "Brak wyników", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ConfigureVisitsGrid()
        {
            if (dgvVisits == null)
            {
                return;
            }

            if (dgvVisits.Columns.Contains("PatientName"))
            {
                dgvVisits.Columns["PatientName"].HeaderText = "Pacjent";
                dgvVisits.Columns["PatientName"].Width = 220;
            }

            if (dgvVisits.Columns.Contains("PatientPESEL"))
            {
                dgvVisits.Columns["PatientPESEL"].HeaderText = "PESEL";
                dgvVisits.Columns["PatientPESEL"].Width = 130;
            }

            if (dgvVisits.Columns.Contains("DoctorName"))
            {
                dgvVisits.Columns["DoctorName"].HeaderText = "Lekarz";
                dgvVisits.Columns["DoctorName"].Width = 200;
            }

            if (dgvVisits.Columns.Contains("StartDateTime"))
            {
                dgvVisits.Columns["StartDateTime"].HeaderText = "Termin";
                dgvVisits.Columns["StartDateTime"].Width = 150;
                dgvVisits.Columns["StartDateTime"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
            }

            if (dgvVisits.Columns.Contains("Office"))
            {
                dgvVisits.Columns["Office"].HeaderText = "Gabinet";
                dgvVisits.Columns["Office"].Width = 120;
            }

            if (dgvVisits.Columns.Contains("Status"))
            {
                dgvVisits.Columns["Status"].HeaderText = "Status";
                dgvVisits.Columns["Status"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            dgvVisits.RowsDefaultCellStyle.BackColor = Color.White;
            dgvVisits.AlternatingRowsDefaultCellStyle.BackColor = Color.Honeydew;
        }

        private void ClearVisitSearchFields()
        {
            if (textBoxVisitPatient == null || textBoxVisitPESEL == null || comboBoxVisitSpecialization == null ||
                comboBoxVisitDoctor == null || dateTimePickerVisitFrom == null || dateTimePickerVisitTo == null ||
                groupBoxVisits == null)
            {
                return;
            }

            textBoxVisitPatient.Text = string.Empty;
            textBoxVisitPESEL.Text = string.Empty;
            comboBoxVisitSpecialization.SelectedIndex = 0;
            comboBoxVisitDoctor.SelectedIndex = 0;
            dateTimePickerVisitFrom.Checked = false;
            dateTimePickerVisitTo.Checked = false;

            groupBoxVisits.Text = "Lista wizyt";
            LoadVisits();
        }

        private void ButtonSearchVisits_Click(object? sender, EventArgs e)
        {
            if (groupBoxVisits != null)
            {
                groupBoxVisits.Text = "Lista wizyt (przefiltrowana)";
            }

            LoadVisits(showNoResults: true);
        }

        private void ButtonClearVisits_Click(object? sender, EventArgs e)
        {
            ClearVisitSearchFields();
        }

        private void ButtonAddVisit_Click(object? sender, EventArgs e)
        {
            using var form = new FormVisitEditor(_visitPatients, _visitDoctors, _visitOffices);
            if (form.ShowDialog(this) == DialogResult.OK && form.CreatedVisit != null)
            {
                var createdVisit = form.CreatedVisit.Value;
                var result = _userService.AddWizyta(
                    createdVisit.PatientId,
                    createdVisit.DoctorId,
                    createdVisit.OfficeId,
                    createdVisit.StartDateTime);

                MessageBox.Show(
                    result.message,
                    result.success ? "Informacja" : "Błąd",
                    MessageBoxButtons.OK,
                    result.success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                if (result.success)
                {
                    LoadVisits();
                }
            }
        }

        private void DgvVisits_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _bindingSourceVisits[e.RowIndex] is not VisitListItem visit)
            {
                return;
            }

            MessageBox.Show(
                $"Pacjent: {visit.PatientName}{Environment.NewLine}" +
                $"PESEL: {visit.PatientPESEL}{Environment.NewLine}" +
                $"Lekarz: {visit.DoctorName}{Environment.NewLine}" +
                $"Termin: {visit.StartDateTime:yyyy-MM-dd HH:mm}{Environment.NewLine}" +
                $"Gabinet: {visit.Office}{Environment.NewLine}" +
                $"Status: {visit.Status}",
                "Podgląd wizyty",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private static TextBox CreateVisitTextBox()
        {
            return new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Margin = new Padding(6, 0, 6, 0)
            };
        }

        private static MaskedTextBox CreateVisitMaskedTextBox(string mask)
        {
            return new MaskedTextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Margin = new Padding(6, 0, 6, 0),
                Mask = mask
            };
        }

        private static ComboBox CreateVisitComboBox()
        {
            return new ComboBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Margin = new Padding(6, 0, 6, 0),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
        }

        private static Label CreateVisitLabel(string text)
        {
            return new Label
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomLeft,
                Margin = new Padding(6, 0, 6, 3),
                Text = text
            };
        }

        private static Control CreateVisitActionHost(Control control)
        {
            var host = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 0, 10, 0)
            };

            control.Dock = DockStyle.None;
            control.Height = 42;
            control.Width = 170;
            control.Location = new Point(0, 16);
            control.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            host.Controls.Add(control);

            return host;
        }

        internal sealed class VisitListItem
        {
            public string PatientName { get; set; } = string.Empty;
            public string PatientPESEL { get; set; } = string.Empty;
            public string DoctorName { get; set; } = string.Empty;
            public DateTime StartDateTime { get; set; }
            public string Office { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
        }

        internal sealed class PatientChoice
        {
            public int Id { get; set; }
            public string FullName { get; set; } = string.Empty;
            public string PESEL { get; set; } = string.Empty;
            public string DisplayName => $"{FullName} ({PESEL})";
        }

        internal sealed class DoctorChoice
        {
            public int Id { get; set; }
            public string FullName { get; set; } = string.Empty;
            public string Login { get; set; } = string.Empty;
            public string DisplayName => FullName;
        }

        internal sealed class OfficeChoice
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string DisplayName => Name;
        }
    }
}
