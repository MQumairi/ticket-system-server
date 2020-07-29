using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using MediatR;

namespace API.Handlers.Products
{
    public class Delete
    {
        public class Command : IRequest
        {
            //Properties
            public int product_id { get; set; }
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

                context.products.Remove(product);
                
                //Handler logic
                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }

        }
    }
}