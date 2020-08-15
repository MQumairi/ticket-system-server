using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Tickets
{
    public class UnassignTicket
    {
        public class Command : IRequest
        {
            //From URL
            public int ticket_id { get; set; }

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
                var ticket = await context.tickets.Include(ticket => ticket.developer).FirstOrDefaultAsync(ticket => ticket.post_id == request.ticket_id);
                if (ticket == null) throw new RestException(HttpStatusCode.NotFound, new { ticket = "Not found" });

                ticket.developer = null;

                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;
                throw new Exception("Problem saving data");
            }

        }
    }
}