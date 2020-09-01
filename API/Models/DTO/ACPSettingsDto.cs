using System.Collections.Generic;

namespace API.Models.DTO
{
    public class ACPSettingsDto
    {
        public UserDto founder { get; set; }
        public bool registration_locked { get; set; }
        public List<UserDto> admin_list { get; set; }

    }
}