using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace API.Handlers.Developers
{
    public class List
    {
        public class Query : IRequest<List<User>> { }

        public class Handler : IRequestHandler<Query, List<User>>
        {
            private readonly ApplicationDBContext context;
            private readonly UserManager<User> userManager;
            public Handler(ApplicationDBContext context, UserManager<User> userManager)
            {
                this.userManager = userManager;
                this.context = context;
            }

            public async Task<List<User>> Handle(Query request, CancellationToken cancellationToken)
            {
                //Handler logic goes here
                var developers = await userManager.GetUsersInRoleAsync("Developer") as List<User>;

                return developers;
            }
        }
    }
}