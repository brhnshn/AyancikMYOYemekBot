using AyancikYemekWeb.Data;
using AyancikYemekWeb.Services;
using AyancikYemekWeb.Models; // Abone modelini tanimasi icin bunu ekledik
using Microsoft.EntityFrameworkCore;

namespace AyancikYemekWeb.BackgroundServices
{
    public class BotWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BotWorker> _logger;

        // Bildirim Saati: 11:00
        private const int BildirimSaati = 11;
        private const int BildirimDakikasi = 00;

        public BotWorker(IServiceScopeFactory scopeFactory, ILogger<BotWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("BotWorker başlatıldı. Bildirim saati bekleniyor...");

            while (!stoppingToken.IsCancellationRequested)
            {
                // 1. ZAMAN HESAPLAMA (Senin kodun aynen korundu)
                var simdi = DateTime.Now;
                var hedefZaman = new DateTime(simdi.Year, simdi.Month, simdi.Day, BildirimSaati, BildirimDakikasi, 0);

                if (simdi > hedefZaman)
                {
                    hedefZaman = hedefZaman.AddDays(1);
                }

                var kalanSure = hedefZaman - simdi;
                _logger.LogInformation($"Bir sonraki bildirim için kalan süre: {kalanSure.Hours} saat {kalanSure.Minutes} dakika.");

                // Saati gelene kadar bekle
                await Task.Delay(kalanSure, stoppingToken);

                // 2. VAKİT GELDİ!
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var scraper = scope.ServiceProvider.GetRequiredService<ScraperService>();
                        var telegramService = scope.ServiceProvider.GetRequiredService<TelegramService>();

                        // Veritabanı bağlantısını da çağırdık
                        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                        _logger.LogInformation("Saat 11:00! Menü kontrol ediliyor...");

                        // Menüyü çek
                        var menu = await scraper.MenuyuGetirAsync();

                        if (menu.MenüVarMi)
                        {
                            // --- DEĞİŞEN KISIM BURASI ---
                            // AdminChatId yerine veritabanındaki tüm aboneleri çekiyoruz.

                            var aboneler = await db.Aboneler.ToListAsync();
                            _logger.LogInformation($"Veritabanında {aboneler.Count} abone bulundu, gönderim başlıyor...");

                            if (aboneler.Count > 0)
                            {
                                foreach (var abone in aboneler)
                                {
                                    try
                                    {
                                        await telegramService.MesajGonder(abone.ChatId, "🔔 Günlük Yemek Bildirimi:\n\n" + menu.ToString());

                                        // Telegram sunucularını yormamak için her mesaj arası minik bir bekleme (0.1 saniye)
                                        await Task.Delay(100);
                                    }
                                    catch (Exception sendEx)
                                    {
                                        _logger.LogError($"Kullanıcıya ({abone.ChatId}) mesaj giderken hata: {sendEx.Message}");
                                    }
                                }
                                _logger.LogInformation("Tüm abonelere gönderim tamamlandı.");
                            }
                            else
                            {
                                _logger.LogWarning("Hiç abone bulunamadı.");
                            }
                        }
                        else
                        {
                            _logger.LogInformation("Menü henüz yayınlanmamış, bildirim atılmadı.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"BotWorker Genel Hatası: {ex.Message}");
                }

                // Döngü hemen tekrar girmesin diye 1 dakika bekle
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}