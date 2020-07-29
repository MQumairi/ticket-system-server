using System;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using MediatR;

namespace API.Handlers.Tickets
{
    public class Create
    {
        public class Command : IRequest
        {
            // public Guid ticketNumb { get; set; }

            public string date_time { get; set; }

            public string description { get; set; }

            public int author_id { get; set; }

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
                //Handler logic
                Ticket ticket = new Ticket
                {
                    date_time = DateTime.Parse(request.description, System.Globalization.CultureInfo.InvariantCulture),
                    description = request.description,
                    author_id = request.author_id,
                    title = request.title,
                    product_id = request.product_id,
                    status_id = request.status_id
                };

                context.tickets.Add(ticket);

                bool success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }
        }
    }
}