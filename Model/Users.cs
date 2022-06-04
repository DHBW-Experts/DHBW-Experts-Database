using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class Users
    {
        public string UserId { get; set; }
        public string EmailPrefix { get; set; }
        public string EmailDomain { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual DhbwDomains EmailDomainNavigation { get; set; }
        public virtual UserData UserData { get; set; }
    }
}
