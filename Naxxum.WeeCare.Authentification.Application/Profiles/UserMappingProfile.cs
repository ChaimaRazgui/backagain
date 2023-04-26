using AutoMapper;
using Naxxum.WeeCare.Authentification.Application.DTOs.Auth;
using Naxxum.WeeCare.Authentification.Application.DTOs.Users;
using Naxxum.WeeCare.Authentification.Domain.Entities;

namespace Naxxum.WeeCare.Authentification.Application.Profiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForCtorParam(nameof(UserDto.RegistrationDate),
                dest => dest.MapFrom(u => u.CreatedAtUtc));

        CreateMap<User, UserWithTokenDto>();
    }
}