using System.Collections.Generic;
using System.Threading.Tasks;
using API.Handlers.Comments;
using API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator mediator;

        public CommentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Comment>>> List()
        {
            return await mediator.Send(new List.Query());
        }

        [HttpGet("{id}/comments")]
        public async Task<ActionResult<List<Comment>>> ListPostComments(int id)
        {
            return await mediator.Send(new Handlers.Comments.ListPostComments.Query { parent_id = id });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> Details(int id)
        {
            return await mediator.Send(new Details.Query { id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            return await mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(int id, Edit.Command command)
        {
            command.post_id = id;
            return await mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(int id)
        {
            return await mediator.Send(new Delete.Command { post_id = id });
        }


    }
}