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
    public class Delete
    {
        public class Command : IRequest
        {
            //Properties
            public string role_id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly RoleManager<Role> roleManager;

            public Handler(RoleManager<Role> roleManager)
            {
                this.roleManager = roleManager;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //Handler logic
                var role = await roleManager.FindByIdAsync(request.role_id);

                if (role == null) throw new RestException(HttpStatusCode.NotFound, new { role = "Not found" });

                if (role.Name == "Admin") throw new RestException(HttpStatusCode.Forbidden, new { role = "Cannot delete the Admin role!" });
                if (role.Name == "Developer") throw new RestException(HttpStatusCode.Forbidden, new { role = "Cannot delete the Developer role!" });

                var deletion = await roleManager.DeleteAsync(role);

                var success = deletion.Succeeded;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }

        }
    }
}