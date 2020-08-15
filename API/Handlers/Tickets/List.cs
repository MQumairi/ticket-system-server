using System.Collections.Generic;
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
        public class Query : IRequest<List<TicketDto>> { }

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
                var tickets = await context.tickets
                                            .Include(ticket => ticket.product)
                                            .Include(ticket => ticket.status)
                                            .Include(ticket => ticket.author)
                                                .ThenInclude(user => user.avatar)
                                            .ToListAsync();

                List<TicketDto> ticketDtos = new List<TicketDto>();

                foreach(Ticket ticket in tickets) {
                    TicketDto ticketDto = mapper.Map<Ticket, TicketDto>(ticket);
                    ticketDtos.Add(ticketDto);
                }

                return ticketDtos;
            }
        }
    }
}