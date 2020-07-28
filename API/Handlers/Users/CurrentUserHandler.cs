using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Security;
using API.Models;
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
            public Handler(ApplicationDBContext context, UserManager<User> userManager, JWTGenerator jWTGenerator, UserAccessor userAccessor)
            {
                this.userAccessor = userAccessor;
                this.jWTGenerator = jWTGenerator;
                this.userManager = userManager;
                this.context = context;
            }

            public async Task<CurrentUser> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByEmailAsync(userAccessor.getCurrentUsername());

                return new CurrentUser
                {
                    email = user.Email,
                    token = jWTGenerator.CreateToken(user)
                };
            }
        }
    }
}