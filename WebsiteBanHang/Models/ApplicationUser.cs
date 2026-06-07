using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebsiteBanHang.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public string? Address { get; set; }

        public int? Age { get; set; }
    }
}
