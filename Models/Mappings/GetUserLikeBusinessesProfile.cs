using AutoMapper;
using foodies_api.Models.Dtos;
using foodies_api.Models.Entities;

namespace foodies_api.Models.Mappings;

public class GetUserLikeBusinessesProfile : Profile
{
    public GetUserLikeBusinessesProfile()
    {
        CreateMap<UserLikeBusinessDto, UserLikeBusiness>();
    }
}
