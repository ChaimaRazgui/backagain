namespace Naxxum.WeeCare.Authentification.Application.DTOs.Auth;

public record UserWithTokenDto(int Id, string Email)
{
    public string? Token { get; set; }
};