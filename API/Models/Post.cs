using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Post
    {
        [Key]
        public int post_id { get; set; }

        [Required]
        public DateTime date_time { get; set; }

        [Required]
        public string description { get; set; }


        //Relationship with Comment entity
        public List<Comment> comments { get; set; }


        //Relationshop with User entity 
        public User user { get; set; }

        [Required]
        [ForeignKey("user")]
        public int author_id { get; set; }
    }
}