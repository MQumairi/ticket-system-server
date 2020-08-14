using System;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace API.Handlers.Roles
{
    public class Add
    {
        public class Command : IRequest
        {
            //Properties
            public string role_name { get; set; }
            public string role_color { get; set; }
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
                var role = new Role
                {
                    Name = request.role_name,
                    color = request.role_color
                };

                var roleAddition = await roleManager.CreateAsync(role);

                var success = roleAddition.Succeeded;

                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }

        }
    }
}