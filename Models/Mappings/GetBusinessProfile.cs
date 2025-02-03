using AutoMapper;
using foodies_api.Models.Dtos.Responses;
using foodies_api.Models.Entities;

namespace foodies_api.Models.Mappings;

public class GetBusinessProfile : Profile
{
  public GetBusinessProfile() 
    {
        CreateMap<GetBusinessResponse, Business>()
        .ForMember(dest => dest.ExternalId, src => src.MapFrom(x => x.Id))
        .ForMember(dest => dest.ImageURL, src => src.MapFrom(x => x.Image_url))
        .ForMember(dest => dest.Latitude, src => src.MapFrom(x => x.Coordinates.Latitude))
        .ForMember(dest => dest.Longitude, src => src.MapFrom(x => x.Coordinates.Longitude));
    }
}
