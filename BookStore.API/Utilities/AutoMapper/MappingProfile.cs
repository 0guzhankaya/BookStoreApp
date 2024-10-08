using AutoMapper;
using BookStore.Entities.DataTransferObjects;
using BookStore.Entities.Models;

namespace BookStore.API.Utilities.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookDtoForUpdate, Book>().ReverseMap();
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<BookDtoForInsertion, Book>();
        }
    }
}
