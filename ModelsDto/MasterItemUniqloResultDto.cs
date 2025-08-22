using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WepApi.ModelsDto
{
    public class MasterItemUniqloResultDto
    {
        [Required]
        [JsonPropertyName("type")]
        public required string? Type { get; set; }
        [Required]
        [JsonPropertyName("uniqlo_code")]
        public required string UniqloCode { get; set; }
        [JsonPropertyName("uniqlo_color")]
        public string? UniqloColor { get; set; }
        [JsonPropertyName("uniqlo_desc")]
        public string? UniqloDesc { get; set; }
        [JsonPropertyName("greige_code")]
        public string? GreigeCode { get; set; }
        [JsonPropertyName("fabric_code")]
        public string? FabricCode { get; set; }
        [JsonPropertyName("fabric_color")]
        public string? FabricColor { get; set; }
        [JsonPropertyName("fabric_desc")]
        public string? FabricDesc { get; set; }
        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }

    }
}