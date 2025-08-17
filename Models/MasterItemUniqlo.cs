using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    [Table("master_item_uniqlo")]
    public class MasterItemUniqlo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("uniqlo_id")]
        public int Id { get; set; }
        [Column("uniqlo_code", TypeName = "varchar(100)")]
        public required string UniqloCode { get; set; }
        [Column("uniqlo_color", TypeName = "varchar(100)")]
        public string? UniqloColor { get; set; }
        [Column("uniqlo_desc", TypeName = "varchar(255)")]
        public string? UniqloDesc { get; set; }

        [Column("type", TypeName = "varchar(1)")]
        public string? Type { get; set; }
        [Column("greige_code", TypeName = "varchar(100)")]
        public string? GreigeCode { get; set; }
        [Column("greige_desc", TypeName = "varchar(255)")]
        public string? GreigeDesc { get; set; }
        [Column("fabric_code", TypeName = "varchar(100)")]
        public string? FabricCode { get; set; }
        [Column("fabric_color", TypeName = "varchar(255)")]
        public string? FabricColor { get; set; }
        [Column("fabric_desc", TypeName = "varchar(255)")]
        public string? FabricDesc { get; set; }
        [Column("is_active", TypeName = "tinyint(1)")]
        public bool IsActive { get; set; }
        [Column("created_by", TypeName = "varchar(150)")]
        public string? CreatedBy { get; set; }
        [Column("updated_by", TypeName = "varchar(150)")]
        public string? UpdatedBy { get; set; }
        [Column("created_at", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column("updated_at", TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        public MasterItemUniqlo()
        {
            IsActive = true; // Default value for IsActive
            CreatedBy = "System";
            UpdatedBy = "System"; // Default value for UpdatedBy // Set UpdatedAt to current time
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}