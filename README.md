# 📚 Library Management System

![.NET](https://img.shields.io/badge/.NET%209-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![MSSQL](https://img.shields.io/badge/MSSQL-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Entity Framework](https://img.shields.io/badge/EF%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Serilog](https://img.shields.io/badge/Serilog-000000?style=for-the-badge&logo=serilog&logoColor=white)

Bu proje, modern yazılım mimarileri ve best-practice'ler kullanılarak geliştirilmiş kapsamlı bir **kütüphane yönetim sistemidir**. Kullanıcıların kitap ödünç alma süreçlerini yönetmek, envanter takibi yapmak ve dış API entegrasyonları ile veri çekmek amacıyla tasarlanmıştır.

---

## 🚀 Öne Çıkan Özellikler

* **N-Tier Architecture:** Data Access, Business ve WebUI katmanları ile modüler ve sürdürülebilir yapı.
* **API Entegrasyonu:** Google Books API kullanılarak kitap bilgilerinin dinamik olarak çekilmesi.
* **Gelişmiş CRUD:** Kitaplar ve kullanıcılar için tam fonksiyonel yönetim paneli.
* **Ödünç Alma Mekanizması:** Kitap ödünç alma ve iade işlemlerini geçmiş kayıtlarıyla birlikte tutan sistem.
* **Güvenlik:** BCrypt şifre hash'leme ve AES-256 ile TC kimlik şifreleme.
* **2FA Doğrulama:** Email ile iki faktörlü kimlik doğrulama sistemi.
* **Şifre Yönetimi:** Şifremi unuttum ve şifre sıfırlama akışı.
* **Mail Sistemi:** Gecikmiş kitap ve son teslim günü için otomatik email bildirimi.
* **Loglama:** Serilog ile servis katmanında kapsamlı loglama.
* **Dashboard:** AdminLTE 4 tabanlı yönetim paneli ve kullanıcı paneli.
* **Kullanıcı Paneli:** Kullanıcıya özel kiralama geçmişi ve istatistikler.

---

## 🛠️ Kullanılan Teknolojiler

### **Backend**
* **Framework:** ASP.NET Core MVC (.NET 9)
* **ORM:** Entity Framework Core 9
* **Database:** MSSQL
* **Design Patterns:** Repository Pattern, Dependency Injection, Service Layer
* **Güvenlik:** BCrypt.Net-Next, AES-256 şifreleme
* **Loglama:** Serilog (File + Console sink)
* **Mail:** MailKit (Gmail SMTP)
* **Background Service:** .NET BackgroundService (otomatik mail hatırlatması)
* **Kütüphaneler:** AutoMapper, X.PagedList

### **Frontend**
* **UI Framework:** AdminLTE 4, Bootstrap 5
* **Diller:** HTML5, CSS3, JavaScript, jQuery
* **Grafikler:** ApexCharts

---

## 📂 Proje Yapısı (N-Tier Architecture)

Proje, sorumlulukların ayrılması prensibine göre aşağıdaki katmanlardan oluşmaktadır:

| Katman | Sorumluluk |
| :--- | :--- |
| **Entites** | Veritabanı modelleri (Entities) ve veri transfer nesneleri (DTOs). |
| **Infrastructure** | Google Books API ve Mail servisi gibi dış servis entegrasyonları. |
| **DataAccess** | Context yapısı, Repository implementasyonları ve Business Servisleri. |
| **WebUI** | Kullanıcı arayüzü, View'lar ve Controller mantığı. |

---

## 🔐 Güvenlik Özellikleri

* **BCrypt** — Kullanıcı şifreleri tek yönlü hash ile saklanır, geri döndürülemez.
* **AES-256** — TC kimlik numaraları şifreli olarak veritabanında saklanır.
* **2FA** — Giriş sırasında email ile 6 haneli doğrulama kodu gönderilir (2 dakika geçerli).
* **Şifre Sıfırlama** — Token bazlı şifre sıfırlama akışı (1 saat geçerli link).
* **İlk Giriş Zorunlu Şifre Değişimi** — Admin tarafından oluşturulan kullanıcılar ilk girişte şifre değiştirmek zorundadır.

---

## 📧 Mail Sistemi

Sistem arka planda her 24 saatte bir otomatik olarak çalışarak:
* **Son teslim günü** olan kullanıcılara hatırlatma maili gönderir.
* **3 gün gecikmiş** kullanıcılara uyarı maili gönderir.

---

## ⚙️ Kurulum ve Çalıştırma

### 1. Repoyu Klonlayın

```bash
git clone https://github.com/alimanay/LibraryManagementSystem.git
cd LibraryManagementSystem
```

### 2. Bağlantı Ayarları

`appsettings.Development.json` dosyası güvenlik nedeniyle repoya eklenmemiştir.
`WebUI/Kütüphane_Yonetim_Sistemi` klasörü içine `appsettings.Development.json` adında bir dosya oluşturun:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "sqlConnection": "Server=YOUR_SERVER;Database=LibrarySystemDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "GoogleBooks": {
    "ApiKey": "YOUR_GOOGLE_BOOKS_API_KEY"
  },
  "MailSettings": {
    "Host": "smtp.gmail.com",
    "Port": "587",
    "Email": "YOUR_GMAIL_ADDRESS",
    "Password": "YOUR_GMAIL_APP_PASSWORD"
  },
  "Encryption": {
    "Key": "YOUR_32_BYTE_BASE64_KEY"
  }
}
```

> 📌 Google Books API key: https://console.cloud.google.com  
> 📌 Gmail App Password: Google Hesabı → Güvenlik → 2 Adımlı Doğrulama → Uygulama Şifreleri  
> 📌 Encryption Key üretmek için:
> ```csharp
> var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
> ```

### 3. Migration ve Veritabanı

Package Manager Console üzerinden (Default Project: DataAccess):

```
Update-Database
```

### 4. Çalıştırın

```bash
dotnet run --project WebUI/Kütüphane_Yonetim_Sistemi
```

---

## 🧪 Unit Test

Proje **xUnit** ve **Moq** kütüphaneleri kullanılarak test edilmiştir. Servis katmanındaki temel iş mantıkları unit test kapsamındadır:

* `BookService` — Ekleme, silme, güncelleme ve getirme testleri
* `UserService` — Kullanıcı yönetimi testleri
* `RentalService` — Kiralama oluşturma ve güncelleme testleri

---

## 🤝 İletişim

**Ali Manay** — Jr. Backend Developer

* 📧 E-posta: alimanayhs@gmail.com
* 💼 LinkedIn: [linkedin.com/in/alimanay](https://linkedin.com/in/alimanay)
* 🐙 GitHub: [github.com/alimanay](https://github.com/alimanay)
