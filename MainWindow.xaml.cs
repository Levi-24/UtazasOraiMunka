using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Utazas
{
    public partial class MainWindow : Window
    {
        List<UtasAdat> utasok = new List<UtasAdat>();

        public MainWindow()
        {
            InitializeComponent();
            UpdateGroupBoxVisibility();

            using StreamReader sr = new StreamReader(@"../../../src/utasadat.txt");
            while (!sr.EndOfStream) utasok.Add(new UtasAdat(sr.ReadLine()));

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

        private void SljegySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SLjegyDb.Text = SljegySlider.Value.ToString() + "db";
        }

        private void UpdateJegyBerlet(object sender, RoutedEventArgs e)
        {
            UpdateGroupBoxVisibility();
        }

        private void UpdateGroupBoxVisibility()
        {
            if (RBberlet != null && RBjegy != null)
            {
                if (RBberlet.IsChecked == true)
                {
                    GBberlet.Visibility = Visibility.Visible;
                    GBjegy.Visibility = Visibility.Collapsed;
                }
                else if (RBjegy.IsChecked == true)
                {
                    GBberlet.Visibility = Visibility.Collapsed;
                    GBjegy.Visibility = Visibility.Visible;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CBMegallo.SelectedIndex == 0)
            {
                MessageBox.Show("Nem választott megállót!", "Hiba!");
            }
            else
            {
                if (!DPdatum.SelectedDate.HasValue)
                {
                    MessageBox.Show("Nem adott meg dátumot!", "Hiba!");
                }
                else
                {
                    if (TBazonosito.Text.Length != 7)
                    {
                        MessageBox.Show("Az azonosító nem 7 karakter hosszú!", "Hiba!");
                    }
                    else
                    {
                        if(TBazonosito.Text.Contains(",") || Convert.ToInt32(TBazonosito.Text) < 0)
                        {
                            MessageBox.Show("A kártya azonosítója nem pozitív egész szám!", "Hiba!");
                        }
                        else
                        {
                            if (TBfelszallIdo.Text.Length != 5 && !(TBfelszallIdo.Text.Contains(":")))
                            {
                                MessageBox.Show("Nem jól adtad meg az időt!", "Hiba!");
                            }
                            else
                            {
                                if (RBberlet.IsChecked == true && CBTipus.SelectedIndex == 0)
                                {
                                    MessageBox.Show("Nem adta meg a bérelt típusát!", "Hiba!");
                                }
                                else
                                {
                                    if (!CBDatum.SelectedDate.HasValue && RBberlet.IsChecked == true)
                                    {
                                        MessageBox.Show("Nem adta meg a bérlet érvényességi idejét!", "Hiba!");
                                    }
                                    else
                                    {
                                        using StreamWriter writer = new StreamWriter(@"../../../src/utasadat.txt", true);

                                        if (RBberlet.IsChecked == true)
                                        {
                                            int megalloSorszam = Convert.ToInt32(CBMegallo.SelectedItem);

                                            DateTime felszallasDatum = DPdatum.SelectedDate.Value;
                                            string felszallasIdo = TBfelszallIdo.Text;

                                            DateTime felszallasDatumIdo = DateTime.ParseExact(felszallasDatum.ToString("yyyyMMdd") + " " + felszallasIdo, "yyyyMMdd HH:mm", null);
                                            string formattedDatumIdo = felszallasDatumIdo.ToString("yyyyMMdd-HHmm");

                                            string kartyaAzonosito = TBazonosito.Text;

                                            string berletTipus = CBTipus.SelectedItem.ToString();

                                            DateTime berletLejaratDatum = CBDatum.SelectedDate.Value;
                                            string formattedBerletLejaratDatum = berletLejaratDatum.ToString("yyyyMMdd");

                                            string formattedData = $"{megalloSorszam} {formattedDatumIdo} {kartyaAzonosito} {berletTipus} {formattedBerletLejaratDatum}";

                                            writer.WriteLine(formattedData);
                                        }
                                        else if (RBjegy.IsChecked == true)
                                        {
                                            int megalloSorszam = Convert.ToInt32(CBMegallo.SelectedItem);

                                            DateTime felszallasDatum = DPdatum.SelectedDate.Value;
                                            string felszallasIdo = TBfelszallIdo.Text;

                                            DateTime felszallasDatumIdo = DateTime.ParseExact(felszallasDatum.ToString("yyyyMMdd") + " " + felszallasIdo, "yyyyMMdd HH:mm", null);
                                            string formattedDatumIdo = felszallasDatumIdo.ToString("yyyyMMdd-HHmm");

                                            string kartyaAzonosito = TBazonosito.Text;
                                            int jegyDarab = Convert.ToInt32(SljegySlider.Value);

                                            string formattedData = $"{megalloSorszam} {formattedDatumIdo} {kartyaAzonosito} JGY {jegyDarab}";

                                            writer.WriteLine(formattedData);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Console_Button_Click(object sender, RoutedEventArgs e)
        {
            int utasCount = utasok.Count;

            int nemSzallFel = utasok.Count(u => u.Jegy == 0 && u.Datum == 0 || u.Datum < u.FelszallasDatum && u.Datum != 0);

            var stopCounts = utasok
                .GroupBy(a => a.MegalloSorszam)
                .Select(group => new
                {
                    MegalloSorszam = group.Key,
                    Count = group.Count()
                })
                .OrderByDescending(result => result.Count)
                .FirstOrDefault();

            int kedvezmenyes = utasok.Count(u => u.Tipus == "TAB" || u.Tipus == "NYB" && (u.Datum > u.FelszallasDatum));
            int ingyenes = utasok.Count(u => u.Tipus == "GYK" || u.Tipus == "NYP" || u.Tipus == "RVS" && (u.Datum > u.FelszallasDatum));

            MessageBox.Show($"A buszra {utasCount} utas akart felszállni. \nA buszra {nemSzallFel} utas nem szállhatott fel. \nA legtöbb ember a(z) {stopCounts.MegalloSorszam} megállóban szállt fel, összesen {stopCounts.Count} ember. \nIngyenesen utazók száma: {ingyenes} fő \nKedvezményesen utazók száma: {kedvezmenyes} fő");
        }
    }
}
