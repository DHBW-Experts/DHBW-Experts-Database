using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class TagValidations
    {
        public int ValidationId { get; set; }
        public int Tag { get; set; }
        public string ValidatedBy { get; set; }
        public string Comment { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Tags TagNavigation { get; set; }
        public virtual UserData ValidatedByNavigation { get; set; }
    }
}
