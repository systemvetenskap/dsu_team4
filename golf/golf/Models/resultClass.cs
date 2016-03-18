using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace golf.Models
{
    public class resultClass
    {
        public PersonGolfer currentPerson { get; set; }
        public IList<HoleStats> holeresult { get; set; }
        public Competition comp { get; set; }
        public List<Competition> compList {get; set;}
        public int CompetitionGolferID { get; set; }
        public Nullable<int> points { get; set; }
        public Nullable<int> net { get; set; }
        public List<Hole> holeinfo { get; set; }

    }   
}