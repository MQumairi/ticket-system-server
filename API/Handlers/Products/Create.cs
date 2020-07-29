using System;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using MediatR;

namespace API.Handlers.Products
{
    public class Create
    {
        public class Command : IRequest
        {
            //Properties
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
                //Create product
                Product product = new Product {
                    product_name = request.product_name,
                    version = request.version,
                    product_image = request.product_image
                };

                context.products.Add(product);
                
                //Handler logic
                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }

        }
    }
}