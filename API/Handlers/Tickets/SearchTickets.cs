using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using API.Models.DTO;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Tickets
{
    public class SearchTickets
    {
        public class Query : IRequest<List<TicketDto>>
        {
            public string search_query { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<TicketDto>>
        {
            private readonly ApplicationDBContext context;
            private readonly IMapper mapper;
            public Handler(ApplicationDBContext context, IMapper mapper)
            {
                this.mapper = mapper;
                this.context = context;
            }

            public async Task<List<TicketDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                //Handler logic goes here
                var tickets = await context.tickets
                                        .Include(ticket => ticket.status)
                                        .Include(ticket => ticket.product)
                                        .Include(ticket => ticket.author)
                                        .Where(ticket =>
                                            ticket.title.Contains(request.search_query) ||
                                            ticket.description.Contains(request.search_query) ||
                                            ticket.status.status_text.Contains(request.search_query) ||
                                            ticket.product.product_name.Contains(request.search_query) ||
                                            ticket.author.UserName.Contains(request.search_query)
                                            )
                                        .ToListAsync();

                var ticketsDto = mapper.Map<List<Ticket>, List<TicketDto>>(tickets);

                return ticketsDto;
            }
        }
    }
}