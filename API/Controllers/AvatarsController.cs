using System.Threading.Tasks;
using API.Handlers.Avatars;
using API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvatarsController : ControllerBase
    {
        private readonly IMediator mediator;
        public AvatarsController(IMediator mediator)
        {
            this.mediator = mediator;

        }

        // GET api/values
        [HttpPost]
        public async Task<ActionResult<Photo>> Create([FromForm] Add.Command command)
        {
            return await mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(string id)
        {
            return await mediator.Send(new Delete.Command { Id = id });
        }
    }
}