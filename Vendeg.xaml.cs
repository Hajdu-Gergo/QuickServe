using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
//MySQL adatbázis kapcsolathoz package kell (MySQL.Data)
using MySql.Data;


/*
    Termékek:
    Név:          Click:          Hozzá tartozó ár:
    btHamburger - btUjrendeles1 - lbHamburgerAr
    btHotdog    - btUjrendeles2 - lbHotdogAr
    btGyros     - btUjrendeles3 - lbGyrosAr
    
    Rendelés leadása:
    btMegrendel - btRendelesLead
    lbRendelesAdatai - Rendelés adatai listbox

    Felső menü:
    - Beállítások
        - Adatbázisteszt
    - Exportálás  (Ez lehet felesleges menüpont)
    - Súgó
        - Dokumentáció
        - Elérhetőség


*/

namespace QuickServe
{
    public partial class Vendeg : Window
    {
        string rendelesSQL = "";//Insert SQL parancsok tárolására szolgál
        int vegossz = 0;//Végösszeg tárolására szolgál
        int vendegID = 1;//jelenlegi egyetlen vendeg ID-ja
        public Vendeg()
        {
            InitializeComponent();
        }

        //Ablakváltás vissza a főoldalra
        private void KiszolgaloMegnyit(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            this.Hide();
            main.Show();                   
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }


        /*
         Az alábbi osztályok a különböző termékekhez tartozó rendelés gombokat kezelik.
        Minden gombhoz tartozik egy ár label. Ez a label tartalmazza az árat, amelyet a végösszeghez hozzáadunk.
        Meghívja a Rendeles fuggvényt (erről később lesz részletes leírás)
         */
        private void btUjrendeles1(object sender, RoutedEventArgs e)
        {
            string? ar = lbHamburgerAr.Content.ToString();
            lbRendelesAdatai.Items.Add(lEtel1.Content + " - " + lbHamburgerAr.Content);
            vegossz+= int.Parse(ar.Substring(0,ar.Length-2));
            lbVegosszeg.Content = vegossz + " Ft";
            Rendeles(lEtel1.Content.ToString(), ar.Substring(0, ar.Length - 2));
        }

        private void btUjrendeles2(object sender, RoutedEventArgs e)
        {
            lbRendelesAdatai.Items.Add(lEtel2.Content+" - "+lbHotdogAr.Content);
            string? ar = lbHotdogAr.Content.ToString();
            vegossz += int.Parse(ar.Substring(0, ar.Length - 2));
            lbVegosszeg.Content = vegossz + " Ft";
            Rendeles(lEtel2.Content.ToString(), ar.Substring(0, ar.Length - 2));
        }

        private void btUjrendeles3(object sender, RoutedEventArgs e)
        {
            lbRendelesAdatai.Items.Add(lEtel3.Content + " - " + lbGyrosAr.Content);
            string? ar = lbGyrosAr.Content.ToString();
            vegossz += int.Parse(ar.Substring(0, ar.Length - 2));
            lbVegosszeg.Content = vegossz + " Ft";
            Rendeles(lEtel3.Content.ToString(), ar.Substring(0, ar.Length - 2));
        }

        private void btRendelesLead(object sender, RoutedEventArgs e)
        {/*
          Ellemőrzi, hogy van-e kiválasztott item a rendelésben.
            ha igen akkor meghívja az AdatbázisKapcsolat függvényt, amely az adatbázisba való feltöltést végzi.
            Értesíti a felhasználót.
          */
            String rendelesString = "";
            foreach (var item in lbRendelesAdatai.Items)
            {
                rendelesString += $"{item};";
            }

            if (rendelesString == "")
            {
                MessageBox.Show("Nincs kiválasztott item", "Rendelés", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Rendelés leadva!\nA pincér hamarosan érkezik az ételével.", "Sikeres megrendendelés", MessageBoxButton.OK, MessageBoxImage.Information);
                AdatbázisKapcsolat();
            }
            lbRendelesAdatai.Items.Clear();
            rendelesSQL = "";
            vegossz = 0;
            lbVegosszeg.Content = vegossz + " Ft";

        }
        private void Rendeles(string nev, string arS)
        {/*
          Integerré alakítja a kapott stringet, majd hozzáfűzi az SQL parancshoz az adott termék rendelésének hozzáadó parancsát.
          */
            int ar= int.Parse(arS);
            rendelesSQL+= $" INSERT INTO Rendeles (v_ID, v_Etel, v_Etelar, v_Allapot) VALUES (1,'{nev}', {ar}, 1);";
        }

        private void AdatbázisKapcsolat()
        {/*
          Az alábbi osztály végzi az adatbázisba való feltöltést.
            Ehhez az összefűzött stringet hasznáűlja, amely tartalmazza az összes rendelés Insert parancsát ';'-vel elválasztva.
          */
            MySql.Data.MySqlClient.MySqlConnection conn;
            string connect;
            connect = "server=api.uniassist.hu;uid=QuickServe;pwd=Csütörtök8;database=QuickServe";
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = connect;
                conn.Open();
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(rendelesSQL, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void NemImplementáltFunkció(object sender, RoutedEventArgs e)
        {
            /*
            Placeholder funkció, amelyet azok a gombok hívnak meg amelyeket nem sikerült befejezni.
            */
          
            MessageBox.Show("Ez a funkció jelenleg nem elérhető", "Nem implementált funkció", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void FelhasznaloBeallitMegnyit(object sender, RoutedEventArgs e)
        {
            var beallitasok = new FelhasznaloModosit("teszt@email.com");
            this.Hide();
            beallitasok.Show();
        }
    }
}
