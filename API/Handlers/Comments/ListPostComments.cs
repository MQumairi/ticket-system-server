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
        public class Query : IRequest<List<Comment>> { 
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
                //Find the post of that id
                Post post = await context.posts.FindAsync(request.parent_id);

                if(post == null)
                    throw new RestException (HttpStatusCode.NotFound, new {post = "Not found"});

                //Find the comments beloning to that post
                var comments = await context.comments.Where(c => c.parent_post_id == post.post_id).ToListAsync();
                
                if (comments.Count() == 0) {
                    return null;
                }

                //Return the above comments
                return comments;
            }
        }
    }
}