using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SystemPrzychodznia.UI
{
    public partial class FormRoleFilter : Form
    {
        // Ta lista przechowa to, co użytkownik zaznaczył
        public List<string> SelectedRoles { get; private set; } = new List<string>();

        // Konstruktor przyjmujący listę wszystkich ról
        public FormRoleFilter(List<string> allRoles)
        {
            InitializeComponent();

            // Konfiguracja wyglądu okienka (Popup)
            this.Text = "Wybierz role do filtrowania";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Ładowanie ról z bazy do kontrolki na ekranie
            // UWAGA: Zakładam, że przeciągnąłeś na ekran kontrolkę CheckedListBox i nazywa się domyślnie 'checkedListBox1'
            foreach (var role in allRoles)
            {
                checkedListBox1.Items.Add(role);
            }
        }

        // Akcja pod przyciskiem "Zastosuj"
        // UWAGA: Upewnij się, że masz na ekranie przycisk (Button) i podpiąłeś pod niego to zdarzenie!
        private void btnApply_Click(object sender, EventArgs e)
        {
            // Zbieramy zaznaczone elementy
            foreach (var item in checkedListBox1.CheckedItems)
            {
                SelectedRoles.Add(item.ToString());
            }

            // Zamykamy okienko i dajemy sygnał, że wszystko OK
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}