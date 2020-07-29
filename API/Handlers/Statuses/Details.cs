using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using MediatR;

namespace API.Handlers.Statuses
{
    public class Details
    {
        public class Query : IRequest<Status> {
            public int status_id { get; set; }
         }

        public class Handler : IRequestHandler<Query, Status>
        {
            private readonly ApplicationDBContext context;
            public Handler(ApplicationDBContext context)
            {
                this.context = context;
            }

            public async Task<Status> Handle(Query request, CancellationToken cancellationToken)
            {
                Status status = await context.status.FindAsync(request.status_id);

                if(status == null) throw new RestException(HttpStatusCode.NotFound, new {status = "Not found"});

                return status;
            }
        }
    }
}