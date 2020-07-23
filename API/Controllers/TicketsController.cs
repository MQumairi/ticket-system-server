using System;
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
        public async Task<ActionResult<Ticket>> Details(Guid id)
        {
            return await mediator.Send(new Details.Query { ticketNumb = id });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            return await mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Edit.Command command, Guid id)
        {
            command.ticketNumb = id;
            return await mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await mediator.Send(new Delete.Command { ticketNumb = id });
        }
    }
}