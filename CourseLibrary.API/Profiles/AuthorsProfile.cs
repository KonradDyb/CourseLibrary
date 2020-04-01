using AutoMapper;
using CourseLibrary.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLibrary.API.Profiles
{
    public class AuthorsProfile : Profile
    {
        public AuthorsProfile()
        {   // by default it will ignore null reference exceptions from source to target
            CreateMap<Entities.Author, Models.AuthorDto>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(
                    dest => dest.Age,
                    opt => opt.MapFrom(src => src.DateOfBirth.GetCurrentAge()));

            // For Member "Name" on the destination object, we want it to be mapped from the source's
            // FirstName and LastName

            CreateMap<Models.AuthorForCreationDto, Entities.Author>();

            CreateMap<Entities.Author, Models.AuthorFullDto>();
        }
    }
}
