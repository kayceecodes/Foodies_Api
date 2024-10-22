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
        // .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
        // .ForMember(dest => dest.Alias, src => src.MapFrom(x => x.Alias))
        .ForMember(dest => dest.ImageURL, src => src.MapFrom(x => x.Image_url))
        // .ForMember(dest => dest.URL, src => src.MapFrom(x => x.Url))
        // .ForMember(dest => dest.ReviewCount, src => src.MapFrom(x => x.ReviewCount))
        // .ForMember(dest => dest.Categories, src => src.MapFrom(x => x.Categories))
        // .ForMember(dest => dest.Rating, src => src.MapFrom(x => x.Rating))
        .ForMember(dest => dest.Latitude, src => src.MapFrom(x => x.Coordinates.Latitude))
        .ForMember(dest => dest.Longitude, src => src.MapFrom(x => x.Coordinates.Longitude));
        // .ForMember(dest => dest.StreetAddress, src => src.MapFrom(x => x.StreetAddress))
        // .ForMember(dest => dest.City, src => src.MapFrom(x => x.City))
        // .ForMember(dest => dest.State, src => src.MapFrom(x => x.State))
        // .ForMember(dest => dest.Zipcode, src => src.MapFrom(x => x.Zipcode))
        // .ForMember(dest => dest.Price, src => src.MapFrom(x => x.Price))
        // .ForMember(dest => dest.Phone, src => src.MapFrom(x => x.Phone));
    }
}
