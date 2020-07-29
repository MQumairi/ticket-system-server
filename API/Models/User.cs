using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Models
{
    public class User : IdentityUser
    {
        public string avatar { get; set; }

        //Relationship with Post entity
        public List<Post> posts { get; set; }
    }
}