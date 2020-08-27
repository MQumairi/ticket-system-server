using System.Collections.Generic;
using System.Threading.Tasks;
using API.Handlers.Archives;
using API.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class ArchivesController : ControllerBase
    {
        private readonly IMediator mediator;

        public ArchivesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<List<Ticket>>> List()
        {
            return await mediator.Send(new List.Query());
        }

        [Authorize(Roles = "Admin,Developer")]
        [HttpPut("{id}/add")]
        public async Task<ActionResult<Unit>> Add(int id)
        {
            return await mediator.Send(new Add.Command { ticket_id = id });
        }

        [Authorize(Roles = "Admin,Developer")]
        [HttpPut("{id}/remove")]
        public async Task<ActionResult<Unit>> Remove(int id)
        {
            return await mediator.Send(new Remove.Command { ticket_id = id });
        }
    }
}