using MediatR;
using Naxxum.WeeCare.Authentification.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naxxum.WeeCare.Authentification.Application.DTOs.Users
{
    public record GetUsersListQuery : IRequest<List<GetDTO>>;
}
