using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseAPI.Model
{
    public partial class Bird
    {
        public int Id { get; set; }
        public string Species { get; set; }
        public string Adress { get; set; }
        public int? Plz { get; set; }
        public DateTime? NestDate { get; set; }
        public int? Temperature { get; set; }
        public int? NumberChicks { get; set; }
        public string BoxKind { get; set; }
        public string Compass { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string Message { get; set; }
        public string City { get; set; }
        public short? Housenumber { get; set; }
    }
}
