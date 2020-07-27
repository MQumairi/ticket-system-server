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
    [AllowAnonymous]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;

        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;

        }
        [HttpPost("login")]
        public async Task<ActionResult<CurrentUser>> Create(Login.Query query)
        {
            return await mediator.Send(query);
        }

        [HttpPost("register")]
        public async Task<ActionResult<CurrentUser>> Create(Register.Command command)
        {
            return await mediator.Send(command);
        }
    }
}