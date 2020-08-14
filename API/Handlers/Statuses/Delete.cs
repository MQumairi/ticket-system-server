using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Statuses
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int status_id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ApplicationDBContext context;
            public Handler(ApplicationDBContext context)
            {
                this.context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                Status status_to_delete = await context.status.FindAsync(request.status_id);

                if (status_to_delete == null) throw new RestException(HttpStatusCode.NotFound, new { status = "Not found" });

                if (status_to_delete.is_default) throw new RestException(HttpStatusCode.Forbidden, new { status = "Cannot delete the default status!" });

                //Find tickets of that status 
                List<Ticket> tickets_of_status = await context.tickets.Include(ticket => ticket.status)
                                                                        .Where(ticket => ticket.status_id == status_to_delete.status_id)
                                                                        .ToListAsync();
                                
                //Find the default status 
                Status default_status = await context.status.Where(status => status.is_default == true).FirstOrDefaultAsync();

                // throw new Exception("Testing exception " + default_status.status_text);

                //Set the status of those tickets to equal default status
                foreach (var ticket in tickets_of_status)
                {
                    ticket.status = default_status;
                }

                await context.SaveChangesAsync();

                //Delete the status you want to delete
                context.Remove(status_to_delete);

                //Handler logic
                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }

        }
    }
}