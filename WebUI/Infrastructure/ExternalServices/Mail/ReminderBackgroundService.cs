using Infrastructure.ExternalServices.Mail;
using Kütüphane_Yonetim_Sistemi.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.ExternalServices.Mail
{
    public class ReminderBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ReminderBackgroundService> _logger;

        public ReminderBackgroundService(IServiceScopeFactory scopeFactory,
            ILogger<ReminderBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ReminderBackgroundService başlatıldı.");

            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckAndSendReminders();

                // Bir sonraki çalışmaya kadar bekle — her 24 saatte bir
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        private async Task CheckAndSendReminders()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();
            var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();
            var today = DateTime.Now.Date;
            var rentals = await context.Rentals
                .Include(r => r.User)
                .Include(r => r.Book)
                .Where(r => !r.IsReturned && r.ReturnDate.HasValue)
                .ToListAsync();

            foreach (var rental in rentals)
            {
                var returnDate = rental.ReturnDate!.Value.Date;
                var overdueDays = (today - returnDate).Days;
                var email = rental.User.Email;
                var userName = $"{rental.User.Name} {rental.User.Surname}";
                var bookTitle = rental.Book.Title;

                  // Son gün 
                if (returnDate == today)
                {
                    _logger.LogInformation(
                        "Son teslim günü maili gönderiliyor. Kullanıcı: {User} - Kitap: {Book}",
                        userName, bookTitle);

                    await mailService.SendDueDataReminderAsync(email, userName, bookTitle, returnDate);
                }

                // 3 gün gecikmiş
                else if (overdueDays == 3)
                {
                    _logger.LogWarning(
                        "Gecikme maili gönderiliyor. Kullanıcı: {User} - Kitap: {Book} - {Days} gün gecikmiş",
                        userName, bookTitle, overdueDays);

                    await mailService.SendOverdueReminderAsync(email, userName, bookTitle, overdueDays);
                }
            }
        }
    }
}