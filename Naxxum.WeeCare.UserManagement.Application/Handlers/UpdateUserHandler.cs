using MediatR;
using Naxxum.WeeCare.UserManagement.Application.Commands;
using Naxxum.WeeCare.UserManagement.Application.Repositories;
using Naxxum.WeeCare.UserManagement.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naxxum.WeeCare.UserManagement.Application.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UsersDetails>
    {
        private readonly IUserDetailsRepository _userDetailsRepository;

        public UpdateUserCommandHandler(IUserDetailsRepository userDetailsRepository)
        {
            _userDetailsRepository = userDetailsRepository;
        }

        public async Task<UsersDetails> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var userDetails = _userDetailsRepository.GetUserById(request.UserId);

            if (userDetails != null)
            {
                userDetails.fullName = request.fullName;
                userDetails.Role = request.Role;

                _userDetailsRepository.UpdateUser(userDetails);
            }

            return userDetails;
        }
    }
}
