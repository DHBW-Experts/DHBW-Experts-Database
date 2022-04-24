using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class Auth0Dhbw
    {
        public Auth0Dhbw()
        {
            Auth0Users = new HashSet<Auth0User>();
        }

        public string Domain { get; set; }
        public string Location { get; set; }

        public virtual ICollection<Auth0User> Auth0Users { get; set; }
    }
}
