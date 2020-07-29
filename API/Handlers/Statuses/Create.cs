using System;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using MediatR;

namespace API.Handlers.Statuses
{
    public class Create
    {
        public class Command : IRequest
        {
            //Properties
            public string status_text { get; set; }
            public string status_color { get; set; }
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
                Status status = new Status
                {
                    status_text = request.status_text,
                    status_color = request.status_color
                };

                context.status.Add(status);

                //Handler logic
                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }
        }
    }
}