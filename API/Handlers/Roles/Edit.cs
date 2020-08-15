using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace API.Handlers.Roles
{
    public class Edit
    {
        public class Command : IRequest
        {
            //Properties
            public string role_id { get; set; }
            public string role_name { get; set; }
            public string role_color { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ApplicationDBContext context;
            private readonly RoleManager<Role> roleManager;
            public Handler(ApplicationDBContext context, RoleManager<Role> roleManager)
            {
                this.roleManager = roleManager;
                this.context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var role = await context.roles.FindAsync(request.role_id);

                if (role == null) throw new RestException(HttpStatusCode.NotFound, new { role = "Not Found" });

                role.color = request.role_color ?? role.color;

                if (request.role_name != null)
                {
                    if (role.Name == "Admin") throw new RestException(HttpStatusCode.Forbidden, new { role = "Cannot Edit the Admin role's name!" });
                    if (role.Name == "Developer") throw new RestException(HttpStatusCode.Forbidden, new { role = "Cannot Edit the Developer role's name!" });

                    await roleManager.SetRoleNameAsync(role, request.role_name);
                }

                //Handler logic
                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }

        }
    }
}