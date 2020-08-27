using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Security;
using API.Models;
using API.Models.DTO;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace API.Handlers.Users
{
    public class CurrentUserHandler
    {
        public class Query : IRequest<CurrentUser> { }

        public class Handler : IRequestHandler<Query, CurrentUser>
        {
            private readonly ApplicationDBContext context;
            private readonly UserManager<User> userManager;
            private readonly JWTGenerator jWTGenerator;
            private readonly UserAccessor userAccessor;
            private readonly IMapper mapper;
            public Handler(ApplicationDBContext context, UserManager<User> userManager, JWTGenerator jWTGenerator, UserAccessor userAccessor, IMapper mapper)
            {
                this.mapper = mapper;
                this.userAccessor = userAccessor;
                this.jWTGenerator = jWTGenerator;
                this.userManager = userManager;
                this.context = context;
            }

            public async Task<CurrentUser> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByEmailAsync(userAccessor.getCurrentUsername());

                var fetched_avatar = await context.profile_pics.FindAsync(user.avatar_id);
                var avatar_to_return = mapper.Map<Avatar, AvatarDto>(fetched_avatar);

                var userRoles = await userManager.GetRolesAsync(user) as List<string>;

                return new CurrentUser
                {
                    id = user.Id,
                    username = user.UserName,
                    avatar = avatar_to_return,
                    email = user.Email,
                    first_name = user.first_name,
                    surname = user.surname,
                    roles = userRoles
                };
            }
        }
    }
}