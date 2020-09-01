using System;
using System.Collections.Generic;
using System.Linq;
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
                //Get current user
                var current_user = await context.Users.FirstOrDefaultAsync(user => user.Email == userAccessor.getCurrentUsername());

                var current_user_role_list = await userManager.GetRolesAsync(current_user);

                Role current_user_role = null;

                if(current_user_role_list.Count > 0) {
                   var current_user_role_string = current_user_role_list[0];
                   current_user_role = await roleManager.FindByNameAsync(current_user_role_string);
                }

                Ticket ticket = await context.tickets.Include(ticket => ticket.attachment).FirstOrDefaultAsync(ticket => ticket.post_id == request.post_id);
                if (ticket == null) throw new RestException(HttpStatusCode.NotFound, new { ticket = "Not found." });

                if (!(current_user.Id == ticket.author_id || (current_user_role != null && current_user_role.can_moderate))) throw new RestException(HttpStatusCode.Forbidden, new { user = "You don't have the permission to do this, since your id is " + current_user.Id + ", while the post's id is " + ticket.author_id });

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