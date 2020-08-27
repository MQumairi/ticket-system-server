using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using API.Models.DTO;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Tickets
{
    public class FilterTickets
    {
        public class Query : IRequest<List<TicketDto>>
        {
            // public int test { get; set; }
            public List<int> product_ids { get; set; }
            public List<int> status_ids { get; set; }
            public string date_from { get; set; }
            public string date_to { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<TicketDto>>
        {
            private readonly ApplicationDBContext context;
            private readonly IMapper mapper;
            public Handler(ApplicationDBContext context, IMapper mapper)
            {
                this.mapper = mapper;
                this.context = context;
            }

            public async Task<List<TicketDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                //Get tickets from context
                var tickets = await context.tickets
                                            .Include(ticket => ticket.product)
                                            .Include(ticket => ticket.status)
                                            .Include(ticket => ticket.author)
                                                .ThenInclude(user => user.avatar)
                                            .Where(ticket => !ticket.is_archived)
                                            .ToListAsync();

                // throw new RestException(HttpStatusCode.NotFound, new {error = "Test is " + request.product_ids[0]});

                DateTime date_from = stringToDate(request.date_from);
                DateTime date_to = stringToDate(request.date_to);

            
                //Build a Filters object
                Filters filters = new Filters(request.product_ids, request.status_ids, date_from, date_to);

                //Filter tickets
                var filteredTickets = filters.filterTickets(tickets);

                //Map them to DtOs
                var filteredTicketsDto = mapper.Map<List<Ticket>, List<TicketDto>>(filteredTickets);

                //Return the mapping
                return filteredTicketsDto;
            }

            private DateTime stringToDate (string dateString) {

                if(dateString == null) return DateTime.MinValue;

                return DateTime.Parse(dateString, System.Globalization.CultureInfo.InvariantCulture);
            }
        }
    }
}