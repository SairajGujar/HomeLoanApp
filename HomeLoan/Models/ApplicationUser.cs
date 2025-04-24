using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HomeLoan.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Gender { get; set; }
        [Required]
        public string AadharNumber { get; set; }

        [Required]
        public string PANNumber { get; set; }
    }
}
