using AutoMapper;
using foodies_api.Models.Dtos;
using foodies_api.Models.Dtos.Auth;
using foodies_api.Models.Dtos.Requests;
using foodies_api.Models.Entities;


namespace foodies_api.Models.Mappings;

public class PostUserProfile : Profile
{   
    public string Name { get; set;}
    public PostUserProfile()
    {
        CreateMap<RegistrationRequest, User>();
        // CreateMap<UserDto, User>()
        // .ForMember(dest => dest.FirstAndLastName, src => src.MapFrom(x => x.FirstName + " " + x.LastName));


        CreateMap<User, RegistrationRequest>();
        // CreateMap<User, UserDto>()
        // .ForMember(dest => dest.FirstName, src => src.MapFrom<FirstNameResolver>())
        // .ForMember(dest => dest.LastName, src => src.MapFrom<LastNameResolver>());
    }    
}

// public class FirstNameResolver : IValueResolver<User, UserDto, string>
// {
//     public string Resolve(User source, UserDto destination, string destMember, ResolutionContext context)
//     {
//         if (source.FirstAndLastName == null || !source.FirstAndLastName.Contains(' '))
//             return source.FirstAndLastName;

//         return source.FirstAndLastName.Split(' ')[0];
//     }
// }

// public class LastNameResolver : IValueResolver<User, UserDto, string>
// {
//     public string Resolve(User source, UserDto destination, string destMember, ResolutionContext context)
//     {
//         if (source.FirstAndLastName == null || !source.FirstAndLastName.Contains(' '))
//             return source.FirstAndLastName;

//         return source.FirstAndLastName.Split(' ')[1];
//     }
// }