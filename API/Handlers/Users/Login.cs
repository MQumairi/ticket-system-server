using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Infrastructure.Security;
using API.Models;
using API.Models.DTO;
using AutoMapper;
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
            private readonly IMapper mapper;
            public Handler(ApplicationDBContext context, UserManager<User> userManager, SignInManager<User> signInManager, JWTGenerator jWTGenerator, IMapper mapper)
            {
                this.mapper = mapper;
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

                var fetched_avatar = await context.profile_pics.FindAsync(user.avatar_id);

                var avatar_to_return = mapper.Map<Avatar, AvatarDto>(fetched_avatar);

                var userRoles = await userManager.GetRolesAsync(user) as List<string>;


                if (result.Succeeded)
                {
                    //TODO: Generate a JWT
                    CurrentUser currentUser = new CurrentUser
                    {
                        id = user.Id,
                        username = user.UserName,
                        email = request.email,
                        first_name = user.first_name,
                        surname = user.surname,
                        avatar = avatar_to_return,
                        token = await jWTGenerator.CreateToken(user),
                        roles = userRoles
                    };

                    return currentUser;
                }

                throw new Exception("Something went wrong!");
            }
        }
    }
}