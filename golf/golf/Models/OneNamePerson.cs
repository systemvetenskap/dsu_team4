using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace golf.Models
{
    public class OneNamePerson
    {
        public int Id { get; set; }
        public string oneName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string streetAddres { get; set; }
        public string postalCode { get; set; }
        public string city { get; set; }
        public string email { get; set; }
        public Nullable<int> gender_ID { get; set; }
        public Nullable<int> memberType_ID { get; set; }

    }
}