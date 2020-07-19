using System.Collections.Generic;
using System.Threading.Tasks;
using API.Handlers.Tickets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly IMediator mediator;

        public TicketsController(IMediator mediator)
        {
            this.mediator = mediator;

        }

        // GET api/tickets
        [HttpGet]
        public async Task<ActionResult<List<Ticket>>> List()
        {
            return await mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public ActionResult<string> GetOne(int id)
        {
            return "The query para is " + id;
        }
    }
}