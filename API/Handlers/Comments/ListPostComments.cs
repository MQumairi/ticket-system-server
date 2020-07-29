using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Comments
{
    public class ListPostComments
    {
        public class Query : IRequest<List<Comment>>
        {
            public int parent_id { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<Comment>>
        {
            private readonly ApplicationDBContext context;
            public Handler(ApplicationDBContext context)
            {
                this.context = context;
            }

            public async Task<List<Comment>> Handle(Query request, CancellationToken cancellationToken)
            {
                //Find the comments whose parent id is equal to that in the request
                var comments = await context.comments.Where(c => c.parent_post_id == request.parent_id).ToListAsync();

                //If non are found, either the parent post being searched for comments exists, or it doesn't.
                if (comments.Count == 0)
                {
                    Post post = await context.posts.FindAsync(request.parent_id);
                    //If the parent doesn't exist in the first place, return a 404
                    if (post == null)
                    {
                        throw new RestException(HttpStatusCode.NotFound, new { post = "Not found" });
                    }
                    //Else, return an empty list
                    else
                    {
                        return null;
                    }
                }
                //Return the above comments
                return comments;
            }
        }
    }
}