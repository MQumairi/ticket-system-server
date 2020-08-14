using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using API.Models.DTO;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace API.Handlers.Users
{
    public class RoleUsers
    {
        public class Query : IRequest<List<UserDto>>
        {
            public string role_name { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<UserDto>>
        {
            private readonly ApplicationDBContext context;
            private readonly UserManager<User> userManager;
            private readonly IMapper mapper;
            public Handler(ApplicationDBContext context, UserManager<User> userManager, IMapper mapper)
            {
                this.mapper = mapper;
                this.userManager = userManager;
                this.context = context;
            }

            public async Task<List<UserDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var role_users = await userManager.GetUsersInRoleAsync(request.role_name);

                var users_dto = new List<UserDto>();

                foreach (var user in role_users)
                {
                    var user_dto = mapper.Map<User, UserDto>(user);
                    users_dto.Add(user_dto);
                }
                //Handler logic goes here
                return users_dto;
            }
        }
    }
}