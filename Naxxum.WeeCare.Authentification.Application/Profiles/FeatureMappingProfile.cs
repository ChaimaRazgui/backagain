using AutoMapper;
using Naxxum.WeeCare.Authentification.Application.DTOs.Features;
using Naxxum.WeeCare.Authentification.Domain.Entities;

namespace Naxxum.WeeCare.Authentification.Application.Profiles;

public class FeatureMappingProfile : Profile
{
    public FeatureMappingProfile()
    {
        CreateMap<Feature, FeatureDto>();
    }
}