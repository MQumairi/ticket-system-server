using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using API.Models.DTO;
using AutoMapper;
using MediatR;

namespace API.Handlers.Products
{
    public class Details
    {
        public class Query : IRequest<ProductDto>
        {
            public int product_id { get; set; }
        }

        public class Handler : IRequestHandler<Query, ProductDto>
        {
            private readonly ApplicationDBContext context;
            private readonly IMapper mapper;
            public Handler(ApplicationDBContext context, IMapper mapper)
            {
                this.mapper = mapper;
                this.context = context;
            }

            public async Task<ProductDto> Handle(Query request, CancellationToken cancellationToken)
            {
                Product product = await context.products.FindAsync(request.product_id);

                if (product == null)
                    throw new RestException(HttpStatusCode.NotFound, new { product = "Not found" });

                var productDto = mapper.Map<Product, ProductDto>(product);

                return productDto;
            }
        }
    }
}