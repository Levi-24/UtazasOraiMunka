using System.IO;
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

namespace Utazas
{
    public partial class MainWindow : Window
    {
        List<UtasAdat> utasok = new List<UtasAdat>();

        public MainWindow()
        {
            InitializeComponent();
            using StreamReader sr = new StreamReader(@"../../../src/utasadat.txt");
            while (!sr.EndOfStream) utasok.Add(new UtasAdat(sr.ReadLine()));

            utasListBox.ItemsSource = utasok;

            int utasCount = utasok.Count;
            TButasCount.Text = utasCount.ToString() + " utas akart felszállni a buszra.";

            int nemSzallFel = utasok.Count(u => u.Jegy == 0 && u.Datum == 0 || u.Datum < u.FelszallasDatum && u.Datum != 0);
            TButasNemSzallFel.Text = nemSzallFel.ToString() + " utas nem szállhatott fel a buszra.";

            TBLegtobbUtasMegallo.Text = "A legtöbb utas (X) a (Y). megállóban próbált felszállni";

            int kedvezmenyes = utasok.Count(u => u.Tipus == "TAB" || u.Tipus == "NYB" && (u.Datum > u.FelszallasDatum));
            int ingyenes = utasok.Count(u => u.Tipus == "GYK" || u.Tipus == "NYP" && (u.Datum > u.FelszallasDatum));
            TBKedvezmenyes.Text = kedvezmenyes.ToString() + " utas utazik kedvezményes bérlettel.";
            TBIngyenes.Text = ingyenes.ToString() + " utas utazik ingyenes bérlettel.";

            var dsa = utasok.Where(u => u.Datum - u.FelszallasDatum <= 3 && u.Datum != 0).ToList();

            using StreamWriter writer = new StreamWriter(@"../../../src/lejar.txt", false);
            foreach (var adat in dsa)
            {
                writer.Write(adat.Azonosito + " ");
                string temp = adat.Datum.ToString().Insert(4, " ");
                temp = temp.Insert(7, " ");
                writer.WriteLine(temp);
            }

            List<string> megallok = new List<string>();
            megallok.Add("Válasszon megállót!");
            for (int i = 0; i < 30; i++) megallok.Add(i.ToString());
            CBMegallo.ItemsSource = megallok;

            List<string> tipusok = new List<string>();
            tipusok.Add("Válasszon típust!");
            tipusok.AddRange(utasok.Select(u => u.Tipus).Distinct().ToList());
            CBTipus.ItemsSource = tipusok;

            DPdatum.SelectedDate = DateTime.Today;

        }
        private void TBazonositoChanged(object sender, TextChangedEventArgs e)
        {
            TBazonositoCount.Text = TBazonosito.Text.Length + "db";
        }

        private void RB_Checked(object sender, RoutedEventArgs e)
        {
            if(RBjegy.IsChecked == true)
            {
                GBberlet.Visibility = Visibility.Collapsed;
            }
            else if (RBberlet.IsChecked == true)
            {
                GBberlet.Visibility= Visibility.Visible;
            }
        }
    }
}


