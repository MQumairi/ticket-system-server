using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Infrastructure.Images;
using API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Tickets
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int post_id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ApplicationDBContext context;
            private readonly PhotoAccessor photoAccessor;
            public Handler(ApplicationDBContext context, PhotoAccessor photoAccessor)
            {
                this.photoAccessor = photoAccessor;
                this.context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                Ticket ticket = await context.tickets.FirstOrDefaultAsync(ticket => ticket.post_id == request.post_id);
                if (ticket == null) throw new RestException(HttpStatusCode.NotFound, new { ticket = "Not found." });

                if (ticket.attachment_id != null)
                {
                    var attachment = await context.attachments.FindAsync(ticket.attachment_id);
                    context.photos.Remove(attachment);
                    photoAccessor.DeletePhoto(attachment.Id);
                }

                context.tickets.Remove(ticket);

                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }
        }
    }
}