using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace API.Models.DTO
{
    public class RoleDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        public string color { get; set; }

        public bool can_manage { get; set; } = false;

        public bool can_moderate { get; set; } = false;
        
        public List<UserDto> roleUsers { get; set; }
        public List<UserDto> userList { get; set; }

    }
}