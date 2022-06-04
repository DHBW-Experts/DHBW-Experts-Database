using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class Tags
    {
        public Tags()
        {
            TagValidations = new HashSet<TagValidations>();
        }

        public int TagId { get; set; }
        public string Tag { get; set; }
        public string User { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual UserData UserNavigation { get; set; }
        public virtual ICollection<TagValidations> TagValidations { get; set; }
    }
}
