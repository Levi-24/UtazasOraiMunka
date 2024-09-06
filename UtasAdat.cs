using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utazas
{
    internal class UtasAdat
    {
        public int MegalloSorszam { get; set; }
        public int FelszallasDatum { get; set; }
        public int FelszallasIdo { get; set; }
        public int Azonosito { get; set; }
        public string Tipus { get; set; }
        public int Datum { get; set; }
        public int Jegy { get; set; }

        public UtasAdat(string readline)
        {
            var adat = readline.Split(' ');
            MegalloSorszam = int.Parse(adat[0]);

            var temp = adat[1].Split('-');

            FelszallasDatum = int.Parse(temp[0]);
            FelszallasIdo = int.Parse(temp[1]);
            Azonosito = int.Parse(adat[2]);
            Tipus = adat[3];

            if (adat[4].Length > 2)
            {
                Datum = int.Parse(adat[4]);
                Jegy = 0;
            }
            else
            {
                Datum = 0;
                Jegy = int.Parse(adat[4]);
            }
        }
    }
}
