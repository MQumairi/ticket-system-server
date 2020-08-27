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
            public Handler(ApplicationDBContext context, PhotoAccessor photoAccessor, UserManager<User> userManager, UserAccessor userAccessor)
            {
                this.userAccessor = userAccessor;
                this.userManager = userManager;
                this.photoAccessor = photoAccessor;
                this.context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //Get the ticket
                Ticket ticket = await context.tickets.Include(ticket => ticket.developer).FirstOrDefaultAsync(ticket => ticket.post_id == request.post_id);
                if (ticket == null) throw new RestException(HttpStatusCode.NotFound, new { ticket = "Not found." });

                //Set the status and is_archvied
                ticket.status_id = request.status_id;

                ticket.is_archived = request.is_archived;

                if (request.developer_id != null)
                {
                    //Find the user requested to be the assigned dev, and their roles
                    User requested_dev = await userManager.FindByIdAsync(request.developer_id);

                    if (requested_dev == null) throw new RestException(HttpStatusCode.NotFound, new { dev = "Not found" });

                    var requested_dev_roles = await userManager.GetRolesAsync(requested_dev) as List<string>;

                    //If the requested_dev is neither a dev nor an admin throw exception
                    if (!requested_dev_roles.Contains("Developer") && requested_dev_roles.Contains("Admin")) throw new RestException(HttpStatusCode.Forbidden, new { user = "This user cannot be assigned the ticket!" });

                    //Else, assign the ticket's dev to equal that dev
                    ticket.developer = requested_dev;
                }

                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }
        }
    }
}