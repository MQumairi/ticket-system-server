using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using MediatR;

namespace API.Handlers.Tickets
{
    public class ChangeTicketStatus
    {
        public class Command : IRequest
        {
            //Properties
            public int ticket_id { get; set; }

            public int status_id {get; set;}
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
                var ticket = await context.tickets.FindAsync(request.ticket_id);
                if(ticket == null) throw new RestException(HttpStatusCode.NotFound, new {ticket = "Not found"});

                var status = await context.status.FindAsync(request.status_id);
                if(status == null) throw new RestException(HttpStatusCode.NotFound, new {status = "Not found"});

                ticket.status = status;
                
                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }

        }
    }
}