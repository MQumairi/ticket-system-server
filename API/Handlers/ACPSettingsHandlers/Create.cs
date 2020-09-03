using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace API.Handlers.ACPSettingsHandlers
{
    public class Create
    {
        public class Command : IRequest
        {
            //Properties
            public string founder_id { get; set; }

            public bool registration_locked { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ApplicationDBContext context;
            private readonly UserManager<User> userManager;
            public Handler(ApplicationDBContext context, UserManager<User> userManager)
            {
                this.userManager = userManager;
                this.context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //If acp settings already has a row, throw an exception. You can only add settings if settings don't already exist
                if (context.acp_settings.Any()) throw new RestException(HttpStatusCode.Forbidden, new { error = "ACP settings already implemented" });


                var requested_founder = await context.Users.FindAsync(request.founder_id);

                var requested_founder_roles = await userManager.GetRolesAsync(requested_founder);

                if (!requested_founder_roles.Contains("Admin")) throw new RestException(HttpStatusCode.Forbidden, new { error = "That user is not an Admin!" });

                if (requested_founder == null) throw new RestException(HttpStatusCode.NotFound, new { user = "This user doesn't exist" });

                ACPSettings acpSettings = new ACPSettings
                {
                    founder_id = requested_founder.Id,
                    registration_locked = request.registration_locked
                };

                context.acp_settings.Add(acpSettings);

                //Handler logic
                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }

        }
    }
}