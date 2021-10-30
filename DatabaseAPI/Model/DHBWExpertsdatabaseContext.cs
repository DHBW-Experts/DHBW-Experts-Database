using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class DHBWExpertsdatabaseContext : DbContext
    {
        public DHBWExpertsdatabaseContext()
        {
        }

        public DHBWExpertsdatabaseContext(DbContextOptions<DHBWExpertsdatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Dhbw> Dhbws { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<TagValidation> TagValidations { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:dhbw-experts.database.windows.net,1433;Initial Catalog=DHBW-Experts-database;Persist Security Info=False;User ID=dhbwexpertsadmin;Password=dhbw21SE;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Dhbw>(entity =>
            {
                entity.HasKey(e => e.Location)
                    .HasName("PK__DHBW__7B4298B4DBC5F1CC");

                entity.ToTable("DHBW");

                entity.Property(e => e.Location)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LOCATION");

                entity.Property(e => e.EmailDomain)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL-DOMAIN");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("TAG");

                entity.Property(e => e.TagId).HasColumnName("TAG-ID");

                entity.Property(e => e.Tag1)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TAG");

                entity.Property(e => e.TmsCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("TMS-CREATED")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.User).HasColumnName("USER");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.Tags)
                    .HasForeignKey(d => d.User)
                    .HasConstraintName("FK__TAG__USER__07C12930");
            });

            modelBuilder.Entity<TagValidation>(entity =>
            {
                entity.HasKey(e => e.ValidationId)
                    .HasName("PK__TAG-VALI__E9A8CAA1FE1997EC");

                entity.ToTable("TAG-VALIDATION");

                entity.Property(e => e.ValidationId).HasColumnName("VALIDATION-ID");

                entity.Property(e => e.Comment)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("COMMENT");

                entity.Property(e => e.Tag).HasColumnName("TAG");

                entity.Property(e => e.TmsCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("TMS-CREATED")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidatedBy).HasColumnName("VALIDATED-BY");

                entity.HasOne(d => d.TagNavigation)
                    .WithMany(p => p.TagValidations)
                    .HasForeignKey(d => d.Tag)
                    .HasConstraintName("FK__TAG-VALIDAT__TAG__0B91BA14");

                entity.HasOne(d => d.ValidatedByNavigation)
                    .WithMany(p => p.TagValidations)
                    .HasForeignKey(d => d.ValidatedBy)
                    .HasConstraintName("FK__TAG-VALID__VALID__0C85DE4D");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("USER");

                entity.HasIndex(e => e.Email, "UQ__USER__161CF7248441E8B6")
                    .IsUnique();

                entity.HasIndex(e => e.RfidId, "UQ__USER__2468517CDBCB09D4")
                    .IsUnique();

                entity.HasIndex(e => e.PwHash, "UQ__USER__6828FAFE118A310D")
                    .IsUnique();

                entity.HasIndex(e => e.ConfirmationId, "UQ__USER__EEBB967A77D36F6A")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("USER-ID");

                entity.Property(e => e.Bio)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("BIO");

                entity.Property(e => e.City)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CITY");

                entity.Property(e => e.ConfirmationId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CONFIRMATION-ID");

                entity.Property(e => e.Course)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("COURSE");

                entity.Property(e => e.CourseAbr)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COURSE-ABR");

                entity.Property(e => e.Dhbw)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("DHBW");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("FIRSTNAME");

                entity.Property(e => e.IsConfirmed).HasColumnName("IS-CONFIRMED");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LASTNAME");

                entity.Property(e => e.PwHash)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("PW-HASH");

                entity.Property(e => e.RfidId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("RFID-ID");

                entity.Property(e => e.TmsCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("TMS-CREATED")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.DhbwNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Dhbw)
                    .HasConstraintName("FK__USER__DHBW__7A672E12");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
