using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace API.Handlers.Tickets
{
    public class Create
    {
        public class Command : IRequest
        {
            public int Id { get; set; }

            public int authorId { get; set; }

            public string status { get; set; }

            public string product { get; set; }

            public string title { get; set; }

            public string date { get; set; }

            public string description { get; set; }
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
                Ticket ticket = new Ticket {
                    Id = request.Id,
                    authorId = request.authorId,
                    status = request.status,
                    product = request.product,
                    title = request.title,
                    date = request.date,
                    description = request.description
                };

                context.tickets.Add(ticket);

                bool success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;
                
                throw new Exception("Problem saving data");
            }
        }
    }
}