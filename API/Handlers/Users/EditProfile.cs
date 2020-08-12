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
                user.first_name = request.first_name ?? user.first_name;
                user.surname = request.surname ?? user.surname;
                user.UserName = request.username ?? user.UserName;
                user.Email = request.email ?? user.Email;

                //Change password after confirmation
                if(request.new_password != null) {
                    await userManager.ChangePasswordAsync(user, request.current_password, request.new_password);
                }

                //Save
                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }

        }
    }
}