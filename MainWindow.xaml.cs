using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//MySQL adatbázis kapcsolathoz package kell (MySQL.Data)
using MySql.Data;


/*
    ListBoxok:
    lbFolyamatban  - Folyamatban lévő rendelések
    lbElkeszult    - Elkészült rendelések
    lbKiszallitott - Kiszállított rendelések

    Felső menü:
    - Beállítások
        - Asztalok
        - Rendelések fogadása
        - Adatbázisteszt
    - Vendég nézet
    - Exportálás  (Ez lehet felesleges menüpont)
        - Minden adat exportálása
        - Rendelések exportálása
        - Biztonsági mentések
            - Naponta
            - Hetente
            - Havonta
    - Súgó
        - Dokumentáció
        - Elérhetőség
*/

namespace QuickServe
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void VendegMegnyit(object sender, RoutedEventArgs e)//Ablakváltás
        {
            var Vendg = new Vendeg();
            this.Hide();
            Vendg.Show();
            
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }
    }
}