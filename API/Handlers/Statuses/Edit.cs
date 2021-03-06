using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using MediatR;

namespace API.Handlers.Statuses
{
    public class Edit
    {
        public class Command : IRequest
        {
            //Properties
            public int status_id { get; set; }
            public string status_text { get; set; }
            public string status_color { get; set; }
            public bool is_default { get; set; }
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
                Status status = await context.status.FindAsync(request.status_id);

                if (status == null) throw new RestException(HttpStatusCode.NotFound, new { status = "Not found" });

                status.status_text = request.status_text ?? status.status_text;
                status.status_color = request.status_color ?? status.status_color;

                //If the chosen status is already default, don't allow it to change to regular
                if(status.is_default && (!request.is_default)) throw new RestException(HttpStatusCode.BadRequest, new {status = "Status is already default status!"});

                //If setting to default
                if (request.is_default)
                {
                    var current_default = context.status.Where(status => status.is_default == true).FirstOrDefault();
                    if (current_default != null) current_default.is_default = false;
                    status.is_default = request.is_default;
                }

                //Handler logic
                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }

        }
    }
}