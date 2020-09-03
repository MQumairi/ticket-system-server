using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class ACPSettings
    {
        public int id { get; set; }
        public User founder { get; set; }

        [Required]
        [ForeignKey("founder")]
        public string founder_id { get; set; }

        [Required]
        public bool registration_locked { get; set; }
    }
}