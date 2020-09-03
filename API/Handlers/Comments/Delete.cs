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

namespace API.Handlers.Comments
{
    public class Delete
    {
        public class Command : IRequest
        {
            //Properties
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
                var current_user = await userManager.FindByEmailAsync(userAccessor.getCurrentUsername());

                var current_user_roles_list = await userManager.GetRolesAsync(current_user);

                Role current_user_role = null;

                if (current_user_roles_list.Count > 0)
                {
                    current_user_role = await roleManager.FindByNameAsync(current_user_roles_list[0]);
                }

                //Find the comment
                Comment comment = await context.comments.FindAsync(request.post_id);
                if (comment == null) throw new RestException(HttpStatusCode.NotFound, new { comment = "Not found" });

                if (!(current_user.Id == comment.author_id || (current_user_role != null && current_user_role.can_moderate))) throw new RestException(HttpStatusCode.Forbidden, new { user = "You don't have the permission to do this" });

                if (comment.attachment_id != null)
                {
                    var attachment = await context.attachments.FindAsync(comment.attachment_id);
                    photoAccessor.DeletePhoto(attachment.Id);
                    context.attachments.Remove(attachment);
                }
                //Delete it
                context.comments.Remove(comment);

                //Save
                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }

        }
    }
}