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
MySql.Data.MySqlClient.MySqlConnection conn;
string myConnectionString;
myConnectionString = "server=api.uniassist.hu;uid=QuickServe;" + "pwd=Csütörtök8;database=QuickServe";
try
{
    conn = new MySql.Data.MySqlClient.MySqlConnection();
    conn.ConnectionString = myConnectionString;
    conn.Open();
    MessageBox.Show("Ok");
}
catch (MySql.Data.MySqlClient.MySqlException ex)
{
    MessageBox.Show(ex.Message);
}*/

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
            frissit();
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

        private bool frissit()
        {
            string lekeres= "SELECT * FROM Rendeles order by v_Datum";
            
            MySql.Data.MySqlClient.MySqlConnection conn;
            string myConnectionString;
            myConnectionString = "server=api.uniassist.hu;uid=QuickServe;" + "pwd=Csütörtök8;database=QuickServe";
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(lekeres, conn);
                MySql.Data.MySqlClient.MySqlDataReader rdr = cmd.ExecuteReader();
                lbFolyamatban.Items.Clear();
                lbElkeszult.Items.Clear();
                lbKiszallitott.Items.Clear();
                while (rdr.Read())
                {
                    if(rdr[5].ToString()=="1") lbFolyamatban.Items.Add(rdr[0].ToString() + " - " + rdr[1].ToString() + " - " + rdr[2].ToString()+" - "+ rdr[3].ToString());
                    else if (rdr[5].ToString() == "2") lbElkeszult.Items.Add(rdr[0].ToString() + " - " + rdr[1].ToString() + " - " + rdr[2].ToString() + " - " + rdr[3].ToString());
                    else if (rdr[5].ToString() == "3") lbKiszallitott.Items.Add(rdr[0].ToString() + " - " + rdr[1].ToString() + " - " + rdr[2].ToString() + " - " + rdr[3].ToString());
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
            if(((ListBox)sender).SelectedItem == null) return;
            string ideigl=((ListBox)sender).SelectedItem.ToString();
            int id = int.Parse(ideigl[0].ToString());
            
            string parancs = "";

            MySql.Data.MySqlClient.MySqlConnection conn;
            string connect;
            connect = "server=api.uniassist.hu;uid=QuickServe;pwd=Csütörtök8;database=QuickServe";
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = connect;
                conn.Open();
                if (ideigl[5] == '3')
                {
                    parancs= $"DELETE FROM Rendeles WHERE v_ID={id}";
                }
                else
                {
                    parancs = $"UPDATE Rendeles SET v_Allapot=v_Allapot+1 WHERE r_ID={id}";
                }

                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(parancs, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            MessageBox.Show(id.ToString());
            frissit();
        }
    }

    
}