using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using MediatR;

namespace API.Handlers.Tickets
{
    public class Details
    {
        public class Query : IRequest<Ticket>
        {
            public int post_id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Ticket>
        {
            private readonly ApplicationDBContext context;
            public Handler(ApplicationDBContext context)
            {
                this.context = context;
            }

            public async Task<Ticket> Handle(Query request, CancellationToken cancellationToken)
            {
                Ticket ticket = await context.tickets.FindAsync(request.post_id);
                if(ticket == null) throw new RestException (HttpStatusCode.NotFound, new{ticket = "Not found."});
                
                return ticket;
            }
        }
    }
}