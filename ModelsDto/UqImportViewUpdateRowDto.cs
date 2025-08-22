using System.Text.Json.Serialization;

namespace WepApi.ModelsDto
{
    public class UqImportViewUpdateRowDto
    {
        [JsonPropertyName("output_type")]
        public string? OutputType { get; set; }
        [JsonPropertyName("output_item")]
        public string? OutputItem { get; set; }
        [JsonPropertyName("output_color")]
        public string? OutputColor { get; set; }
        [JsonPropertyName("input_item")]
        public string? InputItem { get; set; }
        [JsonPropertyName("input_color")]
        public string? InputColor { get; set; }
        [JsonPropertyName("record_type")]
        public string? RecordType { get; set; }
        [JsonPropertyName("week_sys")]
        public string? WeekSys { get; set; }
        [JsonPropertyName("qty_sys")]
        public string? QtySys { get; set; }
        [JsonPropertyName("qty_confirm")]
        public string? QtyConfirm { get; set; }
        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }
}