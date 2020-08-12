using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Avatar : Photo
    {
        //Relationship with user
        [Required]
        public User user { get; set; }
    }
}