# 📚 Library Management System

![.NET](https://img.shields.io/badge/.NET%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![MSSQL](https://img.shields.io/badge/MSSQL-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Entity Framework](https://img.shields.io/badge/EF%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)

Bu proje, modern yazılım mimarileri ve best-practice'ler kullanılarak geliştirilmiş kapsamlı bir **kütüphane yönetim sistemidir**. Kullanıcıların kitap ödünç alma süreçlerini yönetmek, envanter takibi yapmak ve dış API entegrasyonları ile veri çekmek amacıyla tasarlanmıştır.

---

## 🚀 Öne Çıkan Özellikler

* **N-Tier Architecture:** Data Access, Business ve WebUI katmanları ile modüler ve sürdürülebilir yapı.
* **API Entegrasyonu:** Google Books API kullanılarak kitap bilgilerinin dinamik olarak çekilmesi.
* **Gelişmiş CRUD:** Kitaplar, yazarlar ve kullanıcılar için tam fonksiyonel yönetim paneli.
* **Ödünç Alma Mekanizması:** Kitap ödünç alma ve iade işlemlerini geçmiş kayıtlarıyla birlikte tutan sistem.
* **Veri Doğrulama:** Fluent Validation kütüphanesi ile güvenli ve kurallı veri girişi.

---

## 🛠️ Kullanılan Teknolojiler

### **Backend**
* **Framework:** ASP.NET Core MVC (.NET 9)
* **ORM:** Entity Framework Core
* **Database:** MSSQL
* **Design Patterns:** Repository Pattern, Dependency Injection
* **Kütüphaneler:** AutoMapper, Fluent Validation

### **Frontend**
* **UI Framework:** Bootstrap
* **Diller:** HTML5, CSS3, JavaScript

---

## 📂 Proje Yapısı (N-Tier Architecture)

Proje, sorumlulukların ayrılması prensibine göre aşağıdaki katmanlardan oluşmaktadır:

| Katman | Sorumluluk |
| :--- | :--- |
| **Core / Entities** | Veritabanı modelleri (Entities) ve veri transfer nesneleri (DTOs).  |
| **Infrastructure** | Google Books API gibi dış servis entegrasyonlarının yönetimi.  |
| **DataAccess** | Context yapısı, Repository implementasyonları ve Business Servisleri.  |
| **WebUI** | Kullanıcı arayüzü, View'lar ve Controller mantığı.  |

---

## ⚙️ Kurulum ve Çalıştırma

Projeyi yerel makinenizde ayağa kaldırmak için şu adımları izleyin:

### 1. Repoyu Klonlayın
```bash
git clone [https://github.com/alimanay/LibraryManagementSystem.git](https://github.com/alimanay/LibraryManagementSystem.git)
cd LibraryManagementSystem

-------------------------------------------------------------------------------------------------------------------------
2. Veritabanı Ayarları
LibraryManagementSystem.WebUI projesindeki appsettings.json dosyasını kendi SQL Server bilgilerinize göre güncelleyin:

JSON
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=LibraryDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
-------------------------------------------------------------------------------------------------------------------------

3. Migration ve Güncelleme
Package Manager Console üzerinden veritabanını oluşturun:
PowerShell
Update-Database

4. Çalıştırın
Bash
dotnet run --project LibraryManagementSystem.WebUI
🤝 İletişim
Ali Manay - Jr. Backend Developer

E-posta: alimanayhs@gmail.com

LinkedIn: linkedin.com/in/alimanay

GitHub: github.com/alimanay
