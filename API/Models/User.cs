using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace API.Models
{
    public class User : IdentityUser
    {
        public string first_name {get; set;}

        public string surname {get; set;}

        //Relationship with Post entity
        public List<Post> posts { get; set; }

        //Relationship with ProfilePic entity
        public Avatar avatar { get; set; }

        [ForeignKey("avatar")]
        public string avatar_url { get; set; }
    }
}