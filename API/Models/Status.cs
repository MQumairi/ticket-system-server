using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Status
    {
        [Key]
        public int status_id { get; set; }

        [Required]
        public string status_text { get; set; }

        [Required]
        public string status_color { get; set; }

        //Relationship with Ticket entity 
        public List<Ticket> tickets { get; set; }
    }
}