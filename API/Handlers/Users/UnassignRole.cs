using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Users
{
    public class UnassignRole
    {
        public class Command : IRequest
        {
            //Properties
            public string user_id { get; set; }
            public string role_name { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ApplicationDBContext context;
            private readonly UserManager<User> userManager;
            public Handler(ApplicationDBContext context, UserManager<User> userManager)
            {
                this.userManager = userManager;
                this.context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByIdAsync(request.user_id);

                if (user == null) throw new RestException(HttpStatusCode.NotFound, new { user = "Not found" });

                //If the user is a developer, unassign them from all tickets
                var usersRole = await userManager.GetRolesAsync(user) as List<string>;

                if(usersRole.Contains("Developer")) {
                    var assignedTickets = await context.tickets.Where((ticket) => ticket.developer_id == user.Id).ToListAsync();
                    
                    foreach (var ticket in assignedTickets)
                    {
                        ticket.developer_id = null;
                    }

                    var contextSuccess = await context.SaveChangesAsync() > 0;

                    if(!contextSuccess) throw new RestException(HttpStatusCode.BadRequest, new {tickets = "Error unassigning tickets from Developer"});
                }

                //Perform the unassignment
                var unassignment = await userManager.RemoveFromRoleAsync(user, request.role_name);

                //Handler logic
                var success = unassignment.Succeeded;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }

        }
    }
}