using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace golf.Models
{
    public class BookingInfo
    {
        public int Golferid { get; set;}
        public string Golfer_ID { get; set; }
        public int TeeTime { get; set;}
        public string Name { get; set;}
        public int personId { get; set;}
        public string HCP { get; set; }
        public string gender { get; set;}
        public string date { get; set; }
        public int admin { get; set; }
    }
}