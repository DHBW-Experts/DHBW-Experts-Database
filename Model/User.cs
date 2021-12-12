using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class User
    {
        public User()
        {
            ContactContact1Navigations = new HashSet<Contact>();
            ContactUserNavigations = new HashSet<Contact>();
            TagValidations = new HashSet<TagValidation>();
            Tags = new HashSet<Tag>();
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
        public virtual ICollection<Contact> ContactContact1Navigations { get; set; }
        public virtual ICollection<Contact> ContactUserNavigations { get; set; }
        public virtual ICollection<TagValidation> TagValidations { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
