using System.Collections.Generic;
using System.Threading.Tasks;
using API.Handlers.Roles;
using API.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly IMediator mediator;
        public RolesController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<List<Role>>> List()
        {
            return await mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> Details(string id)
        {
            return await mediator.Send(new Details.Query { role_id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Add.Command command)
        {
            return await mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(string id, Edit.Command command)
        {
            command.role_id = id;
            return await mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(string id)
        {
            return await mediator.Send(new Delete.Command { role_id = id });
        }
    }
}