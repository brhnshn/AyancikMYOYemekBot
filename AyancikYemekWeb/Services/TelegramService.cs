using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups; // Butonlar için gerekli kütüphane
using AyancikYemekWeb.Models;
using AyancikYemekWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace AyancikYemekWeb.Services
{
    public class TelegramService
    {
        private readonly TelegramBotClient _botClient;
        private readonly IServiceScopeFactory _scopeFactory;
        private const string BotToken = "TOKEN";

        public TelegramService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _botClient = new TelegramBotClient(BotToken);
            StartReceiving();
        }

        private void StartReceiving()
        {
            var receiverOptions = new ReceiverOptions
            {
                // Mesajları ve Buton Tıklamalarını (CallbackQuery) dinliyoruz
                AllowedUpdates = new[] { UpdateType.Message, UpdateType.CallbackQuery }
            };

            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandlePollingErrorAsync,
                receiverOptions,
                CancellationToken.None
            );
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                // 1. DURUM: KULLANICI MESAJ ATARSA (/start veya /bugun)
                if (update.Message is { } message && message.Text is { } messageText)
                {
                    var chatId = message.Chat.Id;

                    if (messageText == "/start")
                    {
                        // Direkt kaydetmiyoruz, buton gösteriyoruz
                        var inlineKeyboard = new InlineKeyboardMarkup(new[]
                        {
                            new [] // 1. Satır
                            {
                                InlineKeyboardButton.WithCallbackData("🔔 Abone Ol", "abone_ol"),
                            },
                            new [] // 2. Satır
                            {
                                InlineKeyboardButton.WithCallbackData("📅 Bugünkü Menü", "menuyu_getir"),
                            }
                        });

                        await botClient.SendMessage(
                            chatId: chatId,
                            text: "👋 *Ayancık MYO Yemek Botuna Hoş Geldin!*\n\nHer sabah 11:00'de yemek menüsünün cebine gelmesini istiyorsan aşağıdaki *'Abone Ol'* butonuna tıkla.",
                            parseMode: ParseMode.Markdown,
                            replyMarkup: inlineKeyboard
                        );
                    }
                    else if (messageText == "/bugun" || messageText.ToLower().Contains("yemek"))
                    {
                        await MenuyuGonder(chatId);
                    }
                }

                // 2. DURUM: KULLANICI BUTONA TIKLARSA
                else if (update.CallbackQuery is { } callbackQuery)
                {
                    var chatId = callbackQuery.Message!.Chat.Id;
                    var callbackData = callbackQuery.Data;

                    // Telegram'a "tıklamayı algıladım" bilgisini gönder (loading simgesi gitsin)
                    await botClient.AnswerCallbackQuery(callbackQuery.Id);

                    if (callbackData == "abone_ol")
                    {
                        bool zatenAboneMi = false;

                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                            if (!db.Aboneler.Any(x => x.ChatId == chatId))
                            {
                                var yeniAbone = new Abone { ChatId = chatId, AktifMi = true };
                                db.Aboneler.Add(yeniAbone);
                                db.SaveChanges();
                                Console.WriteLine($"✅ Yeni abone butonla eklendi: {chatId}");
                            }
                            else
                            {
                                zatenAboneMi = true;
                            }
                        }

                        if (zatenAboneMi)
                        {
                            await botClient.SendMessage(chatId, "✅ Zaten abone listesindesin. Menüler her sabah gelecek.");
                        }
                        else
                        {
                            // İşte o istediğin mesaj sadece burada gidecek
                            await botClient.SendMessage(chatId, "🎉 *Harika! Kaydın alındı.*\n\nArtık yemek menüsü her sabah 11:00'de otomatik olarak cebine gelecek. \n\nAfiyet olsun! 😋", parseMode: ParseMode.Markdown);
                        }
                    }
                    else if (callbackData == "menuyu_getir")
                    {
                        await MenuyuGonder(chatId);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }
        }

        private async Task MenuyuGonder(long chatId)
        {
            await MesajGonder(chatId, "🍽 Menü kontrol ediliyor...");

            using (var scope = _scopeFactory.CreateScope())
            {
                var scraper = scope.ServiceProvider.GetRequiredService<ScraperService>();
                var menu = await scraper.MenuyuGetirAsync();

                if (menu.MenüVarMi)
                    await MesajGonder(chatId, menu.ToString());
                else
                    await MesajGonder(chatId, "📅 Bugün için yemek listesi henüz yayınlanmamış.");
            }
        }

        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine("Telegram Hatası: " + exception.Message);
            return Task.CompletedTask;
        }

        public async Task MesajGonder(long chatId, string mesaj)
        {
            try
            {
                await _botClient.SendMessage(chatId: chatId, text: mesaj, parseMode: ParseMode.Markdown);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mesaj Gönderme Hatası: {ex.Message}");
            }
        }
    }
}