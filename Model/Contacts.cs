using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class Contacts
    {
        public string User { get; set; }
        public string Contact { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual UserData ContactNavigation { get; set; }
        public virtual UserData UserNavigation { get; set; }
    }
}
