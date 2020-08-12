using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using API.Models.DTO;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Tickets
{
    public class Details
    {
        public class Query : IRequest<TicketDto>
        {
            public int post_id { get; set; }
        }

        public class Handler : IRequestHandler<Query, TicketDto>
        {
            private readonly ApplicationDBContext context;
            private readonly IMapper mapper;
            public Handler(ApplicationDBContext context, IMapper mapper)
            {
                this.mapper = mapper;
                this.context = context;
            }

            public async Task<TicketDto> Handle(Query request, CancellationToken cancellationToken)
            {
                Ticket ticket = await context.tickets
                                            .Include(ticket => ticket.user)
                                            .Include(ticket => ticket.product)
                                            .Include(ticket => ticket.status)
                                            .Include(ticket => ticket.attachment)
                                            .Include(ticket => ticket.comments)
                                                .ThenInclude(comment => comment.user)
                                            .Include(ticket => ticket.comments)
                                                .ThenInclude(comment => comment.attachment)
                                            .FirstOrDefaultAsync(ticket => ticket.post_id == request.post_id);
                if (ticket == null) throw new RestException(HttpStatusCode.NotFound, new { ticket = "Not found." });

                var ticketToReturn = mapper.Map<Ticket, TicketDto>(ticket);

                return ticketToReturn;
            }
        }
    }
}