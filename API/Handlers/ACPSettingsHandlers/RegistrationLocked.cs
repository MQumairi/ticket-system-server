using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.ACPSettingsHandlers
{
    public class RegistrationLocked
    {
        public class Query : IRequest<bool> { }

        public class Handler : IRequestHandler<Query, bool>
        {
            private readonly ApplicationDBContext context;
            public Handler(ApplicationDBContext context)
            {
                this.context = context;
            }

            public async Task<bool> Handle(Query request, CancellationToken cancellationToken)
            {
                var acp_settings = (await context.acp_settings.ToListAsync())[0];

                if(acp_settings == null) throw new RestException(HttpStatusCode.NotFound, new {settings = "Not found"});

                //Handler logic goes here
                return acp_settings.registration_locked;
            }
        }
    }
}