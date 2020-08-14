using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using MediatR;

namespace API.Handlers.Archives
{
    public class Add
    {
        public class Command : IRequest
        {
            //Properties
            public int ticket_id { get; set; }
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
                //Handler logic
                Ticket ticket_to_archive = await context.tickets.FindAsync(request.ticket_id);
                if(ticket_to_archive == null) throw new RestException(HttpStatusCode.NotFound, new {ticket = "Not found"});

                ticket_to_archive.is_archived = true;

                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }

        }
    }
}