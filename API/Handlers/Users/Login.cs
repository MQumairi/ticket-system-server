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
            private readonly ApplicationDBContext context;
            public Handler(ApplicationDBContext context, UserManager<User> userManager, SignInManager<User> signInManager, JWTGenerator jWTGenerator)
            {
                this.context = context;
                this.jWTGenerator = jWTGenerator;
                this.userManager = userManager;
                this.signInManager = signInManager;
            }


            public async Task<CurrentUser> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByEmailAsync(request.email);
                if (user == null) throw new RestException(HttpStatusCode.Unauthorized);

                var result = await signInManager.CheckPasswordSignInAsync(user, request.password, false);

                var fetched_avatar = await context.photos.FindAsync(user.avatar_id);

                if (result.Succeeded)
                {
                    //TODO: Generate a JWT
                    CurrentUser currentUser = new CurrentUser
                    {
                        user_id = user.Id,
                        username = user.UserName,
                        email = request.email,
                        first_name = user.first_name,
                        surname = user.surname,
                        avatar_url = fetched_avatar.url,
                        token = jWTGenerator.CreateToken(user)
                    };

                    return currentUser;
                }

                throw new Exception("Something went wrong!");
            }
        }
    }
}