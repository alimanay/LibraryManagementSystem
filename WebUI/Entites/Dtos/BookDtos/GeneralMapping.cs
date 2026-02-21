using AutoMapper;
using Entites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Dtos.BookDtos
{
    public class GeneralMapping : Profile
    {
        public  GeneralMapping()
        {
            CreateMap<Book,BookListDto>().ReverseMap();
            CreateMap<Book,BookCreateDto>().ReverseMap();
            CreateMap<Book,BookUpdateDto>().ReverseMap();
        }
    }
}
