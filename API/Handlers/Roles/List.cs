using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Roles
{
    public class List
    {
        public class Query : IRequest<List<Role>> { }

        public class Handler : IRequestHandler<Query, List<Role>>
        {
            private readonly ApplicationDBContext context;
            public Handler(ApplicationDBContext context)
            {
                this.context = context;
            }

            public async Task<List<Role>> Handle(Query request, CancellationToken cancellationToken)
            {
                //Handler logic goes here
                var roles = await context.roles.ToListAsync();

                return roles;
            }
        }
    }
}