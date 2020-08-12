using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Infrastructure.Images;
using API.Models;
using MediatR;

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