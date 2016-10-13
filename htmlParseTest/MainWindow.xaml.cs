﻿using System;
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
            List<AçılanDers> AçılanDersler = new List<AçılanDers>();
            List<Not> Notlar = new List<Not>();
            List<AD_Ogrenci> AD_Ogrenciler = new List<AD_Ogrenci>();
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
                    Yariyil y = new Yariyil();
                    List<Ders> dersler = new List<Ders>();
                    var i = items[index1].Cq();
                    i = i.Children("tr");

                    //
                    for (int index = 0; index < i.Count(); index++)
                    {
                        Ders ders = new Ders();
                        var i1 = i[index];
                        var i2 = i1.Cq();
                        i2 = i2.Children("td");
                        ders.DersKodu = WebUtility.HtmlDecode(i2[0].InnerText);
                        ders.DersAdi = WebUtility.HtmlDecode(i2[1].InnerText);
                        ders.Kredi = WebUtility.HtmlDecode(i2[2].InnerText);
                        ders.AKTS = WebUtility.HtmlDecode(i2[3].InnerText);
                        ders.Katsayi = WebUtility.HtmlDecode(i2[4].InnerText);
                        ders.BasariPuan = WebUtility.HtmlDecode(i2[5].InnerText);

                        AçılanDers AD = new AçılanDers();
                        AD.Id = AçılanDersler.Count;
                        AD.Kod = WebUtility.HtmlDecode(i2[0].InnerText);
                        AD.DersAdi = ders.DersAdi;
                        AD.Yariyil = index1.ToString();
                        AD.YılDers = Convert.ToInt32(o.KayitTarihi.Split('.')[2]) + (index1 / 2);

                        //AD.VizeOrani1 = 0.4;
                        //AD.VizeOrani2 = 0.0;
                        //AD.FinalOrani = 0.6;

                        AD_Ogrenci ado = new AD_Ogrenci();
                        ado.Id = AD_Ogrenciler.Count;
                        ado.OgrenciId = o.OgrenciNo;
                        ado.AçılanDersId = AD.Id;

                        if (!AçılanDersler.Any(ad=> ad.Kod == AD.Kod && ad.YılDers == AD.YılDers))
                        {
                            AçılanDersler.Add(AD);
                        }
                        else
                        {
                            ado.AçılanDersId = AçılanDersler.First(ad => ad.Kod == AD.Kod && ad.YılDers == AD.YılDers).Id;
                        }
                        
                        AD_Ogrenciler.Add(ado);

                        Not not = new Not();
                        not.Id = Notlar.Count;
                        not.AD_OgrenciId = ado.Id;
                        not.Vize = NotHesapla(ders.BasariPuan);
                        not.Final = NotHesapla(ders.BasariPuan);
                        not.HarfNotu = ders.BasariPuan;
                        not.Büt = 0;
                        not.YilNotu = Convert.ToInt32(not.Vize * 0.4 + not.Final * 0.6);
                        not.OtomatikMi = true;
                        Notlar.Add(not);

                        dersler.Add(ders);
                    }
                    y.Dersler = dersler;
                    o.Yariyillar.Add(y);
                }
                ogrenciler.Add(o);
            }
            using (var writer = new StreamWriter(new FileStream("AçılanDersler.txt", FileMode.Create), Encoding.GetEncoding("iso-8859-9")))
            {
                writer.Write("Id,DersKodu,DersAdi,AkademisyenId,Yariyil,YilDers\n");
                foreach (var ad in AçılanDersler)
                {

                    writer.Write(ad.Id);
                    writer.Write("," + ad.Kod);
                    writer.Write("," + ad.DersAdi);
                    writer.Write("," + ad.OgretmenId);
                    writer.Write("," + ad.Yariyil);
                    writer.Write("," + ad.YılDers);
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

            //using (var writer = new StreamWriter(new FileStream("asdf.txt", FileMode.Create), Encoding.GetEncoding("iso-8859-9")))
            //{
            //    writer.Write("ÖğrenciNo,Yarıyıl,DersKodu,Kredi,AKTS,Katsayi,BaşarıPuanı\n");
            //    foreach (var ogrenci in ogrenciler)
            //    {
            //        for(int index = 0; index < ogrenci.Yariyillar.Count; index++)
            //        {
            //            foreach (var ders in ogrenci.Yariyillar[index].Dersler)
            //            {
            //                writer.Write(ogrenci.OgrenciNo);
            //                writer.Write("," + (index + 1));
            //                writer.Write("," + ders.DersKodu);
            //                writer.Write(",\"" + ders.Kredi + "\"");
            //                writer.Write(",\"" + ders.AKTS + "\"");
            //                writer.Write(",\"" + ders.Katsayi + "\"");
            //                writer.Write("," + ders.BasariPuan);
            //                writer.Write("\n");
            //            }
            //            writer.Write("\n");
            //        }
            //    }
            //}

        }
    }
}
