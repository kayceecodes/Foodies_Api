using AutoMapper;
using foodies_api.Models.Dtos;
using foodies_api.Models.Entities;


namespace foodies_api.Models.Mappings;

public class PostUserLikeBusinessProfile : Profile
{
    public PostUserLikeBusinessProfile() 
    {
        CreateMap<UserLikeBusinessDto, UserLikeBusiness>();
        // .ForMember(dest => dest.UserId, src => src.MapFrom(x => x.UserId))
        // .ForMember(dest => dest.FullName, src => src.MapFrom(x => x.FullName))
        // .ForMember(dest => dest.BusinessId, src => src.MapFrom(x => x.BusinessId))
        // .ForMember(dest => dest.BusinessName, src => src.MapFrom(x => x.BusinessName));
    }
}
