using Microsoft.AspNetCore.Identity;

namespace UyumsoftProject.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }
    }
}
