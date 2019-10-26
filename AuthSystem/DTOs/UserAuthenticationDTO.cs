using System.ComponentModel.DataAnnotations;

namespace AuthSystem.DTOs
{
    public class UserAuthenticationDTO
    {
        [Required]
        [MinLength(1, ErrorMessage = "Username must be at least 1 character")]
        public string Username { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Password must be at least 1 character")]
        public string Password { get; set; }
    }
}
