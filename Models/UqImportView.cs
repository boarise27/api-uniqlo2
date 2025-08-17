using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    [Table("uq_import_view")]
    public class UqImportView
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("uq_row_id")]
        public int Id { get; set; }
        [Column("output_type", TypeName = "varchar(1)")]
        public required string OutputType { get; set; }
        [Column("output_item", TypeName = "varchar(100)")]
        public required string OutputItem { get; set; }
        [Column("output_color", TypeName = "varchar(255)")]
        public string? OutputColor { get; set; }
        [Column("input_item", TypeName = "varchar(100)")]
        public string? InputItem { get; set; }
        [Column("input_color", TypeName = "varchar(255)")]
        public string? InputColor { get; set; }
        [Column("record_type", TypeName = "varchar(1)")]
        public required string RecordType { get; set; }
        [Column("week_sys", TypeName = "date")]
        public required DateTime WeekSys { get; set; }
        [Column("qty_sys", TypeName = "integer")]
        public int? QtySys { get; set; }
        [Column("qty_confirm", TypeName = "integer")]
        public int? QtyConfirm { get; set; }
        [Column("status", TypeName = "varchar(1)")]
        public required string Status { get; set; }
        [Column("created_by", TypeName = "varchar(150)")]
        public string? CreatedBy { get; set; }
        [Column("updated_by", TypeName = "varchar(150)")]
        public string? UpdatedBy { get; set; }
        [Column("created_at", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column("updated_at", TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }
        
    }
}