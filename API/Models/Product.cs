using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Product
    {
        [Key]
        public int product_id { get; set; }

        [Required]
        public string product_name { get; set; }

        public string version { get; set; }

        public string product_image { get; set; }

        //Relationship with Ticket Entity 
        public List<Ticket> tickets { get; set; }
        
    }
}