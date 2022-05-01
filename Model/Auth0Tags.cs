using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class Auth0Tags
    {
        public Auth0Tags()
        {
            Auth0TagValidations = new HashSet<Auth0TagValidations>();
        }

        public int TagId { get; set; }
        public string Tag { get; set; }
        public string User { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Auth0Users UserNavigation { get; set; }
        public virtual ICollection<Auth0TagValidations> Auth0TagValidations { get; set; }
    }
}
