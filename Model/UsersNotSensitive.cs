using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class UsersNotSensitive
    {
        public int UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Dhbw { get; set; }
        public string CourseAbr { get; set; }
        public string Course { get; set; }
        public string Specialization { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Bio { get; set; }
        public bool IsVerified { get; set; }
        public DateTime? TmsCreated { get; set; }
    }
}
