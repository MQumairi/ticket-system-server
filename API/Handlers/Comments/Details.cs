using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using MediatR;

namespace API.Handlers.Comments
{
    public class Details
    {
        public class Query : IRequest<Comment> {
            public int id { get; set; }
         }

        public class Handler : IRequestHandler<Query, Comment>
        {
            private readonly ApplicationDBContext context;
            public Handler(ApplicationDBContext context)
            {
                this.context = context;
            }

            public async Task<Comment> Handle(Query request, CancellationToken cancellationToken)
            {
                //Handler logic goes here
                var comment = await context.comments.FindAsync(request.id);
                if(comment == null)
                    throw new RestException(HttpStatusCode.NotFound, new {comment = "Not found"});

                return comment;
            }
        }
    }
}