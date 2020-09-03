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
            private readonly RoleManager<Role> roleManager;
            public Handler(ApplicationDBContext context, IMapper mapper, UserManager<User> userManager, RoleManager<Role> roleManager)
            {
                this.roleManager = roleManager;
                this.userManager = userManager;
                this.mapper = mapper;
                this.context = context;
            }

            public async Task<TicketDto> Handle(Query request, CancellationToken cancellationToken)
            {
                //Get the ticket, plus all relevant data
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

                //If the ticket doesn't exist, throw 404
                if (ticket == null) throw new RestException(HttpStatusCode.NotFound, new { ticket = "Not found." });

                //Map ticket
                var ticketToReturn = mapper.Map<Ticket, TicketDto>(ticket);

                //Get the role of the author, assidn it to the maped ticket
                var authorRoleList = await userManager.GetRolesAsync(ticket.author);

                if (authorRoleList.Count > 0)
                {
                    var authorRoleString = authorRoleList[0];
                    var authorRole = await roleManager.FindByNameAsync(authorRoleString);
                    var authorRoleDto = mapper.Map<Role, RoleDto>(authorRole);
                    ticketToReturn.author.role = authorRoleDto;
                }

                for (int i = 0; i < ticket.comments.Count; i++)
                {
                    var commentAuthorRoleList = await userManager.GetRolesAsync(ticket.comments[i].author);

                    if (commentAuthorRoleList.Count > 0)
                    {
                        var commentAuthorRoleString = commentAuthorRoleList[0];
                        var commentAuthorRole = await roleManager.FindByNameAsync(commentAuthorRoleString);
                        var commentAuthorRoleDto = mapper.Map<Role, RoleDto>(commentAuthorRole);
                        ticketToReturn.comments[i].author.role = commentAuthorRoleDto;
                    }
                }

                return ticketToReturn;
            }
        }
    }
}