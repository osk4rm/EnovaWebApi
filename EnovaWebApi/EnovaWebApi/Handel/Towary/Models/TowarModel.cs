using Soneta.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnovaWebApi.Handel.Models
{
    [BinSerializable]
    public class TowarModel
    {
        public string Kod { get; set; }
        public string EAN { get; set; }
        public string Nazwa { get; set; }
        public double Stan { get; set; }
        public string Jednostka { get; set; }
        public double Cena { get; set; }
    }
}
