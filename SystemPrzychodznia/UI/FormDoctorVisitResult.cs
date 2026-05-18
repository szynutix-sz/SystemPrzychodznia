namespace SystemPrzychodznia
{
    internal class FormDoctorVisitResult : Form
    {
        private readonly TextBox _textBoxIllnesses = new();
        private readonly TextBox _textBoxRecommendations = new();
        private readonly Button _buttonSave = new();
        private readonly Button _buttonClose = new();

        internal string Illnesses => _textBoxIllnesses.Text.Trim();
        internal string Recommendations => _textBoxRecommendations.Text.Trim();

        internal FormDoctorVisitResult(FormUserView.DoctorVisitListItem visit)
        {
            InitializeVisitResult(visit);
        }

        private void InitializeVisitResult(FormUserView.DoctorVisitListItem visit)
        {
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(820, 560);
            MinimumSize = new Size(760, 500);
            Text = "Rejestracja wyniku wizyty";

            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                Padding = new Padding(16)
            };

            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 84F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));

            var visitInfo = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = $"Pacjent: {visit.PatientFirstName} {visit.PatientLastName} ({visit.PatientPESEL}){Environment.NewLine}" +
                       $"Termin: {visit.StartDateTime:yyyy-MM-dd HH:mm}    Gabinet: {visit.Office}{Environment.NewLine}" +
                       $"Po zapisie status wizyty zostanie ustawiony na: Zrealizowana"
            };

            var illnessesLabel = new Label
            {
                Dock = DockStyle.Fill,
                Text = "Schorzenia i dolegliwości",
                TextAlign = ContentAlignment.BottomLeft
            };

            ConfigureMemo(_textBoxIllnesses, visit.Illnesses);
            ConfigureMemo(_textBoxRecommendations, visit.Recommendations);

            var recommendationsPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            recommendationsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            recommendationsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            recommendationsPanel.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Text = "Zalecenia i lekarstwa",
                TextAlign = ContentAlignment.BottomLeft
            }, 0, 0);
            recommendationsPanel.Controls.Add(_textBoxRecommendations, 0, 1);

            var buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 12, 0, 0)
            };

            _buttonClose.Text = "Zamknij";
            _buttonClose.AutoSize = true;
            _buttonClose.Click += (_, _) => Close();

            _buttonSave.Text = "Zapisz wynik";
            _buttonSave.AutoSize = true;
            _buttonSave.Click += ButtonSave_Click;

            buttons.Controls.Add(_buttonClose);
            buttons.Controls.Add(_buttonSave);

            mainLayout.Controls.Add(visitInfo, 0, 0);
            mainLayout.Controls.Add(illnessesLabel, 0, 1);
            mainLayout.Controls.Add(_textBoxIllnesses, 0, 2);
            mainLayout.Controls.Add(recommendationsPanel, 0, 3);
            mainLayout.Controls.Add(buttons, 0, 4);

            Controls.Add(mainLayout);
        }

        private static void ConfigureMemo(TextBox textBox, string text)
        {
            textBox.Dock = DockStyle.Fill;
            textBox.Multiline = true;
            textBox.ScrollBars = ScrollBars.Vertical;
            textBox.Text = text;
        }

        private void ButtonSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Illnesses) && string.IsNullOrWhiteSpace(Recommendations))
            {
                MessageBox.Show("Uzupełnij schorzenia, dolegliwości lub zalecenia.", "Brak danych", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
