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
        [AllowAnonymous]
        public async Task<ActionResult<List<Ticket>>> List()
        {
            return await mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDto>> Details(int id)
        {
            return await mediator.Send(new Details.Query { post_id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            return await mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Edit.Command command, int id)
        {
            command.post_id = id;
            return await mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(int id)
        {
            return await mediator.Send(new Delete.Command { post_id = id });
        }

        [HttpGet("{id}/comments")]
        public async Task<ActionResult<List<Comment>>> ListPostComments(int id)
        {
            return await mediator.Send(new Handlers.Comments.ListPostComments.Query {parent_id = id});
        }
    }
}