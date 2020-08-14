using System.Collections.Generic;
using System.Threading.Tasks;
using API.Handlers.Users;
using API.Models;
using API.Models.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;

        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;

        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<CurrentUser>> Login(Login.Query query)
        {
            return await mediator.Send(query);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<CurrentUser>> Register(Register.Command command)
        {
            return await mediator.Send(command);
        }

        [HttpGet("profile")]
        public async Task<ActionResult<CurrentUser>> CurrentUser()
        {
            return await mediator.Send(new CurrentUserHandler.Query());
        }

        [HttpPut("profile")]
        public async Task<ActionResult<Unit>> Edit(EditPorfile.Command command)
        {
            return await mediator.Send(command);
        }


        //Admin stuff
        [Authorize(Roles = "Admin")]
        [HttpGet("list")]
        public async Task<ActionResult<List<UserDto>>> List()
        {
            return await mediator.Send(new List.Query());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("list/{role_name}")]
        public async Task<ActionResult<List<UserDto>>> RoleUsers(string role_name)
        {
            return await mediator.Send(new RoleUsers.Query { role_name = role_name });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/assign")]
        public async Task<ActionResult<Unit>> AssignRole(string id, AssignRole.Command command)
        {
            command.user_id = id;
            return await mediator.Send(command);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/unassign")]
        public async Task<ActionResult<Unit>> UnassignRole(string id, UnassignRole.Command command)
        {
            command.user_id = id;
            return await mediator.Send(command);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> Details(string id)
        {
            return await mediator.Send(new Details.Query { user_id = id });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(string id, Edit.Command command)
        {
            command.user_id = id;
            return await mediator.Send(command);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(string id)
        {
            return await mediator.Send(new Delete.Command { user_id = id });
        }
    }
}