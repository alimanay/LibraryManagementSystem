using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public string TCNumber { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; }
        public bool IsPasswordChanged { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTime {  get; set; }
        public string? TwoFactorCode { get; set; }
        public DateTime? TwoFactorExpiry { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Rental> Rentals { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
