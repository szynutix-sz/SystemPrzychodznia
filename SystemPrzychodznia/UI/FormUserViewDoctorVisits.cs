using SystemPrzychodznia.Data;

namespace SystemPrzychodznia
{
    public partial class FormUserView
    {
        private readonly BindingSource _bindingSourceDoctorVisits = new();

        private TabPage? tabPageDoctorVisits;
        private GroupBox? groupBoxDoctorVisits;
        private DataGridView? dgvDoctorVisits;
        private TextBox? textBoxDoctorVisitFirstName;
        private TextBox? textBoxDoctorVisitLastName;
        private MaskedTextBox? textBoxDoctorVisitPESEL;
        private DateTimePicker? dateTimePickerDoctorVisitFrom;
        private DateTimePicker? dateTimePickerDoctorVisitTo;
        private Button? buttonSearchDoctorVisits;
        private Button? buttonClearDoctorVisits;

        private List<PatientChoice> _doctorVisitPatients = new();

        private void EnsureDoctorVisitsModuleVisible()
        {
            EnsureDoctorVisitsTabInitialized();

            var page = tabPageDoctorVisits!;
            if (!tabControlUserView.TabPages.Contains(page))
            {
                tabControlUserView.TabPages.Add(page);
                LoadDoctorVisitLookups();
                LoadDoctorVisits();
            }
        }

        private void EnsureDoctorVisitsTabInitialized()
        {
            if (tabPageDoctorVisits != null)
            {
                return;
            }

            tabPageDoctorVisits = new TabPage
            {
                Name = "tabPageDoctorVisits",
                Text = "Twoje Wizyty",
                UseVisualStyleBackColor = true
            };

            var panelSearch = new Panel
            {
                Dock = DockStyle.Top,
                Height = 106
            };

            textBoxDoctorVisitFirstName = CreateDoctorVisitTextBox();
            textBoxDoctorVisitLastName = CreateDoctorVisitTextBox();
            textBoxDoctorVisitPESEL = CreateDoctorVisitMaskedTextBox("00000000000");

            dateTimePickerDoctorVisitFrom = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy-MM-dd",
                ShowCheckBox = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Margin = new Padding(6, 0, 6, 0)
            };

            dateTimePickerDoctorVisitTo = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy-MM-dd",
                ShowCheckBox = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Margin = new Padding(6, 0, 6, 0)
            };

            buttonClearDoctorVisits = new Button
            {
                Text = "Wyczyść"
            };
            buttonClearDoctorVisits.Click += ButtonClearDoctorVisits_Click;

            buttonSearchDoctorVisits = new Button
            {
                Text = "Wyszukaj"
            };
            buttonSearchDoctorVisits.Click += ButtonSearchDoctorVisits_Click;

