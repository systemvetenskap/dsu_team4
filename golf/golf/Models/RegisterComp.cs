using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace golf.Models
{
    public class RegisterComp
    {
        public List<Hole> holes = new List<Hole>();
        public List<HoleStats> holeStats = new List<HoleStats>();
        public Competition comp { get; set; }
        public List<CompetitionGolfer> compGolf = new List<CompetitionGolfer>();
        public List<Person> p = new List<Person>();
        public List<int> stroaks = new List<int>();
        public int compgoldID { get; set; }
  

    }
}