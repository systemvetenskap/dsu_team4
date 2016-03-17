using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace golf.Models
{
    public class CreateComp
    {
        public List<Competition> complist = new List<Competition>();

        
        public IEnumerable<OneNamePerson> contactlist = new List<OneNamePerson>();

        public IEnumerable<CompeteClass> classList = new List<CompeteClass>();
        public IEnumerable<TeeTime> sTimes = new List<TeeTime>();
        public TeeTime startTime = new TeeTime();
        public TeeTime endTime = new TeeTime();
        [Required]
        public Competition newComp { get; set; }
        public DateTime currentDate { get; set; }
       
        public Competition comp { get; set; }

        public List<PersonGolfer> golfers = new List<PersonGolfer>();
        public List<bool> alreadySigned = new List<bool>();

        public Person currentUser { get; set; }
       
    }

}