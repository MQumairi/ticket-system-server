using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Comment : Post
    {
        //Relationship with Post
        public Post post { get; set; }

        [Required]
        [ForeignKey("post")]
        public int parent_post_id { get; set; }
    }
}