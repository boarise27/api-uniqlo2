namespace WepApi.Data;

using Microsoft.EntityFrameworkCore;
using WepApi.Models;

public class MySQLDbContext : DbContext
{
    private readonly IConfiguration _connectionString;

    public MySQLDbContext(DbContextOptions<MySQLDbContext> options, IConfiguration connectionString)
        : base(options)
    {
        _connectionString = connectionString;
    }

    // Define your DbSets (tables) here

    public DbSet<MasterItemUniqlo> MasterItemUniqlos { get; set; }
    public DbSet<UqImportView> UqImportViews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _connectionString.GetConnectionString("DefaultMySQLConnection");
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 40));
            optionsBuilder.UseMySql(connectionString, serverVersion)
                            .LogTo(Console.WriteLine, LogLevel.Information)
                          .EnableSensitiveDataLogging()
                          .EnableDetailedErrors();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure your entity mappings here
        modelBuilder.Entity<MasterItemUniqlo>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            // Add other properties and configurations as needed
        });

        modelBuilder.Entity<UqImportView>(entity =>
        {
            entity.Property(e => e.Status).IsRequired().HasMaxLength(1).HasDefaultValue("D");
        });

        // Add more configurations as needed
        base.OnModelCreating(modelBuilder);
    }
}