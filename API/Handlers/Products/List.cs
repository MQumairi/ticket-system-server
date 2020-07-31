using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using API.Models.DTO;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Products
{
    public class List
    {
        public class Query : IRequest<List<ProductDto>> { }

        public class Handler : IRequestHandler<Query, List<ProductDto>>
        {
            private readonly ApplicationDBContext context;
            private readonly IMapper mapper;
            public Handler(ApplicationDBContext context, IMapper mapper)
            {
                this.mapper = mapper;
                this.context = context;
            }

            public async Task<List<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<Product> products = await context.products.ToListAsync();

                List<ProductDto> productDtos = new List<ProductDto>();

                foreach(Product product in products) {
                    ProductDto productDto = mapper.Map<Product, ProductDto>(product);
                    productDtos.Add(productDto);
                }

                return productDtos;
            }
        }
    }
}