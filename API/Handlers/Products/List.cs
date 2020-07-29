using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Products
{
    public class List
    {
        public class Query : IRequest<List<Product>> { }

        public class Handler : IRequestHandler<Query, List<Product>>
        {
            private readonly ApplicationDBContext context;
            public Handler(ApplicationDBContext context)
            {
                this.context = context;
            }

            public async Task<List<Product>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<Product> products = await context.products.ToListAsync();

                return products;
            }
        }
    }
}