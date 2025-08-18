namespace WepApi.ModelsDto
{
    public class UqImportViewResultDto
    {
        public int Id { get; set; }
        public string? OutputType { get; set; }
        public string? OutputItem { get; set; }
        public string? OutputColor { get; set; }
        public string? RecordType { get; set; }
        public string? InputItem { get; set; }
        public string? InputColor { get; set; }
        public string? WeekSys { get; set; }
        public int? QtySys { get; set; }
        public int? QtyConfirm { get; set; }
        public string? Status { get; set; }
        public string? CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }
    }
}