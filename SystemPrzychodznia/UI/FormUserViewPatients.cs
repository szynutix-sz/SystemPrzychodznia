using SystemPrzychodznia.Data;

namespace SystemPrzychodznia
{
    public partial class FormUserView
    {
        private readonly BindingSource _bindingSourcePatients = new();

        private TabPage? tabPagePatients;
        private GroupBox? groupBoxPatients;
        private DataGridView? dgvPatients;
        private TextBox? textBoxPatientFirstName;
        private TextBox? textBoxPatientLastName;
        private TextBox? textBoxPatientPESEL;
        private TextBox? textBoxPatientAddress;
        private TextBox? textBoxPatientPhone;
        private Button? buttonSearchPatients;
        private Button? buttonClearPatients;
        private Button? buttonAddPatient;

        private void EnsurePatientModuleVisible()
        {
            EnsurePatientTabInitialized();

            if (!tabControlUserView.TabPages.Contains(tabPagePatients))
            {
                tabControlUserView.TabPages.Add(tabPagePatients!);
                LoadPatients();
            }
        }

        private void EnsurePatientTabInitialized()
        {
            if (tabPagePatients != null)
            {
                return;
            }

            tabPagePatients = new TabPage
            {
                Name = "tabPagePatients",
                Text = "Pacjenci",
                UseVisualStyleBackColor = true
            };

            var panelPatientsSearch = new Panel
            {
                Dock = DockStyle.Top,
                Height = 106
            };

            buttonAddPatient = new Button
            {
                Text = "Zarejestruj pacjenta"
            };
            buttonAddPatient.Click += ButtonAddPatient_Click;

            buttonClearPatients = new Button
            {
                Text = "Wyczyść"
            };
            buttonClearPatients.Click += ButtonClearPatients_Click;

            buttonSearchPatients = new Button
            {
                Text = "Wyszukaj"
            };
            buttonSearchPatients.Click += ButtonSearchPatients_Click;

            textBoxPatientFirstName = CreatePatientTextBox();
            textBoxPatientLastName = CreatePatientTextBox();
            textBoxPatientPESEL = CreatePatientTextBox();
            textBoxPatientAddress = CreatePatientTextBox();
            textBoxPatientPhone = CreatePatientTextBox();

            var searchLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 7,
                RowCount = 2,
                BackColor = Color.Transparent,
                Padding = new Padding(6, 8, 6, 0)
            };

            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 185F));
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 138F));

            searchLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            searchLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));

            searchLayout.Controls.Add(CreatePatientActionHost(buttonAddPatient), 0, 0);
            searchLayout.SetRowSpan(searchLayout.GetControlFromPosition(0, 0)!, 2);

            searchLayout.Controls.Add(CreatePatientLabel("Imie"), 1, 0);
            searchLayout.Controls.Add(CreatePatientLabel("Nazwisko"), 2, 0);
            searchLayout.Controls.Add(CreatePatientLabel("PESEL"), 3, 0);
            searchLayout.Controls.Add(CreatePatientLabel("Adres zamieszkania"), 4, 0);
            searchLayout.Controls.Add(CreatePatientLabel("Numer telefonu"), 5, 0);

            searchLayout.Controls.Add(textBoxPatientFirstName, 1, 1);
            searchLayout.Controls.Add(textBoxPatientLastName, 2, 1);
            searchLayout.Controls.Add(textBoxPatientPESEL, 3, 1);
            searchLayout.Controls.Add(textBoxPatientAddress, 4, 1);
            searchLayout.Controls.Add(textBoxPatientPhone, 5, 1);

            var actionsLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1
            };
            actionsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            actionsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            actionsLayout.Controls.Add(buttonClearPatients, 0, 0);
            actionsLayout.Controls.Add(buttonSearchPatients, 0, 1);

            searchLayout.Controls.Add(actionsLayout, 6, 0);
            searchLayout.SetRowSpan(actionsLayout, 2);

            panelPatientsSearch.Controls.Add(searchLayout);

            dgvPatients = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false
            };
            dgvPatients.CellDoubleClick += DgvPatients_CellDoubleClick;

            groupBoxPatients = new GroupBox
            {
                Dock = DockStyle.Fill,
                Text = "Lista pacjentow"
            };
            groupBoxPatients.Controls.Add(dgvPatients);

            tabPagePatients.Controls.Add(groupBoxPatients);
            tabPagePatients.Controls.Add(panelPatientsSearch);
        }

        private static TextBox CreatePatientTextBox()
        {
            return new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Margin = new Padding(6, 0, 6, 0)
            };
        }

        private static Label CreatePatientLabel(string text)
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

        private static Control CreatePatientActionHost(Control control)
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

        private void LoadPatients(bool showNoResults = false)
        {
            if (textBoxPatientFirstName == null || textBoxPatientLastName == null || textBoxPatientPESEL == null || textBoxPatientAddress == null || textBoxPatientPhone == null || dgvPatients == null || groupBoxPatients == null)
            {
                return;
            }

            var searchTerms = new SearchTermsPatient
            {
                FirstName = textBoxPatientFirstName.Text.Trim(),
                LastName = textBoxPatientLastName.Text.Trim(),
                PESEL = textBoxPatientPESEL.Text.Trim(),
                Address = textBoxPatientAddress.Text.Trim(),
                Phone = textBoxPatientPhone.Text.Trim()
            };

            var patients = _userService.GetListPatients(searchTerms);
            _bindingSourcePatients.DataSource = patients;
            dgvPatients.DataSource = _bindingSourcePatients;

            if (dgvPatients.Columns.Contains("Id"))
            {
                dgvPatients.Columns["Id"].HeaderText = "ID";
                dgvPatients.Columns["Id"].Width = 70;
            }

            if (dgvPatients.Columns.Contains("FullName"))
            {
                dgvPatients.Columns["FullName"].HeaderText = "Imie i nazwisko";
                dgvPatients.Columns["FullName"].Width = 250;
            }

            if (dgvPatients.Columns.Contains("PESEL"))
            {
                dgvPatients.Columns["PESEL"].HeaderText = "PESEL";
                dgvPatients.Columns["PESEL"].Width = 130;
            }

            if (dgvPatients.Columns.Contains("Address"))
            {
                dgvPatients.Columns["Address"].HeaderText = "Adres zamieszkania";
                dgvPatients.Columns["Address"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (dgvPatients.Columns.Contains("Phone"))
            {
                dgvPatients.Columns["Phone"].HeaderText = "Numer telefonu";
                dgvPatients.Columns["Phone"].Width = 140;
            }

            if (dgvPatients.Columns.Contains("Email"))
            {
                dgvPatients.Columns["Email"].HeaderText = "Adres e-mail";
                dgvPatients.Columns["Email"].Width = 220;
            }

            if (dgvPatients.Columns.Contains("BirthDate"))
            {
                dgvPatients.Columns["BirthDate"].HeaderText = "Data urodzenia";
                dgvPatients.Columns["BirthDate"].Width = 130;
            }

            dgvPatients.RowsDefaultCellStyle.BackColor = Color.White;
            dgvPatients.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;

            if (showNoResults && patients.Count == 0)
            {
                MessageBox.Show("Nie znaleziono pacjentow spelniajacych kryteria wyszukiwania.", "Brak wynikow", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ClearPatientSearchFields()
        {
            if (textBoxPatientFirstName == null || textBoxPatientLastName == null || textBoxPatientPESEL == null || textBoxPatientAddress == null || textBoxPatientPhone == null || groupBoxPatients == null)
            {
                return;
            }

            textBoxPatientFirstName.Text = string.Empty;
            textBoxPatientLastName.Text = string.Empty;
            textBoxPatientPESEL.Text = string.Empty;
            textBoxPatientAddress.Text = string.Empty;
            textBoxPatientPhone.Text = string.Empty;
            groupBoxPatients.Text = "Lista pacjentow";
            LoadPatients();
        }

        private void ButtonSearchPatients_Click(object? sender, EventArgs e)
        {
            if (groupBoxPatients != null)
            {
                groupBoxPatients.Text = "Lista pacjentow (przefiltrowana)";
            }

            LoadPatients(showNoResults: true);
        }

        private void ButtonClearPatients_Click(object? sender, EventArgs e)
        {
            ClearPatientSearchFields();
        }

        private void ButtonAddPatient_Click(object? sender, EventArgs e)
        {
            using var form = new FormPatientEditor(_userService);
            form.ShowDialog(this);
            LoadPatients();
        }

        private void DgvPatients_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            var selectedPatient = (PatientListItem)_bindingSourcePatients[e.RowIndex];
            using var form = new FormPatientEditor(_userService, selectedPatient.Id);
            form.ShowDialog(this);
            LoadPatients();
        }
    }
}
