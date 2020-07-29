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

        [HttpGet("all")]
        public async Task<ActionResult<List<Comment>>> List()
        {
            return await mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Comment>>> ListPostComments(int id)
        {
            return await mediator.Send(new ListPostComments.Query {parent_id = id});
        }
    }
}