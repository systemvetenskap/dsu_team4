using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace golf.Models
{
    public class RegisterComp
    {
        public List<Hole> holes = new List<Hole>();
        public Competition comp { get; set; }
    }
}