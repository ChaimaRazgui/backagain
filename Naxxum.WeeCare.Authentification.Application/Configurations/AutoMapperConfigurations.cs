using System.Reflection;
using AutoMapper;
using Naxxum.WeeCare.Authentification.Application.Profiles;

namespace Naxxum.WeeCare.Authentification.Application.Configurations;

public static class AutoMapperConfigurations
{
    public static Assembly Assemblies => typeof(UserMappingProfile).Assembly;

    public static MapperConfiguration MapperConfiguration => new(c => c.AddMaps(Assemblies));
}