using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class Auth0DhbwDomains
    {
        public Auth0DhbwDomains()
        {
            Auth0Users = new HashSet<Auth0Users>();
        }

        public string Domain { get; set; }
        public string Location { get; set; }

        public virtual ICollection<Auth0Users> Auth0Users { get; set; }
    }
}
