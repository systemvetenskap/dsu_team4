using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace golf.Models
{
    public class PersonGolfer
    {
        public int golfid { get; set; }
        public int personid { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string HCP { get; set; }
        public string golfstring { get; set; }
        public string gender { get; set; }
        public int gender_ID { get; set; }
        public string startime { get; set;}
        public Nullable<int> points { get; set; }

    }
}