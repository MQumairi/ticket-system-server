using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Infrastructure.Images;
using API.Infrastructure.Security;
using API.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Tickets
{
    public class Manage
    {
        public class Command : IRequest
        {
            public int post_id { get; set; }

            public int status_id { get; set; }

            public string developer_id { get; set; }

            public bool is_archived { get; set; }

        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ApplicationDBContext context;
            private readonly PhotoAccessor photoAccessor;
            private readonly UserManager<User> userManager;
            private readonly UserAccessor userAccessor;
            private readonly RoleManager<Role> roleManager;
            public Handler(ApplicationDBContext context, PhotoAccessor photoAccessor, UserManager<User> userManager, RoleManager<Role> roleManager, UserAccessor userAccessor)
            {
                this.roleManager = roleManager;
                this.userAccessor = userAccessor;
                this.userManager = userManager;
                this.photoAccessor = photoAccessor;
                this.context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var current_user = await userManager.FindByEmailAsync(userAccessor.getCurrentUsername());

                var current_user_role_list = await userManager.GetRolesAsync(current_user);

                Role current_user_role = null;

                if (current_user_role_list.Count > 0)
                {
                    var current_user_role_string = current_user_role_list[0];
                    current_user_role = await roleManager.FindByNameAsync(current_user_role_string);
                }

                if (current_user_role == null || !current_user_role.can_manage) throw new RestException(HttpStatusCode.Forbidden, new { error = "You don't have the permission to do this" });

                //Get the ticket
                Ticket ticket = await context.tickets.Include(ticket => ticket.developer).FirstOrDefaultAsync(ticket => ticket.post_id == request.post_id);
                if (ticket == null) throw new RestException(HttpStatusCode.NotFound, new { ticket = "Not found." });

                //Set the status and is_archvied
                ticket.status_id = request.status_id;

                //To prevent double decrement of notifcations
                bool alreadyDecremented = false;

                //If the ticket has a developer, and you're archivig it... decrement their notifications
                if (ticket.developer != null && request.is_archived && (request.is_archived != ticket.is_archived) && ticket.developer.notifications > 0)
                {
                    ticket.developer.notifications--;
                    alreadyDecremented = true;
                }

                if (ticket.developer != null && !request.is_archived && (request.is_archived != ticket.is_archived))
                {
                    ticket.developer.notifications++;
                }

                //If requesting to archive a ticket
                ticket.is_archived = request.is_archived;


                //If requesting a dev assignment to a ticket
                if (request.developer_id != null)
                {
                    //Find the user requested to be the assigned dev, and their roles
                    User requested_dev = await userManager.FindByIdAsync(request.developer_id);

                    if (requested_dev == null) throw new RestException(HttpStatusCode.NotFound, new { dev = "Not found" });

                    var requested_dev_roles_list = await userManager.GetRolesAsync(requested_dev);

                    Role requested_dev_role = null;

                    if (requested_dev_roles_list.Count > 0)
                    {
                        var requested_dev_role_string = requested_dev_roles_list[0];
                        requested_dev_role = await roleManager.FindByNameAsync(requested_dev_role_string);
                    }

                    //If the requested_dev has no role, or has a role, but that role doesn't have managing permissions, throw exception
                    if (requested_dev_role == null || !requested_dev_role.can_manage) throw new RestException(HttpStatusCode.Forbidden, new { user = "This user cannot be assigned the ticket!" });

                    //If the ticket already has a developer, decrement their notifcations 
                    if (ticket.developer != null && ticket.developer.Id != request.developer_id && !alreadyDecremented && ticket.developer.notifications > 0)
                    {
                        Console.WriteLine("Notifications before remove from" + ticket.developer.UserName + ": " + ticket.developer.notifications);
                        ticket.developer.notifications--;
                        Console.WriteLine("Notifications after remove from" + ticket.developer.UserName + ": " + ticket.developer.notifications);

                    }

                    //Finally, assign the ticket's dev to equal the requested dev
                    ticket.developer = requested_dev;

                    //Then increment the developer's notifications by 1
                    if (requested_dev.Id != ticket.developer_id) requested_dev.notifications++;
                }

                //Requesting dev unassignment from ticket
                if (request.developer_id == null && ticket.developer != null)
                {
                    if (ticket.developer.notifications > 0 && !alreadyDecremented)
                    {
                        ticket.developer.notifications--;
                    }

                    ticket.developer = null;
                }

                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }
        }
    }
}