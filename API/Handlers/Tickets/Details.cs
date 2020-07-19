using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace API.Handlers.Tickets
{
    public class Details
    {
        public class Query : IRequest<Ticket>
        {
            public int Id { get; set; }
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
                Ticket ticket = await context.tickets.FindAsync(request.Id);
                
                return ticket;
            }
        }
    }
}