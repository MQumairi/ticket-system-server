using System.Threading.Tasks;
using API.Handlers.ACPSettingsHandlers;
using API.Models.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ACPController : ControllerBase
    {
        private readonly IMediator mediator;
        public ACPController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<ACPSettingsDto>> Details()
        {
            return await mediator.Send(new Details.Query { });
        }

        [AllowAnonymous]
        [HttpGet("registration-locked")]
        public async Task<ActionResult<bool>> RegistrationLocked()
        {
            return await mediator.Send(new RegistrationLocked.Query { });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            return await mediator.Send(command);
        }

        [HttpPut]
        public async Task<ActionResult<Unit>> Edit(Edit.Command command)
        {
            return await mediator.Send(command);
        }
    }
}