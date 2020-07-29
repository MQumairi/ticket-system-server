using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using MediatR;

namespace API.Handlers.Tickets
{
    public class Edit
    {
        public class Command : IRequest
        {
            public int post_id { get; set; }

            public string description { get; set; }

            public string title { get; set; }

            public int product_id { get; set; }

            public int status_id { get; set; }

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
                if (ticket == null) throw new RestException(HttpStatusCode.NotFound, new { ticket = "Not found." });

                ticket.description = request.description ?? ticket.description;
                ticket.title = request.title ?? ticket.title;

                if(request.product_id != 0) 
                    ticket.product_id = request.product_id;

                if(request.status_id != 0)
                    ticket.status_id = request.status_id;

                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }
        }
    }
}