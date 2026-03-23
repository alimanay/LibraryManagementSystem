using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices.Mail
{
    public class GmailService : IMailService
    {
        private readonly IConfiguration _config;
        public GmailService(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
           var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Kütüpahane Sistemi", _config["MailSettings:Email"]));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };
            using var client = new SmtpClient();
            await client.ConnectAsync(_config["MailSettings:Host"],
                int.Parse(_config["MailSettings:Port"]!), false);
            await client.AuthenticateAsync(_config["MailSettings:Email"],
              _config["MailSettings:Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        public async Task SendDueDataReminderAsync(string toEmail, string userName, string bookTitle, DateTime returnDate)
        {

            var subject = "Kitap Teslim Tarihi Hatırlatması";
            var body = $@"
                <h3>Sayın {userName},</h3>
                <p><b>{bookTitle}</b> adlı kitabın son teslim tarihi bugün!</p>
                <p>Teslim tarihi: <b>{returnDate:dd.MM.yyyy}</b></p>
                <p>Lütfen kitabı zamanında teslim ediniz.</p>
                <br/>
                <small>Kütüphane Yönetim Sistemi</small>";

            await SendEmailAsync(toEmail, subject, body);
        }

        public  async Task SendOverdueReminderAsync(string toEmail, string userName, string bookTitle, int overdueDays)
        {
            var subject = "⚠️ Gecikmiş Kitap Uyarısı";
            var body = $@"
                <h3>Sayın {userName},</h3>
                <p><b>{bookTitle}</b> adlı kitabı <b>{overdueDays} gün</b> geciktirdiniz.</p>
                <p>Lütfen en kısa sürede teslim ediniz.</p>
                <br/>
                <small>Kütüphane Yönetim Sistemi</small>";

            await SendEmailAsync(toEmail, subject, body);
        }

       
    }
}
