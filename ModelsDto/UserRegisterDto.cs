using System.ComponentModel.DataAnnotations;

namespace WepApi.ModelsDto
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public required string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public required string Password { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public required string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        public required string LastName { get; set; }
    }
}