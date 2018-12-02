using Application.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Application.Data.Context
{
    public class DicomContext : DbContext
    {
        private readonly string _connectionString;

        public DicomContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<DicomModelEntity> DicomModels { get; set; }
        public DbSet<DicomPatientDataEntity> DicomPatientDatas { get; set; }
        public DbSet<DicomSliceEntity> DicomSlices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DicomModelEntity>()
                .HasOne(a => a.DicomPatientDataEntity)
                .WithOne(b => b.DicomModelEntity)
                .HasForeignKey<DicomPatientDataEntity>(b => b.DicomModelId);

            modelBuilder.Entity<DicomModelEntity>()
                .HasMany(s => s.DicomImages)
                .WithOne(g => g.DicomModelEntity)
                .HasForeignKey(s => s.DicomModelId);
            
            modelBuilder.Entity<DicomModelEntity>()
                .HasKey(x => x.DicomModelId);

            modelBuilder.Entity<DicomModelEntity>()
                .ToTable("Dicom");

            modelBuilder.Entity<DicomPatientDataEntity>()
                .HasKey(x => x.DicomModelId);

            modelBuilder.Entity<DicomPatientDataEntity>()
                .ToTable("DicomData");

            modelBuilder.Entity<DicomSliceEntity>()
                .HasKey(x => new {x.DicomModelId, x.InstanceNumber});

            modelBuilder.Entity<DicomSliceEntity>()
                .ToTable("Images");
        }
    }
}