using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class Dhbw
    {
        public Dhbw()
        {
            Users = new HashSet<User>();
        }

        public string Location { get; set; }
        public string EmailDomain { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
