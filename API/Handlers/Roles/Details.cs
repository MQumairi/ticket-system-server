using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using MediatR;

namespace API.Handlers.Roles
{
    public class Details
    {
        public class Query : IRequest<Role>
        {
            public string role_id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Role>
        {
            private readonly ApplicationDBContext context;
            public Handler(ApplicationDBContext context)
            {
                this.context = context;
            }

            public async Task<Role> Handle(Query request, CancellationToken cancellationToken)
            {
                //Handler logic goes here
                var role = await context.roles.FindAsync(request.role_id);

                if (role == null) throw new RestException(HttpStatusCode.NotFound, new { role = "Not found" });

                return role;
            }
        }
    }
}