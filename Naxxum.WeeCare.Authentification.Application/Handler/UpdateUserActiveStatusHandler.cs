using MediatR;
using Naxxum.WeeCare.Authentification.Application.Abstractions;
using Naxxum.WeeCare.Authentification.Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naxxum.WeeCare.Authentification.Application.Handler
{
    public class UpdateUserActiveStatusHandler : IRequestHandler<UpdateActiveStatusCommand, bool>
{
        private readonly IUsersRepository _userRepository;

        public UpdateUserActiveStatusHandler(IUsersRepository userRepository)
    {
        _userRepository = userRepository;
    }

        public async Task<bool> Handle(UpdateActiveStatusCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user is null)
            {
                throw new Exception("User not found.");
            }

            user.Active = request.Active;

            await _userRepository.UpdateUserActiveStatusAsync(user.UserId, user.Active);

            return true;
        }

    }
}
