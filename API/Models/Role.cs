using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace API.Models
{
    public class Role : IdentityRole
    {
        public string color { get; set; }

        [Required]
        public bool can_manage { get; set; } = false;

        [Required]
        public bool can_moderate { get; set; } = false;

    }
}