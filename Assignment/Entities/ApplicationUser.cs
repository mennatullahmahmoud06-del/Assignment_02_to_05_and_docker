using Microsoft.AspNetCore.Identity;

namespace Assignment.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
