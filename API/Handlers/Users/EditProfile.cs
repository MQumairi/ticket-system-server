using System;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Security;
using API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace API.Handlers.Users
{
    public class EditPorfile
    {
        public class Command : IRequest
        {
            //Editable props
            public string first_name { get; set; }
            public string surname { get; set; }
            public string username { get; set; }
            public string email { get; set; }
            public string current_password { get; set; }
            public string new_password { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ApplicationDBContext context;
            private readonly UserAccessor userAccessor;
            private readonly UserManager<User> userManager;
            public Handler(ApplicationDBContext context, UserAccessor userAccessor, UserManager<User> userManager)
            {
                this.userManager = userManager;
                this.userAccessor = userAccessor;
                this.context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //Handler logic

                //Get current user 
                var user = await userManager.FindByEmailAsync(userAccessor.getCurrentUsername());

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
                    var username_change = await userManager.SetUserNameAsync(user, request.username);
                    userManager_changes = username_change.Succeeded;
                }

                if (request.email != null)
                {
                    var email_change = await userManager.SetEmailAsync(user, request.email);
                    userManager_changes = email_change.Succeeded;
                }

                //Change password after confirmation
                if (request.new_password != null)
                {
                    var password_change = await userManager.ChangePasswordAsync(user, request.current_password, request.new_password);
                    userManager_changes = password_change.Succeeded;
                }

                //Save
                await context.SaveChangesAsync();
                if(userManager_changes) return Unit.Value;

                throw new Exception("Problem saving data, success is " + success + ", while umc is " + userManager_changes);
            }

        }
    }
}