using System.ComponentModel.DataAnnotations;

namespace WebAPITs.Models
{
    public class APIAccount 
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}