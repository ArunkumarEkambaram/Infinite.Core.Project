using System.ComponentModel.DataAnnotations;

namespace Infinite.Core.Project.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password and Confirm Password doesn't match")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Email Id")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string FullName { get; set; }

        // [Required]
        public string Role { get; set; } = "Customer";

    }

    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
