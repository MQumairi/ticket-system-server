using System;
using System.Collections.Generic;
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
            private readonly UserManager<User> userManager;
            public Handler(ApplicationDBContext context, IMapper mapper, UserManager<User> userManager)
            {
                this.userManager = userManager;
                this.mapper = mapper;
                this.context = context;
            }

            public async Task<TicketDto> Handle(Query request, CancellationToken cancellationToken)
            {
                Ticket ticket = await context.tickets
                                            .Include(ticket => ticket.author)
                                                .ThenInclude(user => user.avatar)
                                            .Include(ticket => ticket.product)
                                            .Include(ticket => ticket.status)
                                            .Include(ticket => ticket.attachment)
                                            .Include(ticket => ticket.developer)
                                                .ThenInclude(developer => developer.avatar)
                                            .Include(ticket => ticket.comments)
                                                .ThenInclude(comment => comment.author)
                                                    .ThenInclude(user => user.avatar)
                                            .Include(ticket => ticket.comments)
                                                .ThenInclude(comment => comment.attachment)
                                            .FirstOrDefaultAsync(ticket => ticket.post_id == request.post_id);
                if (ticket == null) throw new RestException(HttpStatusCode.NotFound, new { ticket = "Not found." });

                var authorRoles = await userManager.GetRolesAsync(ticket.author) as List<string>;

                var developerRoles = new List<string>();
                if(ticket.developer != null) developerRoles = await userManager.GetRolesAsync(ticket.developer) as List<string>;

                var ticketToReturn = mapper.Map<Ticket, TicketDto>(ticket);

                ticketToReturn.author.Roles = authorRoles;
                if(ticket.developer != null) ticketToReturn.developer.Roles = developerRoles;

                for(int i = 0; i < ticket.comments.Count; i++) {
                    var commentAuthorRoles = await userManager.GetRolesAsync(ticket.comments[i].author) as List<string>;
                    ticketToReturn.comments[i].author.Roles = commentAuthorRoles;
                }

                return ticketToReturn;
            }
        }
    }
}