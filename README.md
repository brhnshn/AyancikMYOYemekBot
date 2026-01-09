# 🍽️ Ayancık MYO Yemek Menüsü & Telegram Botu

![License](https://img.shields.io/badge/license-MIT-blue.svg) ![.NET 8](https://img.shields.io/badge/.NET-8.0-purple.svg) ![Status](https://img.shields.io/badge/Status-Completed-success.svg)

**AyancıkMYOYemekBot**u, Sinop Üniversitesi Ayancık Meslek Yüksekokulu'nun web sitesinden günlük yemek menüsünü otomatik olarak çeken (web scraping), veritabanına kaydeden ve öğrencilere Telegram üzerinden anlık bildirim gönderen modern bir web uygulamasıdır.

## 🌟 Özellikler

* **Otomatik Veri Çekme:** `HtmlAgilityPack` kullanılarak üniversite sitesinden güncel menü verisi çekilir.
* **Telegram Entegrasyonu:** Günün menüsü, abone olan öğrencilere otomatik olarak veya komutla gönderilir.
* **Veritabanı Yönetimi:** Çekilen menüler ve kullanıcı kayıtları MSSQL veritabanında saklanır.
* **Modern Arayüz:** Kullanıcı dostu, **Dark Mode** destekli ve Glassmorphism tasarımlı web arayüzü.
* **Arka Plan Servisleri:** Uygulama belirli aralıklarla menüyü kontrol eder ve günceller.

## 🛠️ Kullanılan Teknolojiler

* **Backend:** ASP.NET Core 8.0 MVC
* **Veritabanı:** Entity Framework Core & MSSQL
* **Web Scraping:** HtmlAgilityPack
* **Bot API:** Telegram.Bot
* **Frontend:** Bootstrap 5, Custom CSS (Glassmorphism), JavaScript


## ⚠️ Kurulum Öncesi Önemli Uyarı

Bu proje açık kaynaklı olarak paylaşılmıştır, ancak güvenlik nedeniyle **Telegram Bot Token** bilgisi kodlardan çıkarılmıştır. Projeyi yerel makinenizde (localhost) çalıştırmadan önce **kendi Bot Token'ınızı** eklemeniz gerekmektedir.

Aksi takdirde uygulama **hata verecek** ve bot çalışmayacaktır.

## 🚀 Kurulum Adımları

Projeyi bilgisayarınızda çalıştırmak için aşağıdaki adımları izleyin:

### 1. Projeyi Klonlayın
```bash
git clone [https://github.com/kullaniciadiniz/AyancikYemekWeb.git](https://github.com/kullaniciadiniz/AyancikMYOYemekBot.git)
cd AyancikMYOYemekBot
```

**Telegram Token Ayarı**
Proje dosyaları içerisinde TelegramService.cs (veya tokenin tanımlı olduğu ilgili servis dosyasını) açın. 

// ÖRNEK KOD (Lütfen kendi tokeninizi girin)
private readonly string _botToken = "TOKEN";

**Veritabanı Bağlantısı**

appsettings.json dosyasını açın ve ConnectionStrings alanını kendi yerel SQL Server ayarlarınıza göre düzenleyin:

"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=AyancikYemekDb;Trusted_Connection=True;TrustServerCertificate=True;"
}

**Migration Uygulama**

Veritabanını ve tabloları oluşturmak için Package Manager Console'da şu komutu çalıştırın:


Update-Database

**Projeyi Başlatın**
Artık projeyi Visual Studio üzerinden veya terminalden başlatabilirsiniz:

dotnet run


**CANLI DEMO İÇİN**
[Buraya Tıklayınz!](https://burhansahin.com.tr/Yemek)


👤 İletişim:
[burhansahin.com.tr](https://burhansahin.com.tr/)

[LinkedIn](https://www.linkedin.com/in/burhan-sahin/)
