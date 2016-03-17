using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace golf.Models
{
    public class showResult
    {
        public List<PersonGolfer> player { get; set; }
        public List<Hole> holeInfo { get; set; }
        public Competition comp { get; set; }
    }
}