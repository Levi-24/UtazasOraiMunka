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
            DataContext = this;
        }
    }
}


