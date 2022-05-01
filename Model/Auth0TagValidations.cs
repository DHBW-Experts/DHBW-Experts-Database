using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class Auth0TagValidations
    {
        public int ValidationId { get; set; }
        public int Tag { get; set; }
        public string ValidatedBy { get; set; }
        public string Comment { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Auth0Tags TagNavigation { get; set; }
        public virtual Auth0Users ValidatedByNavigation { get; set; }
    }
}
