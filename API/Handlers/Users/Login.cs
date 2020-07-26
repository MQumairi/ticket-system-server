using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Infrastructure.Security;
using API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace API.Handlers.Users
{
    public class Login
    {
        public class Query : IRequest<CurrentUser>
        {

            public string email { get; set; }
            public string password { get; set; }
        }

        public class Handler : IRequestHandler<Query, CurrentUser>
        {
            private readonly UserManager<User> userManager;
            private readonly SignInManager<User> signInManager;
            private readonly JWTGenerator jWTGenerator;
            public Handler(UserManager<User> userManager, SignInManager<User> signInManager, JWTGenerator jWTGenerator)
            {
                this.jWTGenerator = jWTGenerator;
                this.userManager = userManager;
                this.signInManager = signInManager;
            }


            public async Task<CurrentUser> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByEmailAsync(request.email);
                if (user == null) throw new RestException(HttpStatusCode.Unauthorized);

                var result = await signInManager.CheckPasswordSignInAsync(user, request.password, false);

                if (result.Succeeded)
                {
                    //TODO: Generate a JWT
                    CurrentUser currentUser = new CurrentUser
                    {
                        email = request.email,
                        token = jWTGenerator.CreateToken(user)
                    };

                    return currentUser;
                }

                throw new Exception("Something went wrong!");
            }
        }
    }
}