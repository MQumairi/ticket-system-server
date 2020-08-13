using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Errors;
using API.Models;
using API.Models.DTO;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Users
{
    public class Details
    {
        public class Query : IRequest<UserDto>
        {

            public string user_id { get; set; }
        }

        public class Handler : IRequestHandler<Query, UserDto>
        {
            private readonly ApplicationDBContext context;
            private readonly IMapper mapper;
            private readonly UserManager<User> userManager;
            public Handler(ApplicationDBContext context, IMapper mapper, UserManager<User> userManager)
            {
                this.userManager = userManager;
                this.mapper = mapper;
                this.context = context;
            }

            public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
            {
                //Handler logic goes here
                var user = await context.Users.Include(user => user.avatar).SingleOrDefaultAsync(user => user.Id == request.user_id);

                if (user == null) throw new RestException(HttpStatusCode.NotFound, new { user = "Not found" });

                var userRoles = await userManager.GetRolesAsync(user) as List<string>;

                var user_dto = mapper.Map<User, UserDto>(user);
                user_dto.Roles = userRoles;

                return user_dto;
            }
        }
    }
}