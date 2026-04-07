using System;
using System.Data;
using System.Windows.Forms;

namespace SystemPrzychodznia
{
    public partial class FormRoles : Form
    {
        public FormRoles()
        {
            InitializeComponent();
            WczytajRole();
        }

        private void WczytajRole()
        {
            var tabela = new DataTable();
            tabela.Columns.Add("Rola");
            tabela.Columns.Add("Uprawnienia");

            tabela.Rows.Add("Administrator", "Dodawanie użytkowników, Edycja użytkowników, Zapomnienie użytkowników, Przegląd listy użytkowników, Przegląd ról");
            tabela.Rows.Add("Lekarz", "Przegląd listy pacjentów, Podgląd danych pacjenta, Wystawianie recept");
            tabela.Rows.Add("Pielęgniarka", "Przegląd listy pacjentów, Podgląd danych pacjenta");
            tabela.Rows.Add("Rejestratorka", "Dodawanie pacjentów, Edycja danych pacjentów, Przegląd listy pacjentów");
            tabela.Rows.Add("Pacjent", "Podgląd własnych danych");

            dataGridViewRoles.DataSource = tabela;
            dataGridViewRoles.Columns["Rola"].Width = 160;
            dataGridViewRoles.Columns["Uprawnienia"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
    }
}
