using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using MediatR;

namespace API.Handlers.Tickets
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int post_id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ApplicationDBContext context;
            public Handler(ApplicationDBContext context)
            {
                this.context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                Ticket ticket = await context.tickets.FindAsync(request.post_id);
                if(ticket == null) throw new RestException (HttpStatusCode.NotFound, new{ticket = "Not found."});

                context.tickets.Remove(ticket);

                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }
        }
    }
}