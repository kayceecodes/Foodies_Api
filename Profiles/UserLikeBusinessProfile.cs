using AutoMapper;
using foodies_api.Models;
using foodies_api.Models.Dtos;

namespace foodies_api.Profiles;

public class PostUserLikeBusinessProfile : Profile
{
    public PostUserLikeBusinessProfile() 
    {
        CreateMap<UserLikeBusinessDto, UserLikeBusiness>()
        .ForMember(dest => dest.UserId, src => src.MapFrom(x => x.UserId))
        .ForMember(dest => dest.FullName, src => src.MapFrom(x => x.FullName))
        .ForMember(dest => dest.BusinessId, src => src.MapFrom(x => x.BusinessId))
        .ForMember(dest => dest.BusinessName, src => src.MapFrom(x => x.BusinessName));
    }
}
