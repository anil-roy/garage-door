using System.ComponentModel.DataAnnotations;

namespace garagedoor.Shared
{
    public class Credentials
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}