using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class DhbwDomains
    {
        public DhbwDomains()
        {
            Users = new HashSet<Users>();
        }

        public string Domain { get; set; }
        public string Location { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
