using AutoMapper;
using Domain.Entities;
using LibraryAPI.DTO;

namespace LibraryAPI.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateMap<Book, BookResponseDto>();
        this.CreateMap<BookRequestDto, Book>();
    }
}