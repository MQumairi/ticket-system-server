using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using API.Models.DTO;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Archives
{
    public class List
    {
        public class ArchiveEnvelop
        {
            public int archiveCount { get; set; }
            public List<TicketDto> archive { get; set; }
        }

        public class Query : IRequest<ArchiveEnvelop>
        {

            public Query(int? offset, int? limit)
            {
                this.limit = limit;
                this.offset = offset;

            }

            public int? limit { get; set; }
            public int? offset { get; set; }
        }

        public class Handler : IRequestHandler<Query, ArchiveEnvelop>
        {
            private readonly ApplicationDBContext context;
            private readonly IMapper mapper;
            public Handler(ApplicationDBContext context, IMapper mapper)
            {
                this.mapper = mapper;
                this.context = context;
            }

            public async Task<ArchiveEnvelop> Handle(Query request, CancellationToken cancellationToken)
            {
                var queryable = context.tickets.Include(ticket => ticket.product)
                                                .Include(ticket => ticket.status)
                                                .Include(ticket => ticket.author)
                                                    .ThenInclude(user => user.avatar)
                                                .Where(ticket => ticket.is_archived)
                                                .OrderBy(ticket => ticket.date_time)
                                                .AsQueryable();

                //Handler logic goes here
                var archived_tickets = await queryable
                                    .Skip(request.offset ?? 0)
                                    .Take(request.limit ?? 5)
                                    .ToListAsync();
                
                var archived_tickets_dto = mapper.Map<List<Ticket>, List<TicketDto>>(archived_tickets);
                
                ArchiveEnvelop archive_envelop = new ArchiveEnvelop
                {
                    archiveCount = queryable.Count(),
                    archive = archived_tickets_dto
                };

                return archive_envelop;
            }
        }
    }
}