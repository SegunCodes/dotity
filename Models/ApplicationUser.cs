using Microsoft.AspNetCore.Identity;

namespace dotity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string Name { get; set; }
    }
}