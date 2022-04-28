using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class Auth0User
    {
        public string Auth0UserId { get; set; }
        public string EmailPrefix { get; set; }
        public string EmailDomain { get; set; }
        public bool Verified { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Auth0Dhbw EmailDomainNavigation { get; set; }
    }
}
