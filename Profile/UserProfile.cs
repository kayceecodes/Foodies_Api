using AutoMapper;
using foodies_api.Data;
using foodies_api.Models.Dtos;

namespace foodies_api;

public class UserProfile : Profile
{   
    public UserProfile()
    {
        CreateMap<UserDto, User>()
        .ForMember(dest => dest.FirstAndLastName, src => src.MapFrom(x => x.FirstName + " " + x.LastName))
        .ForMember(dest => dest.UserName, src => src.MapFrom(x => x.Username))
        .ForMember(dest => dest.Email, src => src.MapFrom(x => x.Email))
        .ForMember(dest => dest.Password, src => src.MapFrom(x => x.Password)); 
    }
}
