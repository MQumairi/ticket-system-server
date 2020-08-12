using System;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Images;
using API.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace API.Handlers.Tickets
{
    public class Create
    {
        public class Command : IRequest
        {
            public string date_time { get; set; }

            public string description { get; set; }

            public string author_id { get; set; }

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

                Ticket ticket = new Ticket
                {
                    date_time = DateTime.Parse(request.date_time, System.Globalization.CultureInfo.InvariantCulture),
                    description = request.description,
                    author_id = request.author_id,
                    title = request.title,
                    product_id = request.product_id,
                    status_id = request.status_id
                };

                if (request.image != null)
                {
                    //Handler logic
                    var uploadAttachmentResult = photoAccessor.AddPhoto(request.image);

                    Attachment attachment = new Attachment
                    {
                        Id = uploadAttachmentResult.PublicId,
                        url = uploadAttachmentResult.Url
                    };

                    ticket.attachment = attachment;
                }

                context.tickets.Add(ticket);

                bool success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }
        }
    }
}