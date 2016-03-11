using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace golf.Models
{
    public class AddCompPlayer
    {
        public Competition comp { get; set; }
        public List<PersonGolfer> golfers = new List<PersonGolfer>();
    }
}