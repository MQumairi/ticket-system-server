using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Infrastructure.Images;
using MediatR;

namespace API.Handlers.Avatars
{
    public class Delete
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ApplicationDBContext context;
            private readonly PhotoAccessor photoAccessor;

            public Handler(ApplicationDBContext context, PhotoAccessor photoAccessor)
            {
                this.photoAccessor = photoAccessor;
                this.context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //Handler logic
                var avatar = await context.profile_pics.FindAsync(request.Id);

                if(avatar == null) {
                    throw new RestException(HttpStatusCode.NotFound, new {avatar = "Not found"});
                }

                photoAccessor.DeletePhoto(request.Id);
                context.profile_pics.Remove(avatar);

                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving data");
            }

        }
    }
}