using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Infrastructure.Security;
using API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.ACPSettingsHandlers
{
    public class Edit
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
            private readonly UserAccessor userAccessor;
            public Handler(ApplicationDBContext context, UserManager<User> userManager, UserAccessor userAccessor)
            {
                this.userAccessor = userAccessor;
                this.userManager = userManager;
                this.context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //Find the settings
                var acpSettingsList = await context.acp_settings.ToListAsync();

                if (acpSettingsList.Count == 0) throw new RestException(HttpStatusCode.NotFound, new { settings = "Not found" });

                var acpSettings = acpSettingsList[0];

                //Check that current user is the founder
                var current_user = await context.Users.SingleOrDefaultAsync(user => user.Email == userAccessor.getCurrentUsername());
                if(current_user.Id != acpSettings.founder_id) throw new RestException(HttpStatusCode.Forbidden, new { settings = "Only the founder can edit these settings" });

                //Find the user and check that they are admin
                var user = await context.Users.FindAsync(request.founder_id);

                if (user == null) throw new RestException(HttpStatusCode.NotFound, new { user = "Not found" });

                var user_roles = await userManager.GetRolesAsync(user);

                if (!user_roles.Contains("Admin")) throw new RestException(HttpStatusCode.Forbidden, new { error = "That user is not an Admin!" });

                //Perform the edits
                acpSettings.founder_id = request.founder_id ?? acpSettings.founder_id;

                acpSettings.registration_locked = request.registration_locked;

                //Handler logic
                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }

        }
    }
}