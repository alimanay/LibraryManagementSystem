📚 Library Management System (ASP.NET Core MVC)
Bu proje, modern yazılım mimarileri ve best-practice'ler kullanılarak geliştirilmiş kapsamlı bir kütüphane yönetim sistemidir. Kullanıcıların kitap ödünç alma süreçlerini yönetmek, kitap envanterini takip etmek ve dış API'lar ile entegrasyon sağlamak amacıyla tasarlanmıştır.

🚀 Öne Çıkan Özellikler

N-Tier Architecture: Data Access, Business, ve UI katmanları ile modüler yapı.


API Entegrasyonu: Google Books API kullanılarak kitap bilgilerinin otomatik çekilmesi.


Gelişmiş CRUD: Kitap, yazar ve kullanıcı yönetimi için tam fonksiyonel işlemler.


Ödünç Alma Sistemi: Geçmiş kayıtları tutan dinamik kitap ödünç/iade mekanizması.


Validasyon: Fluent Validation ile güvenli ve kurallı veri girişi.

🛠️ Kullanılan Teknolojiler

Framework: .NET 8 / ASP.NET Core MVC 


ORM: Entity Framework Core 


Database: MSSQL 


UI: Bootstrap, HTML5, CSS3, JavaScript 


Tools: AutoMapper, Dependency Injection, Repository Pattern 

⚙️ Kurulum ve Çalıştırma
Projeyi yerel makinenizde çalıştırmak için aşağıdaki adımları izleyin:

1. Repoyu Klonlayın
Bash
git clone https://github.com/alimanay/LibraryManagementSystem.git
cd LibraryManagementSystem
2. Veritabanı Yapılandırması
LibraryManagementSystem.WebUI projesindeki appsettings.json dosyasını açın ve ConnectionStrings bölümünü kendi yerel SQL Server bilgilerinizle güncelleyin:

JSON
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=LibraryDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
3. Migration Uygulama
Visual Studio'da Package Manager Console'u açın ve veritabanını oluşturmak için şu komutu çalıştırın:

PowerShell
Update-Database
4. Projeyi Başlatın
Visual Studio üzerinden F5 tuşuna basarak veya terminalden aşağıdaki komutla projeyi ayağa kaldırabilirsiniz:

Bash
dotnet run --project LibraryManagementSystem.WebUI
📂 Proje Yapısı (Architecture)
Core / Entities: Veritabanı tablolarının modelleri.


DataAccess: Veritabanı bağlantısı ve Repository implementasyonları.


Business: İş mantığı, servisler ve doğrulama (validation) kuralları.


WebUI: Kullanıcı arayüzü ve Controller yapıları.

🤝 İletişim

Ali Manay - LinkedIn - alimanayhs@gmail.com
