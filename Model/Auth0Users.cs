using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class Auth0Users
    {
        public Auth0Users()
        {
            Auth0ContactsContactNavigation = new HashSet<Auth0Contacts>();
            Auth0ContactsUserNavigation = new HashSet<Auth0Contacts>();
            Auth0TagValidations = new HashSet<Auth0TagValidations>();
            Auth0Tags = new HashSet<Auth0Tags>();
        }

        public string UserId { get; set; }
        public string EmailPrefix { get; set; }
        public string EmailDomain { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Auth0DhbwDomains EmailDomainNavigation { get; set; }
        public virtual ICollection<Auth0Contacts> Auth0ContactsContactNavigation { get; set; }
        public virtual ICollection<Auth0Contacts> Auth0ContactsUserNavigation { get; set; }
        public virtual ICollection<Auth0TagValidations> Auth0TagValidations { get; set; }
        public virtual ICollection<Auth0Tags> Auth0Tags { get; set; }
    }
}
