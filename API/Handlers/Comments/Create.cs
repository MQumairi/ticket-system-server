using System;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Images;
using API.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace API.Handlers.Comments
{
    public class Create
    {
        public class Command : IRequest
        {
            //Properties
            public string date_time { get; set; }
            public string description { get; set; }
            public string author_id { get; set; }
            public int parent_post_id { get; set; }

            //If there is an attachment
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
                Comment comment = new Comment
                {
                    date_time = DateTime.Parse(request.date_time, System.Globalization.CultureInfo.InvariantCulture),
                    description = request.description,
                    author_id = request.author_id,
                    parent_post_id = request.parent_post_id
                };

                if (request.image != null)
                {
                    var uploadResults = photoAccessor.AddPhoto(request.image);

                    var new_attachment = new Attachment {
                        Id = uploadResults.PublicId,
                        url = uploadResults.Url
                    };

                    comment.attachment = new_attachment;
                }

                context.comments.Add(comment);
                //Handler logic
                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }

        }
    }
}