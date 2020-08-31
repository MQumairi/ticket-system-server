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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Roles
{
    public class Details
    {
        public class Query : IRequest<RoleDto>
        {
            public string role_id { get; set; }
        }

        public class Handler : IRequestHandler<Query, RoleDto>
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

            public async Task<RoleDto> Handle(Query request, CancellationToken cancellationToken)
            {
                //Handler logic goes here
                var role = await context.roles.FindAsync(request.role_id);

                if (role == null) throw new RestException(HttpStatusCode.NotFound, new { role = "Not found" });

                var roleDto = mapper.Map<Role, RoleDto>(role);

                //Get full user list, map it, place it in roleDto
                var userlist = await context.Users.OrderBy(user => user.UserName).ToListAsync();

                var userDtoList = mapper.Map<List<User>, List<UserDto>>(userlist);

                roleDto.userList = userDtoList;

                ///Get role user list, map it, place it in roleDto
                var roleUsersList = await userManager.GetUsersInRoleAsync(role.Name) as List<User>;

                var roleUsersDtoList = mapper.Map<List<User>, List<UserDto>>(roleUsersList);

                roleDto.roleUsers = roleUsersDtoList.OrderBy(user => user.username).ToList();

                return roleDto;
            }
        }
    }
}