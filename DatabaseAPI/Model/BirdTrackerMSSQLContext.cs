using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DatabaseAPI
{
    public partial class BirdTrackerMSSQLContext : DbContext
    {
        public BirdTrackerMSSQLContext()
        {
        }

        public BirdTrackerMSSQLContext(DbContextOptions<BirdTrackerMSSQLContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bird> Birds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("DHEX_DATABASE"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Bird>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Adress)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.BoxKind)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(85)
                    .IsUnicode(false);

                entity.Property(e => e.Compass)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Latitude).HasColumnType("decimal(14, 12)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(14, 12)");

                entity.Property(e => e.Message)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.NestDate).HasColumnType("date");

                entity.Property(e => e.Plz).HasColumnName("PLZ");

                entity.Property(e => e.Species)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
