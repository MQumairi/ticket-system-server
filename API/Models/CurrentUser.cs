using System.Collections.Generic;
using API.Models.DTO;

namespace API.Models
{
    public class CurrentUser
    {
        public string id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string first_name {get; set;}
        public string surname {get; set;}
        public string token { get; set; }
        public AvatarDto avatar {get; set;}
        public RoleDto role { get; set; }
    }
}