            var searchLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 6,
                RowCount = 2,
                BackColor = Color.Transparent,
                Padding = new Padding(6, 8, 6, 0)
            };

            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 138F));
            searchLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            searchLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));

            searchLayout.Controls.Add(CreateDoctorVisitLabel("Imię"), 0, 0);
            searchLayout.Controls.Add(CreateDoctorVisitLabel("Nazwisko"), 1, 0);
            searchLayout.Controls.Add(CreateDoctorVisitLabel("PESEL"), 2, 0);
            searchLayout.Controls.Add(CreateDoctorVisitLabel("Data od"), 3, 0);
            searchLayout.Controls.Add(CreateDoctorVisitLabel("Data do"), 4, 0);

            searchLayout.Controls.Add(textBoxDoctorVisitFirstName, 0, 1);
            searchLayout.Controls.Add(textBoxDoctorVisitLastName, 1, 1);
            searchLayout.Controls.Add(textBoxDoctorVisitPESEL, 2, 1);
            searchLayout.Controls.Add(dateTimePickerDoctorVisitFrom, 3, 1);
            searchLayout.Controls.Add(dateTimePickerDoctorVisitTo, 4, 1);

            var actionsLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1
            };
            actionsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            actionsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            actionsLayout.Controls.Add(buttonClearDoctorVisits, 0, 0);
            actionsLayout.Controls.Add(buttonSearchDoctorVisits, 0, 1);

            searchLayout.Controls.Add(actionsLayout, 5, 0);
            searchLayout.SetRowSpan(actionsLayout, 2);

            panelSearch.Controls.Add(searchLayout);

            dgvDoctorVisits = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false
            };
            dgvDoctorVisits.CellDoubleClick += DgvDoctorVisits_CellDoubleClick;

            groupBoxDoctorVisits = new GroupBox
            {
                Dock = DockStyle.Fill,
                Text = "Lista twoich wizyt"
            };
            groupBoxDoctorVisits.Controls.Add(dgvDoctorVisits);

            tabPageDoctorVisits.Controls.Add(groupBoxDoctorVisits);
            tabPageDoctorVisits.Controls.Add(panelSearch);
        }

        private void LoadDoctorVisitLookups()
        {
            _doctorVisitPatients = _userService.GetListPatients(new SearchTermsPatient())
                .Select(p => new PatientChoice
                {
                    Id = p.Id,
                    FullName = p.FullName,
                    PESEL = p.PESEL
                })
                .ToList();
        }

        private void LoadDoctorVisits(bool showNoResults = false)
        {
            if (textBoxDoctorVisitFirstName == null || textBoxDoctorVisitLastName == null ||
                textBoxDoctorVisitPESEL == null ||
                dateTimePickerDoctorVisitFrom == null || dateTimePickerDoctorVisitTo == null ||
                dgvDoctorVisits == null || groupBoxDoctorVisits == null)
            {
                return;
            }

            var searchTerms = new SearchTermsWizyta
            {
                IdLekarza = _currentUser.Id,
                DataOd = dateTimePickerDoctorVisitFrom.Checked ? dateTimePickerDoctorVisitFrom.Value.Date : null,
                DataDo = dateTimePickerDoctorVisitTo.Checked ? dateTimePickerDoctorVisitTo.Value.Date : null
            };

            var patientById = _doctorVisitPatients.ToDictionary(p => p.Id);
            var rows = _userService.GetWizyty(searchTerms)
                .Select(v =>
                {
                    patientById.TryGetValue(v.IdPacjenta, out var patient);
                    var (firstName, lastName) = SplitPatientName(v.NazwaPacjenta);
                    return new DoctorVisitListItem
                    {
                        VisitId = v.Id,
                        PatientFirstName = firstName,
                        PatientLastName = lastName,
                        PatientPESEL = patient?.PESEL ?? string.Empty,
                        StartDateTime = v.DataRozpoczecia,
                        Office = v.NazwaGabinetu,
                        Status = v.Status,
                        Illnesses = v.Schorzenia ?? string.Empty,
                        Recommendations = v.Zalecenia ?? string.Empty
                    };
                });

            string firstNameFilter = textBoxDoctorVisitFirstName.Text.Trim();
            if (!string.IsNullOrWhiteSpace(firstNameFilter))
            {
                rows = rows.Where(v => v.PatientFirstName.Contains(firstNameFilter, StringComparison.OrdinalIgnoreCase));
            }

            string lastNameFilter = textBoxDoctorVisitLastName.Text.Trim();
            if (!string.IsNullOrWhiteSpace(lastNameFilter))
            {
                rows = rows.Where(v => v.PatientLastName.Contains(lastNameFilter, StringComparison.OrdinalIgnoreCase));
            }

            string peselFilter = textBoxDoctorVisitPESEL.Text.Trim();
            if (!string.IsNullOrWhiteSpace(peselFilter))
            {
                rows = rows.Where(v => v.PatientPESEL.Contains(peselFilter, StringComparison.OrdinalIgnoreCase));
            }

            var filteredRows = rows
                .OrderBy(v => v.StartDateTime)
                .ToList();

            _bindingSourceDoctorVisits.DataSource = filteredRows;
            dgvDoctorVisits.DataSource = _bindingSourceDoctorVisits;
            ConfigureDoctorVisitsGrid();

            if (showNoResults && filteredRows.Count == 0)
            {
                MessageBox.Show("Nie znaleziono twoich wizyt spełniających kryteria wyszukiwania.", "Brak wyników", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ConfigureDoctorVisitsGrid()
        {
            if (dgvDoctorVisits == null)
            {
                return;
            }


            if (dgvDoctorVisits.Columns.Contains("VisitId"))
            {
                dgvDoctorVisits.Columns["VisitId"]!.Visible = false;
            }

            if (dgvDoctorVisits.Columns.Contains("PatientFirstName"))
            {
                dgvDoctorVisits.Columns["PatientFirstName"]!.HeaderText = "Imię";
                dgvDoctorVisits.Columns["PatientFirstName"]!.Width = 150;
            }

            if (dgvDoctorVisits.Columns.Contains("PatientLastName"))
            {
                dgvDoctorVisits.Columns["PatientLastName"]!.HeaderText = "Nazwisko";
                dgvDoctorVisits.Columns["PatientLastName"]!.Width = 180;
            }

            if (dgvDoctorVisits.Columns.Contains("PatientPESEL"))
            {
                dgvDoctorVisits.Columns["PatientPESEL"]!.HeaderText = "PESEL";
                dgvDoctorVisits.Columns["PatientPESEL"]!.Width = 130;
            }

            if (dgvDoctorVisits.Columns.Contains("StartDateTime"))
            {
                dgvDoctorVisits.Columns["StartDateTime"]!.HeaderText = "Termin";
                dgvDoctorVisits.Columns["StartDateTime"]!.Width = 150;
                dgvDoctorVisits.Columns["StartDateTime"]!.DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
            }

            if (dgvDoctorVisits.Columns.Contains("Office"))
            {
                dgvDoctorVisits.Columns["Office"]!.HeaderText = "Gabinet";
                dgvDoctorVisits.Columns["Office"]!.Width = 140;
            }

            if (dgvDoctorVisits.Columns.Contains("Status"))
            {
                dgvDoctorVisits.Columns["Status"]!.HeaderText = "Status";
                dgvDoctorVisits.Columns["Status"]!.Width = 140;
            }

            if (dgvDoctorVisits.Columns.Contains("Illnesses"))
            {
                dgvDoctorVisits.Columns["Illnesses"]!.HeaderText = "Schorzenia i dolegliwości";
                dgvDoctorVisits.Columns["Illnesses"]!.Width = 240;
            }

            if (dgvDoctorVisits.Columns.Contains("Recommendations"))
            {
                dgvDoctorVisits.Columns["Recommendations"]!.HeaderText = "Zalecenia i lekarstwa";
                dgvDoctorVisits.Columns["Recommendations"]!.Width = 240;
            }

            dgvDoctorVisits.RowsDefaultCellStyle.BackColor = Color.White;
            dgvDoctorVisits.AlternatingRowsDefaultCellStyle.BackColor = Color.Honeydew;
        }

        private void ClearDoctorVisitSearchFields()
        {
            if (textBoxDoctorVisitFirstName == null || textBoxDoctorVisitLastName == null ||
                textBoxDoctorVisitPESEL == null ||
                dateTimePickerDoctorVisitFrom == null || dateTimePickerDoctorVisitTo == null ||
                groupBoxDoctorVisits == null)
            {
                return;
            }

            textBoxDoctorVisitFirstName.Text = string.Empty;
            textBoxDoctorVisitLastName.Text = string.Empty;
            textBoxDoctorVisitPESEL.Text = string.Empty;
            dateTimePickerDoctorVisitFrom.Checked = false;
            dateTimePickerDoctorVisitTo.Checked = false;
            groupBoxDoctorVisits.Text = "Lista twoich wizyt";
            LoadDoctorVisits();
        }

        private void ButtonSearchDoctorVisits_Click(object? sender, EventArgs e)
        {
            if (groupBoxDoctorVisits != null)
            {
                groupBoxDoctorVisits.Text = "Lista twoich wizyt (przefiltrowana)";
            }

            LoadDoctorVisits(showNoResults: true);
        }

        private void ButtonClearDoctorVisits_Click(object? sender, EventArgs e)
        {
            ClearDoctorVisitSearchFields();
        }

        private void DgvDoctorVisits_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _bindingSourceDoctorVisits[e.RowIndex] is not DoctorVisitListItem visit)
            {
                return;
            }

            using var form = new FormDoctorVisitResult(visit);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                _userService.ZmienStatusWizyty(visit.VisitId, "Zrealizowana", form.Illnesses, form.Recommendations);
                LoadDoctorVisits();
            }
        }

        private static TextBox CreateDoctorVisitTextBox()
        {
            return new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Margin = new Padding(6, 0, 6, 0)
            };
        }

        private static MaskedTextBox CreateDoctorVisitMaskedTextBox(string mask)
        {
            return new MaskedTextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Margin = new Padding(6, 0, 6, 0),
                Mask = mask
            };
        }

        private static (string FirstName, string LastName) SplitPatientName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                return (string.Empty, string.Empty);
            }

            string[] parts = fullName.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)
            {
                return (parts[0], string.Empty);
            }

            return (parts[0], parts[1]);
        }

        private static Label CreateDoctorVisitLabel(string text)
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

        internal sealed class DoctorVisitListItem
        {
            public int VisitId { get; set; }
            public string PatientFirstName { get; set; } = string.Empty;
            public string PatientLastName { get; set; } = string.Empty;
            public string PatientPESEL { get; set; } = string.Empty;
            public DateTime StartDateTime { get; set; }
            public string Office { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
            public string Illnesses { get; set; } = string.Empty;
            public string Recommendations { get; set; } = string.Empty;
        }
    }
}
