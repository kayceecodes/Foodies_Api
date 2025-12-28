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

        CreateMap<User, RegisterResponse>();

        CreateMap<RegisterRequest, RegisterResponse>();

        CreateMap<User, LoginResponse>();
        CreateMap<LoginResponse, User>();    
    }    
}