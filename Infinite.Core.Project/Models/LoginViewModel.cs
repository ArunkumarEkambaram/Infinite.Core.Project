using System.ComponentModel.DataAnnotations;

namespace Infinite.Core.Project.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
