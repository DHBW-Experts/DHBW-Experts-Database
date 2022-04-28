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

        public virtual DbSet<Auth0Dhbw> Auth0Dhbw { get; set; }
        public virtual DbSet<Auth0User> Auth0User { get; set; }
        public virtual DbSet<Auth0UserData> Auth0UserData { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Dhbw> Dhbw { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<TagValidation> TagValidation { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Auth0Dhbw>(entity =>
            {
                entity.HasKey(e => e.Domain)
                    .HasName("AUTH0-DHBW-PK");

                entity.ToTable("AUTH0-DHBW");

                entity.Property(e => e.Domain)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("DOMAIN");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LOCATION");
            });

            modelBuilder.Entity<Auth0User>(entity =>
            {
                entity.ToTable("AUTH0-USER");

                entity.HasIndex(e => new { e.EmailPrefix, e.EmailDomain }, "AUTH0-USER-UNIQUE-NO-DUPLICATE-EMAILS")
                    .IsUnique();

                entity.Property(e => e.Auth0UserId)
                    .HasMaxLength(24)
                    .IsUnicode(false)
                    .HasColumnName("AUTH0-USER-ID")
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATED_AT")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EmailDomain)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL-DOMAIN");

                entity.Property(e => e.EmailPrefix)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL-PREFIX");

                entity.Property(e => e.Verified).HasColumnName("VERIFIED");

                entity.HasOne(d => d.EmailDomainNavigation)
                    .WithMany(p => p.Auth0User)
                    .HasForeignKey(d => d.EmailDomain)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AUTH0-USER-FK-EMAIL-DOMAIN");
            });

            modelBuilder.Entity<Auth0UserData>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("AUTH0-USER-DATA");

                entity.Property(e => e.Auth0UserId)
                    .HasMaxLength(24)
                    .IsUnicode(false)
                    .HasColumnName("AUTH0-USER-ID")
                    .IsFixedLength(true);

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

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("FIRSTNAME");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LASTNAME");

                entity.Property(e => e.RfidId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("RFID-ID");

                entity.Property(e => e.Specialization)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SPECIALIZATION");

                entity.HasOne(d => d.Auth0User)
                    .WithMany()
                    .HasForeignKey(d => d.Auth0UserId)
                    .HasConstraintName("AUTH0-USER-DATA-FK-ID");
            });

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
                    .WithMany(p => p.ContactContact1Navigation)
                    .HasForeignKey(d => d.Contact1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CONTACT-FK-USER_CONTACT");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.ContactUserNavigation)
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
                    .WithMany(p => p.Tag)
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
                    .WithMany(p => p.TagValidation)
                    .HasForeignKey(d => d.Tag)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TAG_VALIDATION-FK-TAG");

                entity.HasOne(d => d.ValidatedByNavigation)
                    .WithMany(p => p.TagValidation)
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
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.Dhbw)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("USER-FK-DHBW");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
