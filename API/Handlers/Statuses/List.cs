using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using API.Models.DTO;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Statuses
{
    public class List
    {
        public class Query : IRequest<List<StatusDto>> { }

        public class Handler : IRequestHandler<Query, List<StatusDto>>
        {
            private readonly ApplicationDBContext context;
            private readonly IMapper mapper;
            public Handler(ApplicationDBContext context, IMapper mapper)
            {
                this.mapper = mapper;
                this.context = context;
            }

            public async Task<List<StatusDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<Status> statuses = await context.status.ToListAsync();

                List<StatusDto> statusDtos = new List<StatusDto>();

                foreach(Status status in statuses) {
                    StatusDto statusDto = mapper.Map<Status, StatusDto>(status);
                    statusDtos.Add(statusDto);
                }

                return statusDtos;
            }
        }
    }
}