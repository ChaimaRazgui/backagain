using System.Reflection.Metadata;
using MassTransit;
using MassTransit.Mediator;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Naxxum.WeeCare.Authentification.Application.DTOs.Auth;
using Naxxum.WeeCare.Authentification.Application.DTOs.Users;
using Naxxum.WeeCare.Authentification.Domain.Entities;
using Naxxum.WeeCare.Authentification.Domain.Shared;

namespace Naxxum.WeeCare.Authentification.API.Controllers;

public class AuthController : BaseApiController
{
    private readonly ISender _sender;
    
    public AuthController(ISender sender)
    {
        _sender = sender;
      
    }

   
    
    [HttpPost("register")]
    [Produces(typeof(UserDto))]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken)
    {
        return HandleResult(await _sender.Send(registerDto, cancellationToken));
    }

    
    [HttpPost("login")]
    [ProducesResponseType(typeof(UserWithTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
    {
        return HandleResult(await _sender.Send(loginDto, cancellationToken));
    }
    [HttpGet("GetUser")]
    public async Task<List<GetDTO>> GetUser()
    {
        return await _sender.Send(new GetUsersListQuery());
    }
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUserActiveStatus(int userId, bool active)
    {
        var command = new UpdateActiveStatusCommand { UserId = userId, Active = active };
        var result = await _sender.Send(command);

        if (result)
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }
}