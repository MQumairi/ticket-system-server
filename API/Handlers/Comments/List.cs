using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Comments
{
    public class List
    {
        public class Query : IRequest<List<Comment>> { }

        public class Handler : IRequestHandler<Query, List<Comment>>
        {
            private readonly ApplicationDBContext context;
            public Handler(ApplicationDBContext context)
            {
                this.context = context;
            }

            public async Task<List<Comment>> Handle(Query request, CancellationToken cancellationToken)
            {
                var comments = await context.comments.ToListAsync();
                return comments;
            }
        }
    }
}