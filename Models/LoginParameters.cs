using System.ComponentModel.DataAnnotations;

namespace WebAPITs.Models
{
    /// <summary>
    /// Model to state parameters for logging in to a database
    /// </summary>
    public class LoginParameters
    {
        /// <summary>
        /// Gets or Sets the username for this user
        /// </summary>
        [Required]
        public string Username { get; set; }
        /// <summary>
        /// Gets or Sets the password for this user
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
