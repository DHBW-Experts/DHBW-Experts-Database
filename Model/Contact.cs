using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class Contact
    {
        public int User { get; set; }
        public int Contact1 { get; set; }
        public DateTime? TmsCreated { get; set; }

        public virtual User Contact1Navigation { get; set; }
        public virtual User UserNavigation { get; set; }
    }
}
