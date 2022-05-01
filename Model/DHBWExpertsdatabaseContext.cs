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

        public virtual DbSet<Auth0Contacts> Auth0Contacts { get; set; }
        public virtual DbSet<Auth0DhbwDomains> Auth0DhbwDomains { get; set; }
        public virtual DbSet<Auth0TagValidations> Auth0TagValidations { get; set; }
        public virtual DbSet<Auth0Tags> Auth0Tags { get; set; }
        public virtual DbSet<Auth0UserData> Auth0UserData { get; set; }
        public virtual DbSet<Auth0Users> Auth0Users { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Dhbw> Dhbw { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<TagValidation> TagValidation { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<VwUsers> VwUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Auth0Contacts>(entity =>
            {
                entity.HasKey(e => new { e.User, e.Contact })
                    .HasName("UQ_contacts_no_duplicate_contacts");

                entity.ToTable("AUTH0_contacts");

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
                    .WithMany(p => p.Auth0ContactsContactNavigation)
                    .HasForeignKey(d => d.Contact)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_contacts_contact");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.Auth0ContactsUserNavigation)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_contacts_user");
            });

            modelBuilder.Entity<Auth0DhbwDomains>(entity =>
            {
                entity.HasKey(e => e.Domain)
                    .HasName("PK_dhbw_domains");

                entity.ToTable("AUTH0_dhbw_domains");

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

            modelBuilder.Entity<Auth0TagValidations>(entity =>
            {
                entity.HasKey(e => e.ValidationId)
                    .HasName("PK_tag_validations");

                entity.ToTable("AUTH0_tag_validations");

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
                    .WithMany(p => p.Auth0TagValidations)
                    .HasForeignKey(d => d.Tag)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tag_validations_tag");

                entity.HasOne(d => d.ValidatedByNavigation)
                    .WithMany(p => p.Auth0TagValidations)
                    .HasForeignKey(d => d.ValidatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tag_validations_validated_by");
            });

            modelBuilder.Entity<Auth0Tags>(entity =>
            {
                entity.HasKey(e => e.TagId)
                    .HasName("PK_tags");

                entity.ToTable("AUTH0_tags");

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
                    .WithMany(p => p.Auth0Tags)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tags-user_id");
            });

            modelBuilder.Entity<Auth0UserData>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("AUTH0_user_data");

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

                entity.Property(e => e.User)
                    .HasMaxLength(24)
                    .IsUnicode(false)
                    .HasColumnName("user")
                    .IsFixedLength(true);

                entity.HasOne(d => d.UserNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.User)
                    .HasConstraintName("FK_user_data_user_id");
            });

            modelBuilder.Entity<Auth0Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_users");

                entity.ToTable("AUTH0_users");

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
                    .WithMany(p => p.Auth0Users)
                    .HasForeignKey(d => d.EmailDomain)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_users_email_domain");
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

            modelBuilder.Entity<VwUsers>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_users");

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
