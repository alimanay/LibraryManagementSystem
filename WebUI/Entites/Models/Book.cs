using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Models
{
    public  class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string? Author { get; set; }
        public string? Type {  get; set; }
        public string? ISBN { get; set; }
        public string? Description { get; set; }

        public string? Image {  get; set; }

        public bool IsActive { get; set; }

        // Navigation
        public ICollection<Rental> Rentals { get; set; }


    }
}
