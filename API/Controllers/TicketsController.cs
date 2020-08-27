using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Handlers.Tickets;
using API.Models;
using API.Models.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<List<TicketDto>>> List()
        {
            return await mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDto>> Details(int id)
        {
            return await mediator.Send(new Details.Query { post_id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create([FromForm] Create.Command command)
        {
            return await mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit([FromForm] Edit.Command command, int id)
        {
            command.post_id = id;
            return await mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(int id)
        {
            return await mediator.Send(new Delete.Command { post_id = id });
        }

        //Filter tickets
        [HttpPost("filter")]
        public async Task<ActionResult<List<TicketDto>>> Filter(FilterTickets.Query query)
        {
            return await mediator.Send(query);
        }

        //Developer stuff
        [Authorize(Roles = "Admin,Developer")]
        [HttpPut("{id}/assign")]
        public async Task<ActionResult<Unit>> AssignTicket(int id, AssignTicket.Command command)
        {
            command.ticket_id = id;
            return await mediator.Send(command);
        }

        [HttpPut("{id}/unassign")]
        public async Task<ActionResult<Unit>> UnassignTicket(int id)
        {
            return await mediator.Send(new UnassignTicket.Command { ticket_id = id });
        }

        [Authorize(Roles = "Admin,Developer")]
        [HttpPut("{id}/status-change")]
        public async Task<ActionResult<Unit>> ChangeTicketStatus(int id, ChangeTicketStatus.Command command)
        {
            command.ticket_id = id;
            return await mediator.Send(command);
        }

        [Authorize(Roles = "Admin,Developer")]
        [HttpPut("{id}/manage")]
        public async Task<ActionResult<Unit>> Manage(int id, Manage.Command command)
        {
            command.post_id = id;
            return await mediator.Send(command);
        }

    }
}