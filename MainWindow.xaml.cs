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
using Google.Protobuf;

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
        const string connect = "server=localhost;uid=root;database=QuickServe";
        public MainWindow()
        {
            InitializeComponent();
            frissit();//Adatok betöltése a Listboxokba
        }

        private void VendegMegnyit(object sender, RoutedEventArgs e)//Ablakváltás
        {
            var Vendg = new Vendeg();
            this.Hide();
            Vendg.Show();
            
        }

        protected override void OnClosed(EventArgs e)//Leállítás
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private bool frissit()
        {
            /*
            Adatok lekérdezése az adatbázisból és ezeknek a ListBoxokba töltése.
            Ennek érdekében csatlakozik az adatbázishoz.

            Alapvetően 3 listbox van, ahol a rendeléseket tároljuk:
            - lbFolyamatban
            - lbElkeszult
            - lbKiszallitott

            Az adatbázisban a rendeléseknek 3 állapota lehet:
            - 1: Folyamatban
            - 2: Elkészült
            - 3: Kiszállított

            Ezt figyelembe véve osztja szét az adatokat a ListBoxok között.
            
            Először lekérdezi az adatokat dátum szerint rendezve, majd a MySQLDataReader segítségével egy elágazás eldönti, hogy mely részbe kell kerülniük.
            Ha a lekérdezés sikeres, akkor a ListBoxok előző értékeit törli és újra feltölti az új adatokkal.
            Egyébként hibaüzenetet dob.
            Tesztelési célból a metódus visszaad egy bool típusú értéket is.

             */
            string lekeres= "SELECT * FROM Rendeles order by v_Datum";
            
            MySql.Data.MySqlClient.MySqlConnection conn;
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = connect;
                conn.Open();
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(lekeres, conn);
                MySql.Data.MySqlClient.MySqlDataReader rdr = cmd.ExecuteReader();
                lbFolyamatban.Items.Clear();
                lbElkeszult.Items.Clear();
                lbKiszallitott.Items.Clear();
                while (rdr.Read())
                {
                    if(rdr[5].ToString()=="1") lbFolyamatban.Items.Add(rdr[0].ToString() + " - " + rdr[1].ToString() + " - " + rdr[2].ToString()+" - "+ rdr[3].ToString()+" Ft");
                    else if (rdr[5].ToString() == "2") lbElkeszult.Items.Add(rdr[0].ToString() + " - " + rdr[1].ToString() + " - " + rdr[2].ToString() + " - " + rdr[3].ToString() + " Ft");
                    else if (rdr[5].ToString() == "3") lbKiszallitott.Items.Add(rdr[0].ToString() + " - " + rdr[1].ToString() + " - " + rdr[2].ToString() + " - " + rdr[3].ToString() + " Ft");
                    
                }
                conn.Close();
                return true;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            

        }

        private void Adatbazisteszt(object sender, RoutedEventArgs e)
        {
            /*
            Az adatbázikapcsolat tesztelése a korábban megírt frissít() segítségével. 
            Ha a frissít() visszaadott értéke igaz, akkor az adatbázis kapcsolat megfelelő és erről tájékoztatja a felhasználót, egyébként hibaüzenetet dob.
             */ 
            if (frissit())
            {
                MessageBox.Show("Az adatbázis kapcsolat megfelelő, adatok frissítve", "Adatbázis teszt", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Az adatbázis nem elérhető", "Adatbázis teszt", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Kijelol(object sender, SelectionChangedEventArgs e)
        {
            /*
            Módosítás és törlés érdekében csatlakozik az adatbázishoz
            
            Ez a függvény teszi lehetővé, hogy a rendelések állapotát változtassuk a ListBoxokban.
            Ha a ListBoxban nincs kijelölt elem, akkor nem történik semmi.
            Ha van kijelölt elem, akkor annak az állapotát fogja növelni. Azaz, ha a rendelés folyamatban van, akkor elkészültté teszi, ha elkészült, akkor kiszállítottá.
            Ha a rendelés kiszállított, akkor törli a rendelést az adatbázisból kattintás hatására.

            Megvalósítja a rendelések állapotának növelését (UPDATE) és törlését (DELETE).
             */
            if(((ListBox)sender).SelectedItem == null) return;
            string ideigl=((ListBox)sender).SelectedItem.ToString();
            int id = int.Parse(ideigl[0].ToString());
            
            string parancs = "";

            MySql.Data.MySqlClient.MySqlConnection conn;
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = connect;
                conn.Open();
                if (ideigl[4].ToString()=="3"|| ideigl[5].ToString() == "3")
                {
                    parancs= $"DELETE FROM Rendeles WHERE r_ID={id};";
                }
                else
                {
                    parancs = $"UPDATE Rendeles SET v_Allapot=v_Allapot+1 WHERE r_ID={id}; DELETE From Rendeles where v_Allapot=4;";
                }

                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(parancs, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            frissit();
        }

        private void IndexReset(object sender, RoutedEventArgs e)
        {
            /*
            IndexReset: Az adatbázisban az autóinkrementáló érték visszaállítása 1-re.
            
            */
            MySql.Data.MySqlClient.MySqlConnection conn;
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = connect;
                conn.Open();
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand("Alter table Rendeles Auto_Increment=1", conn);
                cmd.ExecuteNonQuery();
                conn.Close();

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            /*
            Teljes Reset: Az adatbázisban az összes rendelés törlése.
             */
            MySql.Data.MySqlClient.MySqlConnection conn;
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = connect;
                conn.Open();
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand("delete from Rendeles", conn);
                cmd.ExecuteNonQuery();
                conn.Close();

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    
}