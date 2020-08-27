using System;
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

        //Relationshop with User entity 
        [Column("author")]
        public User author { get; set; }

        [Required]
        [ForeignKey("author")]
        public string author_id { get; set; }


        //Relationshop wtih Attachment entity
        public Attachment attachment { get; set; }

        [ForeignKey("attachment")]
        public string attachment_id { get; set; }
    }
}