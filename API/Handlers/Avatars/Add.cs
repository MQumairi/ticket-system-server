using System;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Images;
using API.Infrastructure.Security;
using API.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Avatars
{
    public class Add
    {
        public class Command : IRequest<Photo>
        {
            public IFormFile image { get; set; }
        }

        public class Handler : IRequestHandler<Command, Photo>
        {
            private readonly ApplicationDBContext context;
            private readonly UserAccessor userAccessor;
            private readonly PhotoAccessor photoAccessor;
            public Handler(ApplicationDBContext context, UserAccessor userAccessor, PhotoAccessor photoAccessor)
            {
                this.photoAccessor = photoAccessor;
                this.userAccessor = userAccessor;
                this.context = context;
            }

            public async Task<Photo> Handle(Command request, CancellationToken cancellationToken)
            {
                //Handler logic
                var uploadResult = photoAccessor.AddPhoto(request.image);

                var user = await context.Users.Include(user => user.avatar).SingleOrDefaultAsync(user => user.Email == userAccessor.getCurrentUsername());

                var avatar = new Avatar
                {
                    Id = uploadResult.PublicId,
                    url = uploadResult.Url
                };

                if (user.avatar != null)
                {
                    photoAccessor.DeletePhoto(user.avatar_id);
                    context.photos.Remove(user.avatar);
                }

                context.photos.Add(avatar);

                user.avatar = avatar;

                var success = await context.SaveChangesAsync() > 0;
                if (success) return null;

                throw new Exception("Problem saving data");
            }
        }
    }
}