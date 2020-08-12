using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Photo
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string url {get; set;}
    }
}