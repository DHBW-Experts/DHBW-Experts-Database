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

        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Dhbw> Dhbws { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<TagValidation> TagValidations { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasKey(e => new { e.User, e.Contact1 })
                    .HasName("CONTACT-UNIQUE-NO-DUPLICATE-CONTACTS");

                entity.ToTable("CONTACT");

                entity.Property(e => e.User).HasColumnName("USER");

                entity.Property(e => e.Contact1).HasColumnName("CONTACT");

                entity.Property(e => e.TmsCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("TMS-CREATED")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Contact1Navigation)
                    .WithMany(p => p.ContactContact1Navigations)
                    .HasForeignKey(d => d.Contact1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CONTACT-FK-USER_CONTACT");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.ContactUserNavigations)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CONTACT-FK-USER");
            });

            modelBuilder.Entity<Dhbw>(entity =>
            {
                entity.HasKey(e => e.Location)
                    .HasName("DHBW-PK");

                entity.ToTable("DHBW");

                entity.Property(e => e.Location)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LOCATION");

                entity.Property(e => e.EmailDomain)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL-DOMAIN");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("TAG");

                entity.HasIndex(e => new { e.Tag1, e.User }, "TAG-UNIQUE-NO-DUPLICATE-TAGS")
                    .IsUnique();

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
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TAG-FK-USER");
            });

            modelBuilder.Entity<TagValidation>(entity =>
            {
                entity.HasKey(e => e.ValidationId)
                    .HasName("TAG_VALIDATION-PK");

                entity.ToTable("TAG-VALIDATION");

                entity.HasIndex(e => new { e.Tag, e.ValidatedBy }, "TAG-VALIDATION-UNIQUE-NO-DUPLICATE-VALIDATIONS")
                    .IsUnique();

                entity.Property(e => e.ValidationId).HasColumnName("VALIDATION-ID");

                entity.Property(e => e.Comment)
                    .HasMaxLength(250)
                    .HasColumnName("COMMENT")
                    .UseCollation("Latin1_General_100_CI_AI_SC_UTF8");

                entity.Property(e => e.Tag).HasColumnName("TAG");

                entity.Property(e => e.TmsCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("TMS-CREATED")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ValidatedBy).HasColumnName("VALIDATED-BY");

                entity.HasOne(d => d.TagNavigation)
                    .WithMany(p => p.TagValidations)
                    .HasForeignKey(d => d.Tag)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TAG_VALIDATION-FK-TAG");

                entity.HasOne(d => d.ValidatedByNavigation)
                    .WithMany(p => p.TagValidations)
                    .HasForeignKey(d => d.ValidatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TAG_VALIDATION-FK-USER");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("USER");

                entity.HasIndex(e => new { e.EmailPrefix, e.Dhbw }, "USER-UNIQUE-NO-DUPLICATE-EMAILS")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("USER-ID");

                entity.Property(e => e.Biography)
                    .HasMaxLength(1000)
                    .HasColumnName("BIOGRAPHY")
                    .UseCollation("Latin1_General_100_CI_AI_SC_UTF8");

                entity.Property(e => e.City)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CITY");

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
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("DHBW");

                entity.Property(e => e.EmailPrefix)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL-PREFIX");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("FIRSTNAME");

                entity.Property(e => e.IsVerified).HasColumnName("IS-VERIFIED");

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

                entity.Property(e => e.Specialization)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SPECIALIZATION");

                entity.Property(e => e.TmsCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("TMS-CREATED")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.VerificationId).HasColumnName("VERIFICATION-ID");

                entity.HasOne(d => d.DhbwNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Dhbw)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("USER-FK-DHBW");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
