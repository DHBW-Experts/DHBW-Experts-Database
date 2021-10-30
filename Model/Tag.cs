using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class Tag
    {
        public Tag()
        {
            TagValidations = new HashSet<TagValidation>();
        }

        public int TagId { get; set; }
        public string Tag1 { get; set; }
        public int? User { get; set; }
        public DateTime? TmsCreated { get; set; }

        public virtual User UserNavigation { get; set; }
        public virtual ICollection<TagValidation> TagValidations { get; set; }
    }
}
