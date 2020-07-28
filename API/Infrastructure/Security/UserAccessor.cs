using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace API.Infrastructure.Security
{
    public class UserAccessor
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;

        }

        public string getCurrentUsername()
        {
            var username = httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(
                x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return username;
        }
    }
}