using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using API.Models.DTO;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Developers
{
    public class ListAssignedTickets
    {
        public class Query : IRequest<List<TicketDto>>
        {
            public string dev_id { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<TicketDto>>
        {
            private readonly ApplicationDBContext context;
            private readonly UserManager<User> userManager;
            private readonly IMapper mapper;
            public Handler(ApplicationDBContext context, UserManager<User> userManager, IMapper mapper)
            {
                this.mapper = mapper;
                this.userManager = userManager;
                this.context = context;
            }

            public async Task<List<TicketDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                //Handler logic goes here
                var user = await context.Users.FindAsync(request.dev_id);
                if (user == null) throw new RestException(HttpStatusCode.NotFound, new { user = "Not found" });

                List<string> user_roles = await userManager.GetRolesAsync(user) as List<string>;
                if (!user_roles.Contains("Developer")) throw new RestException(HttpStatusCode.BadRequest, new { user = "This user is not a developer!" });

                var tickets = await context.tickets.Where(ticket => ticket.developer_id == user.Id).ToListAsync();

                List<TicketDto> ticketsDto = new List<TicketDto>();

                foreach (var ticket in tickets)
                {
                    TicketDto ticketDto = mapper.Map<Ticket, TicketDto>(ticket);
                    ticketsDto.Add(ticketDto);
                }

                return ticketsDto;
            }
        }
    }
}