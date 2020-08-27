using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Archives
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
                //Handler logic goes here
                var archived_tickets = await context.tickets
                                                        .Include(ticket => ticket.product)
                                                        .Include(ticket => ticket.status)
                                                        .Include(ticket => ticket.author)
                                                            .ThenInclude(user => user.avatar)
                                                        .Where(ticket => ticket.is_archived).ToListAsync();

                return archived_tickets;
            }
        }
    }
}