using MediatR;
using Naxxum.WeeCare.UserManagement.Application.Commands;
using Naxxum.WeeCare.UserManagement.Application.DTOs;
using Naxxum.WeeCare.UserManagement.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naxxum.WeeCare.UserManagement.Application.Handlers
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserDetailsRepository _userRepository;
        private readonly IRabitMQProducerD _rabitMQProducer;
        public DeleteUserHandler(IUserDetailsRepository userRepository,IRabitMQProducerD rabitMQProducer)
        {
            _userRepository = userRepository;
            _rabitMQProducer = rabitMQProducer;

        }

        public Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            {
                var userDetails = _userRepository.GetUserById(request.UserId);
                if (userDetails is null)
                    return default;
                var message = new UserDeleted
                {
                    UserId = request.UserId,
                };
                _rabitMQProducer.SendProductMessage(message);
                return Task.FromResult(_userRepository.DeleteUser(userDetails.UserId));
            }
        }
    }
}
