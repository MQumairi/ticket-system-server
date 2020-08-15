using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Ticket : Post
    {
        [Required]
        public string title { get; set; }

        //Relationship with Product Entity
        public Product product { get; set; }

        [Required]
        [ForeignKey("product")]
        public int product_id { get; set; }

        //Relationship with Status entity
        public Status status { get; set; }

        [Required]
        [ForeignKey("status")]
        public int status_id { get; set; }

        //Relationship with Comment entity
        public List<Comment> comments { get; set; }

        //Relationshop with User entity (Developer)
        public User developer { get; set; } = null;

        [ForeignKey("developer")]
        public string developer_id { get; set; } = null;

        //Is archived?
        [Required]
        public bool is_archived { get; set; } = false;
    }
}