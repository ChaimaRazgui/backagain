using System.ComponentModel.DataAnnotations;

namespace Naxxum.WeeCare.Authentification.Application.DTOs.Features;

public record CreateFeatureDto([Required, StringLength(50)] string Name, [StringLength(1000)] string? Description);