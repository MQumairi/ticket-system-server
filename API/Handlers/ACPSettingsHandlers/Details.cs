using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using API.Models.DTO;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.ACPSettingsHandlers
{
    public class Details
    {
        public class Query : IRequest<ACPSettingsDto> { }

        public class Handler : IRequestHandler<Query, ACPSettingsDto>
        {
            private readonly ApplicationDBContext context;
            private readonly IMapper mapper;
            private readonly UserManager<User> userManager;
            public Handler(ApplicationDBContext context, UserManager<User> userManager, IMapper mapper)
            {
                this.userManager = userManager;
                this.mapper = mapper;
                this.context = context;
            }

            public async Task<ACPSettingsDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var acpSettings = (await context.acp_settings.Include(a => a.founder).ToListAsync())[0];

                ACPSettingsDto aCPSettingsDto = mapper.Map<ACPSettings, ACPSettingsDto>(acpSettings);

                //Get a list of all admins
                var admin_list = await userManager.GetUsersInRoleAsync("Admin") as List<User>;

                //Map it
                var admin_list_dto = mapper.Map<List<User>, List<UserDto>>(admin_list);

                //Add it to the acpSettings Dto
                aCPSettingsDto.admin_list = admin_list_dto;

                //Handler logic goes here
                return aCPSettingsDto;
            }
        }
    }
}