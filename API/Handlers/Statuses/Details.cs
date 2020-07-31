using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using API.Models.DTO;
using AutoMapper;
using MediatR;

namespace API.Handlers.Statuses
{
    public class Details
    {
        public class Query : IRequest<StatusDto>
        {
            public int status_id { get; set; }
        }

        public class Handler : IRequestHandler<Query, StatusDto>
        {
            private readonly ApplicationDBContext context;
            private readonly IMapper mapper;
            public Handler(ApplicationDBContext context, IMapper mapper)
            {
                this.mapper = mapper;
                this.context = context;
            }

            public async Task<StatusDto> Handle(Query request, CancellationToken cancellationToken)
            {
                Status status = await context.status.FindAsync(request.status_id);

                if (status == null) throw new RestException(HttpStatusCode.NotFound, new { status = "Not found" });

                StatusDto statusDto = mapper.Map<Status, StatusDto>(status);

                return statusDto;
            }
        }
    }
}