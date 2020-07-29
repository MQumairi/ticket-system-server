using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Statuses
{
    public class List
    {
        public class Query : IRequest<List<Status>> { }

        public class Handler : IRequestHandler<Query, List<Status>>
        {
            private readonly ApplicationDBContext context;
            public Handler(ApplicationDBContext context)
            {
                this.context = context;
            }

            public async Task<List<Status>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<Status> statuses = await context.status.ToListAsync();

                return statuses;
            }
        }
    }
}