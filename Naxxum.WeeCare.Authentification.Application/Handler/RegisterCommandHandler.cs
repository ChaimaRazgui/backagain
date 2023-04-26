using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Naxxum.WeeCare.Authentification.Application.Abstractions;
using Naxxum.WeeCare.Authentification.Application.DTOs.Auth;
using Naxxum.WeeCare.Authentification.Application.DTOs.Users;
using Naxxum.WeeCare.Authentification.Application.Services;
using Naxxum.WeeCare.Authentification.Domain.Entities;
using Naxxum.WeeCare.Authentification.Domain.Enums;
using Naxxum.WeeCare.Authentification.Domain.Shared;

namespace Naxxum.WeeCare.Authentification.Application.Handlers.Command.Auth;

internal class RegisterCommandHandler : IRequestHandler<RegisterDto, OperationResult<UserDto>>
{
    private readonly IUsersRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<RegisterCommandHandler> _logger;
    private readonly IRabitMQProducer _rabitMQProducer;
    public RegisterCommandHandler(IUsersRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper,
        ILogger<RegisterCommandHandler> logger, IRabitMQProducer rabitMQProducer)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _rabitMQProducer = rabitMQProducer;
    }

    public async Task<OperationResult<UserDto>> Handle(RegisterDto request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Checking for email duplication");
        if (await _userRepository.AnyAsync(u => u.Email == request.Email, cancellationToken))
        {
            _logger.LogInformation("Email '{Email}' you have provided is already exists.", request.Email);
            return DomainErrors.EmailIsAlreadyExists;
        }

        _logger.LogInformation("Email is not exists, creating new user with email and encrypted password.");
        var (passwordHash, passwordSalt) = Sha512PasswordService.Generate(request.Password);

        var user = User.Create(request.Email, passwordHash, passwordSalt,false);
        _userRepository.Add(user);

        _logger.LogInformation("Saving new user to database.");
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var message = new UserCreated
        {
            UserId = user.UserId,
            Email = user.Email,
            fullName = request.fullName
        };
        _rabitMQProducer.SendProductMessage(message);
        _logger.LogInformation("Received message: UserId={UserId}, Email={Email}, fullName={fullName}",
         message.UserId, message.Email, message.fullName);
        _logger.LogInformation("New user has been created successfully!");
        return _mapper.Map<UserDto>(user);
    }
}