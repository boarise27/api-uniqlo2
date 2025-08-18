using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WepApi.ModelsDto
{
    public class UqImportViewAddRowDto
    {
        [Required]
        [JsonPropertyName("amount")]
        public int Amount { get; set; }
        // Add other properties as needed
        [Required]
        [JsonPropertyName("type")]
        public required string Type { get; set; }

        public UqImportViewAddRowDto()
        {
            // Initialize properties if necessary
            Amount = 1;
            Type = string.Empty;
        }
    }
}