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
            private readonly RoleManager<Role> roleManager;
            public Handler(ApplicationDBContext context, IMapper mapper, UserManager<User> userManager, RoleManager<Role> roleManager)
            {
                this.roleManager = roleManager;
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
                    var user_dto = mapper.Map<User, UserDto>(user);

                    var user_roles_string = await userManager.GetRolesAsync(user);

                    if (user_roles_string.Count > 0)
                    {
                        var user_role_string = user_roles_string[0];
                        var user_role = await roleManager.FindByNameAsync(user_role_string);
                        var user_role_dto = mapper.Map<Role, RoleDto>(user_role);
                        user_dto.role = user_role_dto;
                    }
                    users_dto.Add(user_dto);
                }

                return users_dto;
            }
        }
    }
}