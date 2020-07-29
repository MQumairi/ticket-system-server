using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using MediatR;

namespace API.Handlers.Products
{
    public class Details
    {
        public class Query : IRequest<Product>
        {
            public int product_id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Product>
        {
            private readonly ApplicationDBContext context;
            public Handler(ApplicationDBContext context)
            {
                this.context = context;
            }

            public async Task<Product> Handle(Query request, CancellationToken cancellationToken)
            {
                Product product = await context.products.FindAsync(request.product_id);

                if (product == null)
                    throw new RestException(HttpStatusCode.NotFound, new { product = "Not found" });

                return product;
            }
        }
    }
}