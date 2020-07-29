using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Tickets
{
    public class List
    {
        public class Query : IRequest<List<Ticket>> { }

        public class Handler : IRequestHandler<Query, List<Ticket>>
        {
            private readonly ApplicationDBContext context;
            public Handler(ApplicationDBContext context)
            {
                this.context = context;
            }


            public async Task<List<Ticket>> Handle(Query request, CancellationToken cancellationToken)
            {
                var tickets = await context.tickets.ToListAsync();
                return tickets;
            }
        }
    }
}