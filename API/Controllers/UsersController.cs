using System.Threading.Tasks;
using API.Handlers.Users;
using API.Models;
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

        [HttpGet]
        public async Task<ActionResult<CurrentUser>> CurrentUser()
        {
            return await mediator.Send(new CurrentUserHandler.Query());
        }
    }
}