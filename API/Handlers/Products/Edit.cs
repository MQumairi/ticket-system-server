using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using MediatR;

namespace API.Handlers.Products
{
    public class Edit
    {
        public class Command : IRequest
        {
            //Properties
            public int product_id { get; set; }
            public string product_name { get; set; }
            public string version { get; set; }
            public string product_image { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ApplicationDBContext context;
            public Handler(ApplicationDBContext context)
            {
                this.context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                Product product = await context.products.FindAsync(request.product_id);

                if(product == null) throw new RestException(HttpStatusCode.NotFound, new {product = "Not found"});

                product.product_name = request.product_name ?? product.product_name;
                product.version = request.version ?? product.version;
                product.product_image = request.product_image ?? product.product_image;

                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }

        }
    }
}