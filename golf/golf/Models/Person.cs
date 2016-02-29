using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace golf.Models
{
    public class Person
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string streetName {get;set;}
        public string postalCode {get;set;}
        public string city { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public double HCP { get; set; }
        public int golfID { get; set; }
        public string memberType { get; set; }
        public string PW { get; set; }
        
    }
}