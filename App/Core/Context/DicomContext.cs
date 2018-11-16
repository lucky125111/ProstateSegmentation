using Core.Entity;
using Core.Model.NewDicom;
using Microsoft.EntityFrameworkCore;

namespace Core.Context
{
    public class DicomContext : DbContext
    {
        private readonly string _connectionString;
        public DicomContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }
        public DbSet<DicomModel> DicomModels { get; set; }
        public DbSet<DicomPatientData> DicomPatientDatas { get; set; }
        public DbSet<DicomSlice> DicomSlices{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DicomModel>()
                .HasOne(a => a.DicomPatientData)
                .WithOne(b => b.DicomModel)
                .HasForeignKey<DicomPatientData>(b => b.DicomModelId);

            modelBuilder.Entity<DicomModel>()
                .HasMany(s => s.DicomImages)
                .WithOne(g => g.DicomModel)
                .HasForeignKey(s => s.DicomModelId);

            modelBuilder.Entity<DicomModel>()
                .ToTable("Dicom");

            modelBuilder.Entity<DicomPatientData>()
                .HasKey(x => x.DicomModelId);

            modelBuilder.Entity<DicomPatientData>()
                .ToTable("DicomData");

            modelBuilder.Entity<DicomSlice>()
                .HasKey(x => new {x.DicomModelId, x.SliceIndex});

            modelBuilder.Entity<DicomSlice>()
                .ToTable("Images");
        }
    }
}