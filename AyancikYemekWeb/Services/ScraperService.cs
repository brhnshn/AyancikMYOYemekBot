using AyancikYemekWeb.Data;
using AyancikYemekWeb.Models;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net;

namespace AyancikYemekWeb.Services
{
    public class ScraperService
    {
        private readonly AppDbContext _context;
        private const string Url = "https://sksdb.sinop.edu.tr/yemek-listesi/";

        public ScraperService(AppDbContext context)
        {
            _context = context;
        }

        // Hata veren yer burasıydı, ismi düzelttik:
        public async Task<YemekModel> MenuyuGetirAsync()
        {
            var model = new YemekModel();
            try
            {
                var web = new HtmlWeb();
                var doc = await web.LoadFromWebAsync(Url);
                var culture = new CultureInfo("tr-TR");

                var bugunTarihFormat1 = DateTime.Now.ToString("d MMMM yyyy", culture);
                var bugunTarihFormat2 = DateTime.Now.ToString("dd.MM.yyyy");

                // İki formatı da kontrol et
                var satir = doc.DocumentNode.SelectSingleNode($"//tr[td[contains(text(), '{bugunTarihFormat1}')]]")
                            ?? doc.DocumentNode.SelectSingleNode($"//tr[td[contains(text(), '{bugunTarihFormat2}')]]");

                if (satir != null)
                {
                    var hucreler = satir.SelectNodes("td");
                    if (hucreler != null && hucreler.Count >= 3)
                    {
                        model.Tarih = DateTime.Now.ToString("dd.MM.yyyy");

                        var yemekMetni = WebUtility.HtmlDecode(hucreler[2].InnerText).Trim();
                        yemekMetni = yemekMetni.Replace("\"", "").Replace("“", "").Replace("”", "");

                        var yemekListesi = yemekMetni.Split(new[] { ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                        if (yemekListesi.Length > 0) model.Corba = yemekListesi[0].Trim();
                        if (yemekListesi.Length > 1) model.AnaYemek = yemekListesi[1].Trim();
                        if (yemekListesi.Length > 2) model.YardimciYemek = yemekListesi[2].Trim();
                        if (yemekListesi.Length > 3) model.Tatli = yemekListesi[3].Trim();

                        model.MenüVarMi = true;

                        // Veritabanı kontrolü ve kayıt
                        var varMi = await _context.YemekMenuleri.AnyAsync(x => x.Tarih == model.Tarih);
                        if (!varMi)
                        {
                            _context.YemekMenuleri.Add(model);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Scraper Hatası: " + ex.Message);
            }
            return model;
        }
    }
}