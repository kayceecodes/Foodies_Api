using AutoMapper;
using foodies_api.Models.Dtos;
using foodies_api.Models.Entities;

namespace foodies_api.Models.Mappings;

public class GetUserLikeBusinesses : Profile
{
    public GetUserLikeBusinesses()
    {
        CreateMap<UserLikeBusinessDto, UserLikeBusiness>();
    }
}
