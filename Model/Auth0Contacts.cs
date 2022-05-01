using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class Auth0Contacts
    {
        public string User { get; set; }
        public string Contact { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Auth0UserData ContactNavigation { get; set; }
        public virtual Auth0UserData UserNavigation { get; set; }
    }
}
