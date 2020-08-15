using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace API.Handlers.Tickets
{
    public class AssignTicket
    {
        public class Command : IRequest
        {
            //From URL
            public int ticket_id { get; set; }

            //Properties
            public string dev_id { get; set; }
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
                //Find the ticket (check for null)
                var ticket = await context.tickets.FindAsync(request.ticket_id);
                if (ticket == null) throw new RestException(HttpStatusCode.NotFound, new { ticket = "Not found" });

                //Find the user (check for null, then check that he's a dev)
                var user = await context.Users.FindAsync(request.dev_id);
                if (user == null) throw new RestException(HttpStatusCode.NotFound, new { user = "Not found" });

                List<string> user_roles = await userManager.GetRolesAsync(user) as List<string>;
                if (!user_roles.Contains("Developer")) throw new RestException(HttpStatusCode.BadRequest, new { user = "This user is not a developer!" });

                //Set ticket's dev to found dev
                ticket.developer = user;

                //Save and exit
                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }
        }
    }
}