using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace API.Handlers.Users
{
    public class Delete
    {
        public class Command : IRequest
        {
            //Properties
            public string user_id { get; set; }
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
                var user = await userManager.FindByIdAsync(request.user_id);

                if(user == null) throw new RestException(HttpStatusCode.NotFound, new {user = "Not Found"});

                var userRoles = await userManager.GetRolesAsync(user) as List<string>;
                if(userRoles.Contains("Admin")) throw new RestException(HttpStatusCode.Forbidden, new {user = "Cannot delete admin accounts!"});
                if(userRoles.Contains("Developer")) throw new RestException(HttpStatusCode.Forbidden, new {user = "Cannot delete developer accounts!"});

                var deletion = await userManager.DeleteAsync(user);

                //Handler logic
                var success = deletion.Succeeded;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }

        }
    }
}