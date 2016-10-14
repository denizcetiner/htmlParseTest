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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CsQuery;
using System.IO;
using System.Windows.Forms;
using System.Net;
using Excel = Microsoft.Office.Interop.Excel;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace htmlParseTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<String> files = new List<string>();
        public List<String> htmls = new List<string>();
        public List<Ogrenci> ogrenciler = new List<Ogrenci>();
        String csv1Filename;
        String csv2Filename;
        List<AçılanDers> AçılanDersler = new List<AçılanDers>();
        List<Not> Notlar = new List<Not>();
        List<AD_Ogrenci> AD_Ogrenciler = new List<AD_Ogrenci>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            DialogResult result = fbd.ShowDialog();

            if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                files = Directory.GetFiles(fbd.SelectedPath).AsEnumerable().ToList();
            }
            foreach(var filename in files)
            {
             
                var html = File.ReadAllText(filename,Encoding.UTF8);
                htmls.Add(html);
            }
        }

        public int NotHesapla(String not)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            if(not == "AA")
            {
                return r.Next(90, 100);
            }
            if(not == "BA")
            {
                return r.Next(85, 89);
            }
            if(not == "BB")
            {
                return r.Next(80, 84);
            }
            if (not == "CB")
            {
                return r.Next(70, 79);
            }
            if (not == "CC")
            {
                return r.Next(60, 69);
            }
            if (not == "DC")
            {
                return r.Next(55, 59);
            }
            if (not == "DD")
            {
                return r.Next(50, 54);
            }
            if (not == "FD")
            {
                return r.Next(40, 49);
            }
            if (not == "FF")
            {
                return r.Next(0,39);
            }
            if (not == "YE")
            {
                return 100;
            }
            if (not == "YS")
            {
                return 0;
            }
            if (not == "DS")
            {
                return 0;
            }
            else
            {
                return 0;
            }


        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            foreach(var html in htmls)
            {
                Ogrenci o = new Ogrenci();
                List<Yariyil> Yariyillar = new List<Yariyil>();
                o.Yariyillar = Yariyillar;
                
                CQ dom = CQ.Create(html);
                var items = dom[".rgMasterTable"].Children("tbody");
                var tablo_0 = items[0]; // sıfırıncı tbody, öğrenci bilgilerini tutuyor.
                var tablo_0_trler = tablo_0.Cq().Children("tr"); //tbody'nin tr'leri
                o.AkademikBirim = WebUtility.HtmlDecode(tablo_0_trler[0].Cq().Children("td")[2].InnerText.Trim());
                
                //sıfırıncı tr'nin tdlerinden ikincisinin iç yazısı
                o.Bolum = WebUtility.HtmlDecode(tablo_0_trler[1].Cq().Children("td")[1].InnerText.Trim());
                o.KimlikNo = WebUtility.HtmlDecode(tablo_0_trler[2].Cq().Children("td")[1].InnerText.Trim());
                o.Ad = WebUtility.HtmlDecode(tablo_0_trler[3].Cq().Children("td")[1].InnerText.Trim());
                o.Soyad = WebUtility.HtmlDecode(tablo_0_trler[4].Cq().Children("td")[1].InnerText.Trim());
                o.KayitTarihi = WebUtility.HtmlDecode(tablo_0_trler[5].Cq().Children("td")[1].InnerText.Trim());
                o.AktifMi = WebUtility.HtmlDecode(tablo_0_trler[6].Cq().Children("td")[1].InnerText.Trim());
                o.OgrenciNo = WebUtility.HtmlDecode(tablo_0_trler[7].Cq().Children("td")[1].InnerText.Trim());
                o.Yonetmelik = WebUtility.HtmlDecode(tablo_0_trler[8].Cq().Children("td")[1].InnerText.Trim());

                for (int index1 = 1; index1 < items.Count() - 1; index1++) //yariyillar
                {
                    var i = items[index1].Cq();
                    i = i.Children("tr");

                    //
                    for (int index = 0; index < i.Count(); index++)
                    {
                        var i1 = i[index];
                        var i2 = i1.Cq();
                        i2 = i2.Children("td");

                        AçılanDers ad = new AçılanDers();
                        ad.Id = AçılanDersler.Count;
                        ad.Kod = WebUtility.HtmlDecode(i2[0].InnerText);
                        try
                        {
                            ad.DersAdi = WebUtility.HtmlDecode(i2[1].InnerText); 
                            //bazı kişilerde yarıyıl tablosunda ders bulunmayabiliyor
                            //örneğin 7.yarıyıl boş ama 8.yarıyıl dolu
                            //öyle tabloları atlamak için koydum
                        }
                        catch(Exception ex)
                        {
                            continue;
                        }

                        ad.Yariyil = index1.ToString();
                        ad.YılDers = Convert.ToInt32(o.KayitTarihi.Split('.')[2]) + ((index1 - 1)/ 2);

                        if (ad.DersAdi == "Algoritma  ve Programlama ")
                        {
                            ad.DersAdi = "Algoritma ve Programlama";
                            ad.Kod = "14BLM105";
                        }
                        else if (ad.DersAdi == "Atatürk  İlkeleri ve İnkılap Tarihi I")
                        {
                            ad.DersAdi = "Atatürk İlkeleri ve İnkilap Tarihi I";
                            ad.Kod = "14ATA101";
                        }
                        else if (ad.DersAdi == "Atatürk İlkeleri ve İnkılap Tarihi I")
                        {
                            ad.DersAdi = "Atatürk İlkeleri ve İnkilap Tarihi I";
                            ad.Kod = "14ATA101";
                        }
                        else if (ad.DersAdi == "Algoritma Analizi")
                        {
                            ad.DersAdi = "Algoritma Analiz";
                            ad.Kod = "BLM4006";
                        }
                        else if (ad.DersAdi == "Matematik")
                        {
                            ad.DersAdi = "Matematik I";
                            ad.Kod = "\"14BLM103 \"";
                        }
                        else if (ad.DersAdi == "İngilizce I")
                        {
                            ad.DersAdi = "Yabancı Dil I (İngilizce)";
                            ad.Kod = "\"14YDİ101 \"";
                        }
                        else if (ad.DersAdi == "Fizik I")
                        {
                            ad.DersAdi = "Genel Fizik I";
                            ad.Kod = "14BLM101";
                        }
                        else if (ad.DersAdi == "Yabancı Dil (İngilizce) I")
                        {
                            ad.DersAdi = "Yabancı Dil I (İngilizce)";
                            ad.Kod = "\"14YDİ101 \"";
                        }
                        else if (ad.DersAdi == "İngilizce II")
                        {
                            ad.DersAdi = "Yabancı Dil II (İngilizce)";
                            ad.Kod = "14YDİ102";
                        }
                        else if (ad.DersAdi == "Fizik II")
                        {
                            ad.DersAdi = "Genel Fizik II";
                            ad.Kod = "14BLM102";
                        }
                        else if (ad.DersAdi == "Yabancı Dil (İngilizce) II")
                        {
                            ad.DersAdi = "Yabancı Dil II (İngilizce)";
                            ad.Kod = "14YDİ102";
                        }
                        else if (ad.DersAdi == "Yapısal Programlama")
                        {
                            ad.DersAdi = "Yabancı Dil II (İngilizce)";
                            ad.Kod = "14YDİ102";
                        }

                        AD_Ogrenci ado = new AD_Ogrenci();
                        ado.Id = AD_Ogrenciler.Count;
                        ado.OgrenciId = o.OgrenciNo;
                        ado.AçılanDersId = ad.Id;

                        if (!AçılanDersler.Any(AD=> AD.Kod == ad.Kod && AD.YılDers == ad.YılDers))
                        {
                            AçılanDersler.Add(ad);
                        }
                        else
                        {
                            ado.AçılanDersId = AçılanDersler.First(AD => AD.Kod == ad.Kod && AD.YılDers == ad.YılDers).Id;
                        }
                        
                        AD_Ogrenciler.Add(ado);

                        Not not = new Not();
                        not.Id = Notlar.Count;
                        not.AD_OgrenciId = ado.Id;
                        not.Vize = NotHesapla(WebUtility.HtmlDecode(i2[5].InnerText));
                        not.Final = NotHesapla(WebUtility.HtmlDecode(i2[5].InnerText));
                        not.HarfNotu = WebUtility.HtmlDecode(i2[5].InnerText);
                        not.Büt = 0;
                        not.YilNotu = Convert.ToInt32(not.Vize * 0.4 + not.Final * 0.6);
                        not.OtomatikMi = true;
                        Notlar.Add(not);
                    }
                }
            }

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.FilterIndex = 2;
            file.RestoreDirectory = true;
            file.CheckFileExists = false;
            

            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                csv1Filename = file.FileName;
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            List<String> rows = File.ReadAllLines(csv1Filename).ToList();
            for(int i = 1; i < rows.Count; i++)
            {
                String row = WebUtility.HtmlDecode(rows[i]);
                String[] columns = row.Split(',');
                AçılanDersler.ForEach(u =>
                {
                    if(u.DersAdi == columns[1])
                    {
                        u.Kod = columns[0];
                    }
                });
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            using (var writer = new StreamWriter(new FileStream("AçılanDersler.txt", FileMode.Create), Encoding.GetEncoding("iso-8859-9")))
            {
                writer.Write("Id,DersKodu,DersAdi,AkademisyenId,Yariyil,YilDers\n");
                foreach (var ad in AçılanDersler)
                {
                    writer.Write(ad.Id);
                    writer.Write("," + ad.Kod);
                    
                    writer.Write("," + ad.DersAdi);
                    if(ad.OgretmenId == null)
                    {
                        ad.OgretmenId = "-1";
                    }
                    writer.Write("," + ad.OgretmenId);
                    writer.Write("," + ad.Yariyil);
                    if(ad.YılDers == 2016)
                    {
                        writer.Write("," + 1900);
                    }
                    else
                    {
                        writer.Write("," + ad.YılDers);
                    }
                    
                    writer.Write("\n");

                }
            }
            using (var writer = new StreamWriter(new FileStream("Notlar.txt", FileMode.Create), Encoding.GetEncoding("iso-8859-9")))
            {
                writer.Write("NotId,KayıtId,Vize,Final,Büt,YılNot,HarfNotu,OtomatikMi\n");
                foreach (var not in Notlar)
                {
                    writer.Write(not.Id);
                    writer.Write("," + not.AD_OgrenciId);
                    writer.Write("," + not.Vize);
                    writer.Write("," + not.Final);
                    writer.Write("," + not.Büt);
                    writer.Write("," + not.YilNotu);
                    writer.Write("," + not.HarfNotu);
                    writer.Write("," + not.OtomatikMi);
                    writer.Write("\n");
                }
            }
            using (var writer = new StreamWriter(new FileStream("Kayıtlar.txt", FileMode.Create), Encoding.GetEncoding("iso-8859-9")))
            {
                writer.Write("KayıtId,ADId,OgrId\n");
                foreach (var kayıt in AD_Ogrenciler)
                {
                    writer.Write(kayıt.Id);
                    writer.Write("," + kayıt.AçılanDersId);
                    writer.Write("," + kayıt.OgrenciId);
                    writer.Write("\n");
                }
            }
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.FilterIndex = 2;
            file.RestoreDirectory = true;
            file.CheckFileExists = false;


            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                csv2Filename = file.FileName;
            }
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            List<String> rows = File.ReadAllLines(csv2Filename).ToList();
            for (int i = 0; i < rows.Count; i++)
            {
                String row = WebUtility.HtmlDecode(rows[i]);
                String[] columns = row.Split(',');
                AçılanDersler.ForEach(u =>
                {
                    if (u.Kod == columns[0])
                    {
                        if (columns[2] == "")
                        {
                            u.OgretmenId = "-1";
                        }
                        else
                        {
                            u.OgretmenId = columns[2].Trim();
                        }
                    }
                });
            }
        }

        
    }
}
