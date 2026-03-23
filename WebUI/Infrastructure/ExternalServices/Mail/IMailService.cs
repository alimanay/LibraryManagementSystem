using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices.Mail
{
    public interface IMailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendDueDataReminderAsync(string toEmail, string userName, string bookTitle, DateTime returnDate);
        Task SendOverdueReminderAsync(string toEmail, string userName, string bookTitle, int overdueDays);


    }
}
