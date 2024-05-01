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
    public partial class Vendeg : Window
    {
        public Vendeg()
        {
            InitializeComponent();
        }

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

        private void btUjrendeles1(object sender, RoutedEventArgs e)
        {
            lbRendelesAdatai.Items.Add(lEtel1.Content);
        }

        private void btUjrendeles2(object sender, RoutedEventArgs e)
        {
            lbRendelesAdatai.Items.Add(lEtel2.Content);
        }

        private void btUjrendeles3(object sender, RoutedEventArgs e)
        {
            lbRendelesAdatai.Items.Add(lEtel3.Content);
        }

        private void btRendelesLead(object sender, RoutedEventArgs e)
        {
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
                MessageBox.Show(rendelesString, "Rendelés", MessageBoxButton.OK, MessageBoxImage.Information); 
            }
            lbRendelesAdatai.Items.Clear();

        }
    }
}
