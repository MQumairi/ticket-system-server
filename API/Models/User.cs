using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace API.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string first_name {get; set;}

        [Required]
        public string surname {get; set;}

        //Relationship with Post entity
        public List<Post> posts { get; set; }

        //Relationship with Avatar entity
        public Avatar avatar { get; set; }

        [ForeignKey("avatar")]
        public string avatar_id { get; set; }

        //Relationship with ACPSettins
        public ACPSettings acpSettings { get; set; }
    }
}