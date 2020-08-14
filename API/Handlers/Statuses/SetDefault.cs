using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using MediatR;

namespace API.Handlers.Statuses
{
    public class SetDefault
    {
        public class Command : IRequest
        {
            //Properties
            public int status_id { get; set; }
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
                //Handler logic
                var current_default = context.status.Where(status => status.is_default == true).FirstOrDefault();

                if(current_default != null) current_default.is_default = false;

                var status_to_make_default = await context.status.FindAsync(request.status_id);

                status_to_make_default.is_default = true;

                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }

        }
    }
}