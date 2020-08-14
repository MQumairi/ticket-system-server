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

namespace API.Handlers.Comments
{
    public class Edit
    {
        public class Command : IRequest
        {
            public int post_id { get; set; }
            public string description { get; set; }

            //If attachmnet is included
            public IFormFile image { get; set; }
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

                //Find the comment
                Comment comment = await context.comments.FindAsync(request.post_id);
                if (comment == null) throw new RestException(HttpStatusCode.NotFound, new { comment = "Not found" });

                if(!(current_user.Id == comment.author_id || current_user_roles.Contains("Admin"))) throw new RestException(HttpStatusCode.Forbidden, new {user = "You don't have the permission to do this"});

                //Edit it
                comment.description = request.description ?? comment.description;

                //Handling attachments 

                //Find current attachment
                var current_attachement = await context.attachments.FindAsync(comment.attachment_id);

                //Initialize new attachemnt
                Attachment new_attachment = null;

                //If the request contains an attachment, upload it, and save to new_attachemnt
                if (request.image != null)
                {
                    var uploadAttachmentResult = photoAccessor.AddPhoto(request.image);

                    new_attachment = new Attachment
                    {
                        Id = uploadAttachmentResult.PublicId,
                        url = uploadAttachmentResult.Url
                    };
                }

                //If current_attachment is not null, delete it from the cloud and delete from DB
                if (current_attachement != null)
                {
                    photoAccessor.DeletePhoto(current_attachement.Id);
                    context.attachments.Remove(current_attachement);
                }

                //Set the ticket.attachment to new_attachment (this may null)
                comment.attachment = new_attachment;

                //Save
                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }
        }
    }
}