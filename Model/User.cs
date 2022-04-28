using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class User
    {
        public User()
        {
            ContactContact1Navigation = new HashSet<Contact>();
            ContactUserNavigation = new HashSet<Contact>();
            Tag = new HashSet<Tag>();
            TagValidation = new HashSet<TagValidation>();
        }

        public int UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Dhbw { get; set; }
        public string CourseAbr { get; set; }
        public string Course { get; set; }
        public string Specialization { get; set; }
        public string EmailPrefix { get; set; }
        public string City { get; set; }
        public string Biography { get; set; }
        public string RfidId { get; set; }
        public string PwHash { get; set; }
        public bool IsVerified { get; set; }
        public int VerificationId { get; set; }
        public DateTime? TmsCreated { get; set; }

        public virtual Dhbw DhbwNavigation { get; set; }
        public virtual ICollection<Contact> ContactContact1Navigation { get; set; }
        public virtual ICollection<Contact> ContactUserNavigation { get; set; }
        public virtual ICollection<Tag> Tag { get; set; }
        public virtual ICollection<TagValidation> TagValidation { get; set; }
    }
}
