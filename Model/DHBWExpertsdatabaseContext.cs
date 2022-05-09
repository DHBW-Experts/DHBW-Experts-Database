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

        public virtual DbSet<Contacts> Contacts { get; set; }
        public virtual DbSet<DhbwDomains> DhbwDomains { get; set; }
        public virtual DbSet<TagValidations> TagValidations { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }
        public virtual DbSet<UserData> UserData { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<VwUsers> VwUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Contacts>(entity =>
            {
                entity.HasKey(e => new { e.User, e.Contact })
                    .HasName("UQ_contacts_no_duplicate_contacts");

                entity.ToTable("contacts");

                entity.Property(e => e.User)
                    .HasMaxLength(24)
                    .IsUnicode(false)
                    .HasColumnName("user")
                    .IsFixedLength(true);

                entity.Property(e => e.Contact)
                    .HasMaxLength(24)
                    .IsUnicode(false)
                    .HasColumnName("contact")
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ContactNavigation)
                    .WithMany(p => p.ContactsContactNavigation)
                    .HasForeignKey(d => d.Contact)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_contacts_contact");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.ContactsUserNavigation)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_contacts_user");
            });

            modelBuilder.Entity<DhbwDomains>(entity =>
            {
                entity.HasKey(e => e.Domain);

                entity.ToTable("dhbw_domains");

                entity.Property(e => e.Domain)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("domain");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("location");
            });

            modelBuilder.Entity<TagValidations>(entity =>
            {
                entity.HasKey(e => e.ValidationId);

                entity.ToTable("tag_validations");

                entity.HasIndex(e => new { e.Tag, e.ValidatedBy }, "UQ_tag_validations_no_duplicate_validations")
                    .IsUnique();

                entity.Property(e => e.ValidationId).HasColumnName("validation_id");

                entity.Property(e => e.Comment)
                    .HasMaxLength(250)
                    .HasColumnName("comment")
                    .UseCollation("Latin1_General_100_CI_AI_SC_UTF8");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Tag).HasColumnName("tag");

                entity.Property(e => e.ValidatedBy)
                    .IsRequired()
                    .HasMaxLength(24)
                    .IsUnicode(false)
                    .HasColumnName("validated_by")
                    .IsFixedLength(true);

                entity.HasOne(d => d.TagNavigation)
                    .WithMany(p => p.TagValidations)
                    .HasForeignKey(d => d.Tag)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tag_validations_tag");

                entity.HasOne(d => d.ValidatedByNavigation)
                    .WithMany(p => p.TagValidations)
                    .HasForeignKey(d => d.ValidatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tag_validations_validated_by");
            });

            modelBuilder.Entity<Tags>(entity =>
            {
                entity.HasKey(e => e.TagId);

                entity.ToTable("tags");

                entity.HasIndex(e => new { e.Tag, e.User }, "UQ_tags_no_duplicate_tags")
                    .IsUnique();

                entity.Property(e => e.TagId).HasColumnName("tag_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Tag)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("tag");

                entity.Property(e => e.User)
                    .IsRequired()
                    .HasMaxLength(24)
                    .IsUnicode(false)
                    .HasColumnName("user")
                    .IsFixedLength(true);

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.Tags)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tags-user_id");
            });

            modelBuilder.Entity<UserData>(entity =>
            {
                entity.HasKey(e => e.User)
                    .HasName("PK_user-data");

                entity.ToTable("user_data");

                entity.Property(e => e.User)
                    .HasMaxLength(24)
                    .IsUnicode(false)
                    .HasColumnName("user")
                    .IsFixedLength(true);

                entity.Property(e => e.Biography)
                    .HasMaxLength(1000)
                    .HasColumnName("biography")
                    .UseCollation("Latin1_General_100_CI_AI_SC_UTF8");

                entity.Property(e => e.City)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("city");

                entity.Property(e => e.Course)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("course");

                entity.Property(e => e.CourseAbbr)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("course_abbr");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("firstname");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("lastname");

                entity.Property(e => e.RfidId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("rfid_id");

                entity.Property(e => e.Specialization)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("specialization");

                entity.HasOne(d => d.UserNavigation)
                    .WithOne(p => p.UserData)
                    .HasForeignKey<UserData>(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_data_user_id");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("users");

                entity.HasIndex(e => new { e.EmailPrefix, e.EmailDomain }, "UQ_users_no_duplicate_emails")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasMaxLength(24)
                    .IsUnicode(false)
                    .HasColumnName("user_id")
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EmailDomain)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("email_domain");

                entity.Property(e => e.EmailPrefix)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("email_prefix");

                entity.HasOne(d => d.EmailDomainNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.EmailDomain)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_users_email_domain");
            });

            modelBuilder.Entity<VwUsers>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_users");

                entity.Property(e => e.Biography)
                    .HasMaxLength(1000)
                    .HasColumnName("biography")
                    .UseCollation("Latin1_General_100_CI_AI_SC_UTF8");

                entity.Property(e => e.City)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("city");

                entity.Property(e => e.Course)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("course");

                entity.Property(e => e.CourseAbbr)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("course_abbr");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.DhbwLocation)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("dhbw_location");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(61)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("firstname");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("lastname");

                entity.Property(e => e.Registered).HasColumnName("registered");

                entity.Property(e => e.Specialization)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("specialization");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(24)
                    .IsUnicode(false)
                    .HasColumnName("user_id")
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
