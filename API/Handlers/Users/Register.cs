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

namespace API.Handlers.Users
{
    public class Register
    {
        public class Command : IRequest<CurrentUser>
        {
            public string first_name { get; set; }
            public string surname { get; set; }
            public string username { get; set; }
            public string email { get; set; }
            public string password { get; set; }
        }

        public class Handler : IRequestHandler<Command, CurrentUser>
        {
            private readonly ApplicationDBContext context;
            private readonly UserManager<User> userManager;
            private readonly JWTGenerator jWTGenerator;
            public Handler(ApplicationDBContext context, UserManager<User> userManager, JWTGenerator jWTGenerator)
            {
                this.context = context;
                this.jWTGenerator = jWTGenerator;
                this.userManager = userManager;
            }

            public async Task<CurrentUser> Handle(Command request, CancellationToken cancellationToken)
            {
                if ((await context.Users.AnyAsync(x => x.Email == request.email)))
                    throw new RestException(HttpStatusCode.BadRequest, new { Email = "Email already exists. Try registering with another one." });

                if ((await context.Users.AnyAsync(x => x.UserName == request.username)))
                    throw new RestException(HttpStatusCode.BadRequest, new { Username = "Username already exists. Try registering with another one" });
                
                if((await context.acp_settings.ToListAsync())[0].registration_locked) {
                    throw new RestException(HttpStatusCode.BadRequest, new { Locked = "Registration is currently locked to protect the site from spammers. Browse as guest." });
                }

                User userToRegister = new User
                {
                    UserName = request.username,
                    Email = request.email,
                    first_name = request.first_name,
                    surname = request.surname
                };

                var registerUser = await userManager.CreateAsync(userToRegister, request.password);

                if (registerUser.Succeeded)
                {
                    // return Unit.Value;
                    return new CurrentUser
                    {
                        id = userToRegister.Id,
                        username = userToRegister.UserName,
                        email = userToRegister.Email,
                        first_name = userToRegister.first_name,
                        surname = userToRegister.surname,
                        avatar = null,
                        token = await jWTGenerator.CreateToken(userToRegister)
                    };
                }


                throw new Exception("Something went wrong!");
            }
        }
    }
}