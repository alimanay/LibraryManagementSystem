using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Dtos.BookDtos
{
    public class BookListDto
    {
        public string Title { get; set; }
        public string? Author { get; set; }
  
        public string? Description { get; set; }

        public string? Image { get; set; }

        public bool IsActive { get; set; }
    }
}
