using Microsoft.AspNetCore.Identity;

namespace API.Models
{
    public class Role : IdentityRole
    {
        public string color { get; set; }
        
    }
}