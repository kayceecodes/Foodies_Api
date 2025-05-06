using AutoMapper;
using foodies_api.Models.Dtos.Requests;
using foodies_api.Models.Dtos.Responses;
using foodies_api.Models.Entities;


namespace foodies_api.Models.Mappings;

public class PostUserProfile : Profile
{   
    public string Name { get; set;}
    public PostUserProfile()
    {
        CreateMap<RegisterRequest, User>();
        // CreateMap<UserDto, User>()
        // .ForMember(dest => dest.FirstAndLastName, src => src.MapFrom(x => x.FirstName + " " + x.LastName));

        CreateMap<RegisterRequest, RegisterResponse>();
        // CreateMap<User, UserDto>()
        // .ForMember(dest => dest.FirstName, src => src.MapFrom<FirstNameResolver>())
        // .ForMember(dest => dest.LastName, src => src.MapFrom<LastNameResolver>());

        CreateMap<User, LoginResponse>();
        CreateMap<LoginResponse, User>();
        }    
}