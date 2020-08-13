using System;
using System.Threading;
using System.Threading.Tasks;
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
            public string new_password { get; set; }
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
                //Handler logic
                //Get current user 
                var user = await userManager.FindByIdAsync(request.user_id);

                //Change fields
                user.first_name = request.first_name ?? user.first_name;
                user.surname = request.surname ?? user.surname;

                if (request.username != null)
                {
                    await userManager.SetUserNameAsync(user, request.username);
                }

                if (request.email != null)
                {
                    await userManager.SetEmailAsync(user, request.email);
                }

                //Change password after confirmation
                if (request.new_password != null)
                {
                    var pass_token = await userManager.GeneratePasswordResetTokenAsync(user);
                    await userManager.ResetPasswordAsync(user, pass_token, request.new_password);
                }

                //Save
                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }

        }
    }
}