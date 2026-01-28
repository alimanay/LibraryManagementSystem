using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Models
{
    public class Rental
    {
        public int Id { get; set; }

        // Foreign Keys
        public int UserId { get; set; }
        public int BookId { get; set; }

        // Dates
        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        // Status
        public bool IsReturned { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public Book Book { get; set; }
    }
}
