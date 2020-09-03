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
    public class Edit
    {
        public class Command : IRequest
        {
            //Properties
            public string user_id { get; set; }
            public string first_name { get; set; }
            public string surname { get; set; }
            public string username { get; set; }
            public string email { get; set; }
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
                //Get user 
                var user = await userManager.FindByIdAsync(request.user_id);

                //If the user does not exist, throw an exception
                if(user == null) throw new RestException(HttpStatusCode.NotFound, new {user = "Not found"});

                //If the user is an admin, throw an exception
                var userRoles = await userManager.GetRolesAsync(user) as List<string>;
                if(userRoles.Contains("Admin")) throw new RestException(HttpStatusCode.Forbidden, new {user = "Cannot edit admin accounts!"});

                //Change fields
                if (request.first_name != null || request.surname != null)
                {
                    user.first_name = request.first_name ?? user.first_name;
                    user.surname = request.surname ?? user.surname;
                }

                bool userManager_changes = true;
                bool success = true;

                if (request.username != null)
                {
                    var change_username = await userManager.SetUserNameAsync(user, request.username);
                    userManager_changes = change_username.Succeeded;
                }

                if (request.email != null)
                {
                    var change_email = await userManager.SetEmailAsync(user, request.email);
                    userManager_changes = change_email.Succeeded;
                }

                //Save
                await context.SaveChangesAsync();
                if (userManager_changes) return Unit.Value;

                throw new Exception("Problem saving data, success is " + success + ", while umc is " + userManager_changes);
            }
        }
    }
}