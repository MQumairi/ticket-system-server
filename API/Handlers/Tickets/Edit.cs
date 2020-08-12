using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Infrastructure.Images;
using API.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace API.Handlers.Tickets
{
    public class Edit
    {
        public class Command : IRequest
        {
            public int post_id { get; set; }

            public string description { get; set; }

            public string title { get; set; }

            public int product_id { get; set; }

            public int status_id { get; set; }

            //If attachement
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
                Ticket ticket = await context.tickets.FindAsync(request.post_id);
                if (ticket == null) throw new RestException(HttpStatusCode.NotFound, new { ticket = "Not found." });

                ticket.description = request.description ?? ticket.description;
                ticket.title = request.title ?? ticket.title;

                if (request.product_id != 0)
                    ticket.product_id = request.product_id;

                if (request.status_id != 0)
                    ticket.status_id = request.status_id;

                //Handling attachments 

                //Find current attachment
                var current_attachement = await context.attachments.FindAsync(ticket.attachment_id);

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
                ticket.attachment = new_attachment;

                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }
        }
    }
}