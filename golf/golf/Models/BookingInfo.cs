using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace golf.Models
{
    public class BookingInfo
    {
        public int TeeTime { get; set; }
        public string Name { get; set; }
        public int personId { get; set; }
        public string HCP { get; set; }
        public string gender { get; set; }
    }
}