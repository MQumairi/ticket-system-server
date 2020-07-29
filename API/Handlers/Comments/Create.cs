using System;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using MediatR;

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
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ApplicationDBContext context;
            public Handler(ApplicationDBContext context)
            {
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

                context.comments.Add(comment);
                //Handler logic
                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }

        }
    }
}