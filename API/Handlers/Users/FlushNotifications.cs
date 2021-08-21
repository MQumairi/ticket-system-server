using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace API.Handlers.Users
{
    public class FlushNotifications
    {
        public class Command : IRequest
        {
            public string user_id { get; set; }
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
                //Find the user
                var user = await context.Users.FindAsync(request.user_id);

                //Set their notifications to 0
                user.notifications = 0;

                //Handler logic
                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }

        }
    }
}