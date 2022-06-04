using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class UserData
    {
        public UserData()
        {
            ContactsContactNavigation = new HashSet<Contacts>();
            ContactsUserNavigation = new HashSet<Contacts>();
            TagValidations = new HashSet<TagValidations>();
            Tags = new HashSet<Tags>();
        }

        public string User { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string CourseAbbr { get; set; }
        public string Course { get; set; }
        public string Specialization { get; set; }
        public string City { get; set; }
        public string Biography { get; set; }
        public string RfidId { get; set; }

        public virtual Users UserNavigation { get; set; }
        public virtual ICollection<Contacts> ContactsContactNavigation { get; set; }
        public virtual ICollection<Contacts> ContactsUserNavigation { get; set; }
        public virtual ICollection<TagValidations> TagValidations { get; set; }
        public virtual ICollection<Tags> Tags { get; set; }
    }
}
