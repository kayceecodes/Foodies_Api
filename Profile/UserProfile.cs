using AutoMapper;
using foodies_api.Data;
using foodies_api.Models.Dtos;

namespace foodies_api;

public class UserProfile : Profile
{   
    public UserProfile()
    {
        CreateMap<UserDto, User>()
        .ForMember(dest => dest.I) 
    }
}
