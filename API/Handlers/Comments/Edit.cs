using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Infrastructure.Images;
using API.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

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
            public Handler(ApplicationDBContext context, PhotoAccessor photoAccessor)
            {
                this.photoAccessor = photoAccessor;
                this.context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //Find the comment
                Comment comment = await context.comments.FindAsync(request.post_id);

                if (comment == null)
                    throw new RestException(HttpStatusCode.NotFound, new { comment = "Not found" });

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