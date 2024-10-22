using AutoMapper;
using foodies_api.Models.Dtos.Auth;
using foodies_api.Models.Entities;


namespace foodies_api.Models.Mappings;

public class PostUserProfile : Profile
{   
    public string Name { get; set;}
    public PostUserProfile()
    {
        CreateMap<UserDto, User>()
        .ForMember(dest => dest.FirstAndLastName, src => src.MapFrom(x => x.FirstName + " " + x.LastName))
        .ForMember(dest => dest.Username, src => src.MapFrom(x => x.Username))
        .ForMember(dest => dest.Email, src => src.MapFrom(x => x.Email))
        .ForMember(dest => dest.Password, src => src.MapFrom(x => x.Password));

        CreateMap<User, UserDto>() 
        .ForMember(dest => dest.FirstName, src => src.MapFrom<FirstNameResolver>())
        .ForMember(dest => dest.LastName, src => src.MapFrom<LastNameResolver>())
        .ForMember(dest => dest.Username, src => src.MapFrom(x => x.Username))
        .ForMember(dest => dest.Email, src => src.MapFrom(x => x.Email))
        .ForMember(dest => dest.Password, src => src.MapFrom(x => x.Password));
    }    
}

public class FirstNameResolver : IValueResolver<User, UserDto, string>
{
    public string Resolve(User source, UserDto destination, string destMember, ResolutionContext context)
    {
        if (source.FirstAndLastName == null || !source.FirstAndLastName.Contains(' '))
            return source.FirstAndLastName;

        return source.FirstAndLastName.Split(' ')[0];
    }
}

public class LastNameResolver : IValueResolver<User, UserDto, string>
{
    public string Resolve(User source, UserDto destination, string destMember, ResolutionContext context)
    {
        if (source.FirstAndLastName == null || !source.FirstAndLastName.Contains(' '))
            return source.FirstAndLastName;

        return source.FirstAndLastName.Split(' ')[1];
    }
}