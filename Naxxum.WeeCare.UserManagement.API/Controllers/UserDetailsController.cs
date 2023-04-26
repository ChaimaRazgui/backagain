using MediatR;
using Microsoft.AspNetCore.Mvc;
using Naxxum.WeeCare.UserManagement.Application.Commands;
using Naxxum.WeeCare.UserManagement.Application.Queries;
using Naxxum.WeeCare.UserManagement.Domain.Entites;

namespace Naxxum.WeeCare.UserManagement.API.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class UserDetailsController
    {
        private readonly IMediator _mediator;

        public UserDetailsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("GetUsers")]
        public async Task<List<UsersDetails>> Get()
        {
            return await _mediator.Send(new GetUsersListQuery());
        }
       

        [HttpPut("update")]
        public async Task<ActionResult<UsersDetails>> Update(UpdateUserCommand command)
        {
            return await _mediator.Send(command);
        }
        [HttpDelete("delete")]
        public async Task<bool> DeleteUser(int UserId)
        {
            return await _mediator.Send(new DeleteUserCommand() { UserId = UserId });
        }

    }
}
