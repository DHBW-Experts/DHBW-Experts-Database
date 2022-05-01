using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class Auth0UserData
    {
        public Auth0UserData()
        {
            Auth0ContactsContactNavigation = new HashSet<Auth0Contacts>();
            Auth0ContactsUserNavigation = new HashSet<Auth0Contacts>();
            Auth0TagValidations = new HashSet<Auth0TagValidations>();
            Auth0Tags = new HashSet<Auth0Tags>();
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

        public virtual Auth0Users UserNavigation { get; set; }
        public virtual ICollection<Auth0Contacts> Auth0ContactsContactNavigation { get; set; }
        public virtual ICollection<Auth0Contacts> Auth0ContactsUserNavigation { get; set; }
        public virtual ICollection<Auth0TagValidations> Auth0TagValidations { get; set; }
        public virtual ICollection<Auth0Tags> Auth0Tags { get; set; }
    }
}
