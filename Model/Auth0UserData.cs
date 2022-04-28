using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class Auth0UserData
    {
        public string Auth0UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string CourseAbr { get; set; }
        public string Course { get; set; }
        public string Specialization { get; set; }
        public string City { get; set; }
        public string Biography { get; set; }
        public string RfidId { get; set; }

        public virtual Auth0User Auth0User { get; set; }
    }
}
