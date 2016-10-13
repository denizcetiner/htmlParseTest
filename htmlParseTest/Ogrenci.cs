using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace htmlParseTest
{
    public class Ogrenci
    {
        public String AkademikBirim { get; set; }
        public String Bolum { get; set; }
        public String KimlikNo { get; set; }
        public String Ad { get; set; }
        public String Soyad { get; set; }
        public String KayitTarihi { get; set; }
        public String AktifMi { get; set; }
        public String OgrenciNo { get; set; }
        public String Yonetmelik { get; set; }
        public List<Yariyil> Yariyillar { get; set; }
    }
}
