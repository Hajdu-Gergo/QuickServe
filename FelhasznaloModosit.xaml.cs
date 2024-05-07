using MySql.Data.MySqlClient;
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

namespace QuickServe
{
    /// <summary>
    /// Interaction logic for FelhasznaloModosit.xaml
    /// </summary>
    public partial class FelhasznaloModosit : Window
    
    {
        public string Email { get; set; }
        public string Név { get; set; }
        public int Asztalszám { get; set; }
        public FelhasznaloModosit(string email)
        {
            InitializeComponent();
            Email = email;


            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection("server=api.uniassist.hu;uid=QuickServe;pwd=Csütörtök8;database=QuickServe");
                conn.Open();

                string query = "SELECT v_Nev, v_Asztalszam FROM Vendeg WHERE v_Email = @Email";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", Email);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Név = reader["v_Nev"].ToString();
                    Asztalszám = int.Parse(reader["v_Asztalszam"].ToString());
                }

                reader.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Hiba az adatbázis-kapcsolatban: " + ex.Message, "Adatbázis hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                conn?.Close();
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }
        private void Mentes(object sender, RoutedEventArgs e)
        {

            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection("server=api.uniassist.hu;uid=QuickServe;pwd=Csütörtök8;database=QuickServe");
                conn.Open();

                string query = "UPDATE Vendeg SET v_Nev = @Nev, v_Asztalszam = @Asztalszam WHERE v_Email = @Email";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Nev", Név);
                cmd.Parameters.AddWithValue("@Asztalszam", Asztalszám);
                cmd.ExecuteNonQuery();

                MessageBox.Show("A beállítások mentve!", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Hiba az adatbázis-kapcsolatban: " + ex.Message, "Adatbázis hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                conn?.Close();
            }
        }
    }
}
