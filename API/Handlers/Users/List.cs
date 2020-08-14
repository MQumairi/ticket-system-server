using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using API.Models.DTO;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Users
{
    public class List
    {
        public class Query : IRequest<List<UserDto>> { }

        public class Handler : IRequestHandler<Query, List<UserDto>>
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

            public async Task<List<UserDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                //Handler logic goes here
                var users = await context.Users
                                    .Include(user => user.avatar)
                                    .ToListAsync();

                var users_dto = new List<UserDto>();

                foreach (var user in users)
                {
                    var user_roles = await userManager.GetRolesAsync(user) as List<string>;
                    var user_dto = mapper.Map<User, UserDto>(user);
                    user_dto.Roles = user_roles;
                    users_dto.Add(user_dto);
                }

                return users_dto;
            }
        }
    }
}