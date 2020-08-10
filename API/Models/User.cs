using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Models
{
    public class User : IdentityUser
    {
        public string avatar { get; set; }

        // public string first_name {get; set;}

        // public string surname {get; set;}

        //Relationship with Post entity
        public List<Post> posts { get; set; }
    }
}