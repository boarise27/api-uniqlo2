using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WepApi.ModelsDto
{
    public class UserRegisterDto
    {
        [JsonPropertyName("username")]
        [Required(ErrorMessage = "Username is required.")]
        public required string Username { get; set; }
        [JsonPropertyName("password")]
        [Required(ErrorMessage = "Password is required.")]
        public required string Password { get; set; }

        [JsonPropertyName("email")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public required string Email { get; set; }

        [JsonPropertyName("f_name")]
        [Required(ErrorMessage = "First name is required.")]
        public required string FirstName { get; set; }
        [JsonPropertyName("l_name")]
        [Required(ErrorMessage = "Last name is required.")]
        public required string LastName { get; set; }
    }
}