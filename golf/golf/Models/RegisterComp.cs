using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace golf.Models
{
    public class RegisterComp
    {
    
        public Competition comp { get; set; }
        public int Holes { get; set; }
        public int Golfid { get; set; }
        public List<Hole> courseHoles = new List<Hole>();
        public List<PersonGolfer> persongolfer = new List<PersonGolfer>();
        public PersonGolfer currPerson { get; set; }
        public IList<HoleStats> holeresult { get; set; }
        public 
    }
}