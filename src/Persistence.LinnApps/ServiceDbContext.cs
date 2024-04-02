namespace Linn.ManufacturingEngineering.Persistence.LinnApps
{
    using Linn.Common.Configuration;
    using Linn.ManufacturingEngineering.Domain.LinnApps;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class ServiceDbContext : DbContext
    {
        public static readonly LoggerFactory MyLoggerFactory =
            new LoggerFactory(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() });

        public DbSet<PurchaseOrderLine> PurchaseOrderLines { get; set; }

        public DbSet<InspectionRecordHeader> InspectionRecords { get; set; }

        public DbSet<Part> Parts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Model.AddAnnotation("MaxIdentifierLength", 30);
            base.OnModelCreating(builder);
            this.BuildInspectionHeaderLines(builder);
            this.BuildInspectionRecordHeaders(builder);
            this.BuildPurchaseOrderLines(builder);
            this.BuildParts(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var host = ConfigurationManager.Configuration["DATABASE_HOST"];
            var userId = ConfigurationManager.Configuration["DATABASE_USER_ID"];
            var password = ConfigurationManager.Configuration["DATABASE_PASSWORD"];
            var serviceId = ConfigurationManager.Configuration["DATABASE_NAME"];

            var dataSource =
                $"(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={host})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME={serviceId})(SERVER=dedicated)))";

            var connectionString = $"Data Source={dataSource};User Id={userId};Password={password};";

            optionsBuilder.UseOracle(connectionString, options => options.UseOracleSQLCompatibility("11"));

            // can optionally Log any SQL that is ran by uncommenting:
            // optionsBuilder.UseLoggerFactory(MyLoggerFactory);
            optionsBuilder.EnableSensitiveDataLogging(true);
            base.OnConfiguring(optionsBuilder);
        }

        private void BuildPurchaseOrderLines(ModelBuilder builder)
        {
            var e = builder.Entity<PurchaseOrderLine>().ToTable("PL_ORDER_DETAILS");
            e.HasKey(l => new { l.OrderNumber, l.OrderLine });
            e.Property(l => l.OrderLine).HasColumnName("ORDER_LINE");
            e.Property(l => l.OrderNumber).HasColumnName("ORDER_NUMBER");
            e.Property(l => l.Qty).HasColumnName("ORDER_QUANTITY");
            e.HasOne(l => l.Part).WithMany().HasForeignKey("PART_NUMBER");
            e.HasMany(l => l.InspectionRecords).WithOne().HasForeignKey(i => new { i.OrderNumber, i.OrderLine });
        }

        private void BuildInspectionRecordHeaders(ModelBuilder builder)
        {
            var e = builder.Entity<InspectionRecordHeader>().ToTable("INSPECTION_RECORD_HEADERS");
            e.HasKey(h => h.Id);
            e.Property(h => h.Id).HasColumnName("ID");
            e.Property(h => h.PreprocessedBatch).HasColumnName("PREPROCESSED_BATCH").HasMaxLength(1);
            e.Property(h => h.DateOfEntry).HasColumnName("DATE_OF_ENTRY");
            e.Property(h => h.BatchSize).HasColumnName("BATCH_SIZE");
            e.Property(h => h.OrderNumber).HasColumnName("PURCHASE_ORDER_NUMBER");
            e.Property(h => h.OrderLine).HasColumnName("ORDER_LINE");
            e.HasMany(h => h.Lines).WithOne().HasForeignKey(l => l.HeaderId);
        }

        private void BuildInspectionHeaderLines(ModelBuilder builder)
        {
            var e = builder.Entity<InspectionRecordLine>().ToTable("INSPECTION_RECORD_LINE");
            e.HasKey(l => new { l.HeaderId, l.LineNumber });
            e.Property(l => l.HeaderId).HasColumnName("HEADER_ID");
            e.Property(l => l.LineNumber).HasColumnName("LINE_NUMBER");
            e.Property(l => l.Timestamp).HasColumnName("TIMESTAMP");
            e.Property(l => l.Status).HasColumnName("STATUS").HasMaxLength(100);
            e.Property(l => l.FailureModes).HasColumnName("FAILURE_MODES").HasMaxLength(100);
            e.Property(l => l.Material).HasColumnName("MATERIA").HasMaxLength(100);
        }

        private void BuildParts(ModelBuilder builder)
        {
            var e = builder.Entity<Part>().ToTable("PARTS");
            e.HasKey(p => p.PartNumber);
            e.Property(p => p.PartNumber).HasColumnName("PART_NUMBER");
            e.Property(p => p.Description).HasColumnName("DESCRIPTION");
        }
    }
}
