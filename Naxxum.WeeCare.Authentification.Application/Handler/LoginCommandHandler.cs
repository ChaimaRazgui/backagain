using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Naxxum.WeeCare.Authentification.Application.Abstractions;
using Naxxum.WeeCare.Authentification.Application.DTOs.Auth;
using Naxxum.WeeCare.Authentification.Application.Options;
using Naxxum.WeeCare.Authentification.Application.Services;
using Naxxum.WeeCare.Authentification.Domain.Entities;
using Naxxum.WeeCare.Authentification.Domain.Enums;
using Naxxum.WeeCare.Authentification.Domain.Shared;

namespace Naxxum.WeeCare.Authentification.Application.Handler;
internal class LoginCommandHandler : IRequestHandler<LoginDto, OperationResult<UserWithTokenDto>>
{
    private readonly IUsersRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly JwtOptions _jwtOptions;
    private readonly ILogger<LoginCommandHandler> _logger;
    private readonly IRabitMQProducer _rabitMQProducer;
    private readonly TokenConsumer _tokenConsumer;
    private UserWithRoleAndNameDto _receivedUserData;
    public LoginCommandHandler(IUsersRepository userRepository, IMapper mapper, IOptions<JwtOptions> jwtOptions,
       ILogger<LoginCommandHandler> logger, IRabitMQProducer rabitMQProducer, TokenConsumer tokenConsumer)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
        _jwtOptions = jwtOptions.Value;
        _rabitMQProducer = rabitMQProducer;
        _tokenConsumer = tokenConsumer;
      
    }

    public async Task<OperationResult<UserWithTokenDto>> Handle(LoginDto request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Checking if email '{request.Email}' is exists.", request.Email);
        var user = await _userRepository.GetUserAsync(u => u.Email == request.Email, cancellationToken);

        if (user is null)
        {
            _logger.LogInformation("Email '{request.Email}' is not found.", request.Email);
            return DomainErrors.InvalidEmailOrPassword;
        }

        if (!Sha512PasswordService.ValidatePassword(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            _logger.LogInformation("Wrong password for email '{request.Email}'", request.Email);
            return DomainErrors.InvalidEmailOrPassword;
        }
        if (!user.Active)
        {
            _logger.LogInformation("User with email '{request.Email}' is not active.", request.Email);
            return OperationResult<UserWithTokenDto>.Failed(DomainErrors.ForbiddenAccess);
        }
        _rabitMQProducer.SendUserIdMessage(user.UserId);
        _logger.LogInformation("Logging in success for email '{request.Email}', generating JWT token...",
            request.Email);


        _tokenConsumer.MessageReceived += OnMessageReceived;
        while (_receivedUserData is null)
        {
            await Task.Delay(100);
        }

        var userWithToken = _mapper.Map<UserWithTokenDto>(user);

        userWithToken.Token = GenerateToken(user, _receivedUserData.Role, _receivedUserData.FullName);

        return userWithToken;
    }
   
      private void OnMessageReceived(object sender, UserWithRoleAndNameDto e)
        {
            _logger.LogInformation($"Received message from RabbitMQ: Role={e.Role}, fullName={e.FullName}");
        // Do something with the received message 
        _receivedUserData = e;
    }

    private string GenerateToken(User user, string role, string fullName)
    {
        var claims = new Claim[]
        {
        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new(ClaimTypes.Role, role),
        new(ClaimTypes.Name, fullName)
        };

        var expirationDate = DateTime.Now.AddHours(_jwtOptions.ExpirationInHours == 0 ? 5 : _jwtOptions.ExpirationInHours);

        return JwtService.GenerateToken(claims, _jwtOptions.Key, expirationDate);
    }
}