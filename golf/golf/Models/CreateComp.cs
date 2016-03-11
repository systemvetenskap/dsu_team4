using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace golf.Models
{
    public class CreateComp
    {
        public List<Competition> complist = new List<Competition>();

        public IEnumerable<OneNamePerson> contactlist = new List<OneNamePerson>();
        public IEnumerable<CompeteClass> classList = new List<CompeteClass>();
        public IEnumerable<TeeTime> sTimes = new List<TeeTime>();
        public TeeTime startTime { get; set; }
        public TeeTime endTime { get; set; }
        public Competition newComp { get; set; }
        public DateTime currentDate { get; set; }
       

       
    }

}