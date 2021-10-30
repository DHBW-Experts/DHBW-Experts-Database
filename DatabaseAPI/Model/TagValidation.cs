using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class TagValidation
    {
        public int ValidationId { get; set; }
        public int? Tag { get; set; }
        public int? ValidatedBy { get; set; }
        public string Comment { get; set; }
        public DateTime? TmsCreated { get; set; }

        public virtual Tag TagNavigation { get; set; }
        public virtual User ValidatedByNavigation { get; set; }
    }
}
