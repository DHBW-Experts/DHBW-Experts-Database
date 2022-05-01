﻿using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class Auth0UserData
    {
        public string User { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string CourseAbbr { get; set; }
        public string Course { get; set; }
        public string Specialization { get; set; }
        public string City { get; set; }
        public string Biography { get; set; }
        public string RfidId { get; set; }

        public virtual Auth0Users UserNavigation { get; set; }
    }
}
