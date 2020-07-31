using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Comment : Post
    {
        //Relationship with Ticket
        public Ticket ticket { get; set; }

        [Required]
        [ForeignKey("ticket")]
        public int parent_post_id { get; set; }
    }
}