using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using API.Models.DTO;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace API.Handlers.Developers
{
    public class List
    {
        public class Query : IRequest<List<UserDto>> { }

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
                //Handler logic goes here
                var developers = await userManager.GetUsersInRoleAsync("Developer") as List<User>;

                List<UserDto> developersDto = new List<UserDto>();

                foreach (var developer in developers)
                {
                    var developerDto = mapper.Map<User, UserDto>(developer);
                    developersDto.Add(developerDto);
                }

                return developersDto;
            }
        }
    }
}