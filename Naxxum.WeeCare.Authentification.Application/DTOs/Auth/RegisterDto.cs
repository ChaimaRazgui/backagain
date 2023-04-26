using System.ComponentModel.DataAnnotations;
using MediatR;
using Naxxum.WeeCare.Authentification.Application.DTOs.Users;
using Naxxum.WeeCare.Authentification.Domain.Shared;

namespace Naxxum.WeeCare.Authentification.Application.DTOs.Auth;

public record RegisterDto(string fullName,[Required, StringLength(100), 
    EmailAddress] string Email,
    [Required, StringLength(50)] string Password) : IRequest<OperationResult<UserDto>>;