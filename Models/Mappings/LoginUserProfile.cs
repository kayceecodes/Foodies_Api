using AutoMapper;
using foodies_api.Models.Dtos;
using foodies_api.Models.Dtos.Auth;
using foodies_api.Models.Dtos.Requests;
using foodies_api.Models.Dtos.Responses;
using foodies_api.Models.Entities;


namespace foodies_api.Models.Mappings;

public class LoginUserProfile : Profile
{   
    public string Name { get; set;}
    public LoginUserProfile()
    {
        CreateMap<LoginRequest, LoginResponse>();
    }    
}
