using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WepApi.ModelsDto
{
    public class UserLoginDto
    {
        [JsonPropertyName("username")]
        [Required(ErrorMessage = "Username is required")]
        public required string Username { get; set; }
        [JsonPropertyName("password")]
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public required string Password { get; set; }
    }
}