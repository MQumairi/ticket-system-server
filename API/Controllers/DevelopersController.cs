using System.Collections.Generic;
using System.Threading.Tasks;
using API.Handlers.Developers;
using API.Models;
using API.Models.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "Admin,Developer")]
    [ApiController]
    [Route("api/[controller]")]
    public class DevelopersController : ControllerBase
    {
        private readonly IMediator mediator;
        public DevelopersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<List<User>>> List()
        {
            return await mediator.Send(new List.Query());
        }

        [HttpGet("{id}/tickets")]
        public async Task<ActionResult<List<TicketDto>>> ListAssignedTickets(string id)
        {
            return await mediator.Send(new ListAssignedTickets.Query { dev_id = id });
        }
    }
}