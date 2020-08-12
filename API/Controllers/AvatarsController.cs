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
    }
}