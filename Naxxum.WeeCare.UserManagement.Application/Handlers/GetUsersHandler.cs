using MediatR;
using Naxxum.WeeCare.UserManagement.Application.Queries;
using Naxxum.WeeCare.UserManagement.Application.Repositories;
using Naxxum.WeeCare.UserManagement.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naxxum.WeeCare.UserManagement.Application.Handlers
{
    public class GetUsersHandler : IRequestHandler<GetUsersListQuery, List<UsersDetails>>
    {
        private readonly IUserDetailsRepository _userRepository;

        public GetUsersHandler(IUserDetailsRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<List<UsersDetails>> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_userRepository.GetAllUsers());
        }
    }
}
