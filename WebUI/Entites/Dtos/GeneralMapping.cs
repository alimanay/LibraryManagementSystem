using AutoMapper;
using Entites.Dtos.BookDtos;
using Entites.Dtos.UserDtos;
using Entites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Dtos
{
    public class GeneralMapping : Profile
    {
        public  GeneralMapping()
        {
            //Book
            CreateMap<Book,BookListDto>().ReverseMap();
            CreateMap<Book,BookCreateDto>().ReverseMap();
            CreateMap<Book,BookUpdateDto>().ReverseMap();
            //User
            CreateMap<User, GetListUserDto>().ReverseMap();
            CreateMap<User,CreateUserDto>().ReverseMap();   
            CreateMap<User,UpdateUserDto>().ReverseMap();   

        }
    }
}
