using AutoMapper;
using BookStore.Entities;
using BookStore.Models;
using System.Runtime.InteropServices;

namespace BookStore.Utilities
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Book, BookViewModel>();
        }
    }
}
