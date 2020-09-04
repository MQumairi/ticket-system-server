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
    public class List
    {
        public class TicketEnvelop
        {
            public int ticketCount { get; set; }
            public List<TicketDto> tickets { get; set; }
        }

        public class Query : IRequest<TicketEnvelop>
        {
            public Query(int? offset, int? limit)
            {
                this.limit = limit;
                this.offset = offset;

            }
            public int? limit { get; set; }
            public int? offset { get; set; }
        }

        public class Handler : IRequestHandler<Query, TicketEnvelop>
        {
            private readonly ApplicationDBContext context;
            private readonly IMapper mapper;
            public Handler(ApplicationDBContext context, IMapper mapper)
            {
                this.mapper = mapper;
                this.context = context;
            }


            public async Task<TicketEnvelop> Handle(Query request, CancellationToken cancellationToken)
            {
                var queryable = context.tickets.Include(ticket => ticket.product)
                                                .Include(ticket => ticket.status)
                                                .Include(ticket => ticket.author)
                                                    .ThenInclude(user => user.avatar)
                                                .Where(ticket => !ticket.is_archived)
                                                .OrderBy(ticket => ticket.date_time)
                                                .AsQueryable();

                var tickets = await queryable
                                    .Skip(request.offset ?? 0)
                                    .Take(request.limit ?? 5)
                                    .ToListAsync();

                List<TicketDto> ticketDtos = new List<TicketDto>();

                foreach (Ticket ticket in tickets)
                {
                    TicketDto ticketDto = mapper.Map<Ticket, TicketDto>(ticket);
                    ticketDtos.Add(ticketDto);
                }

                TicketEnvelop ticketEnvelop = new TicketEnvelop
                {
                    ticketCount = queryable.Count(),
                    tickets = ticketDtos
                };

                return ticketEnvelop;
            }
        }
    }
}