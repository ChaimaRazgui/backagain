using MediatR;
using Naxxum.WeeCare.Authentification.Application.Abstractions;
using Naxxum.WeeCare.Authentification.Application.DTOs.Users;
using Naxxum.WeeCare.Authentification.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naxxum.WeeCare.Authentification.Application.Handler
{
    public class GetUserListQueryHandler : IRequestHandler<GetUsersListQuery, List<GetDTO>>
    {
        private readonly IUsersRepository _userRepository;

        public GetUserListQueryHandler(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<GetDTO>> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
        {
            var users = _userRepository.GetAllUsers();

            var getDtos = users.Select(u => new GetDTO
            {
                UserId = u.UserId,
                Email = u.Email,
                Active = u.Active
            }).ToList();

            return getDtos;
        }
    }
}
