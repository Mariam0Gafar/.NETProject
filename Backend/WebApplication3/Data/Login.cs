using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Data
{
    public class Login
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
