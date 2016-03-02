using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace golf.Models
{

    public class CalendarClass
    {
        public DateTime currDate {get; set;}
        public DateTime nextDate { get; set; }
        public DateTime prevDate { get; set; }
        public List<TeeTime> TeeTime = new List<TeeTime>();
        public List<TeeTimeDate> TeeTimeDate = new List<TeeTimeDate>();
        public string dateString { get; set; }
       


     }
        

      
 }
