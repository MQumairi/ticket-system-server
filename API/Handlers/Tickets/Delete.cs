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
using Microsoft.AspNetCore.Identity;
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
                //Get current user
                var current_user = await userManager.FindByEmailAsync(userAccessor.getCurrentUsername());
                var current_user_roles = await userManager.GetRolesAsync(current_user) as List<string>;

                Ticket ticket = await context.tickets.FirstOrDefaultAsync(ticket => ticket.post_id == request.post_id);
                if (ticket == null) throw new RestException(HttpStatusCode.NotFound, new { ticket = "Not found." });

                if(!(current_user.Id == ticket.author_id || current_user_roles.Contains("Admin"))) throw new RestException(HttpStatusCode.Forbidden, new {user = "You don't have the permission to do this, since your id is " + current_user.Id + ", while the post's id is " + ticket.author_id});

                if (ticket.attachment_id != null)
                {
                    var attachment = await context.attachments.FindAsync(ticket.attachment_id);
                    photoAccessor.DeletePhoto(attachment.Id);
                    context.attachments.Remove(attachment);
                }

                context.tickets.Remove(ticket);

                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }
        }
    }
}