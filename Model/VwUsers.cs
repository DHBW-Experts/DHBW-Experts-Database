using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class VwUsers
    {
        public string UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string DhbwLocation { get; set; }
        public string CourseAbbr { get; set; }
        public string Course { get; set; }
        public string Specialization { get; set; }
        public string City { get; set; }
        public bool? Registered { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
