using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WepApi.ModelsDto
{
    public class UqImportViewUpdateByColumn
    {
        [Required]
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [Required]
        [JsonPropertyName("column_name")]
        public string? ColumnName { get; set; }
        [JsonPropertyName("new_value")]
        public string? NewValue { get; set; }
    }
}