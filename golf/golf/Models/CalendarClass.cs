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
        public List<TeeDate> TeeDate = new List<TeeDate>();
        public List<TeeTimeDateGolfer>  TeeTimeDateGolfer = new List<TeeTimeDateGolfer> ();
  
        public string dateString { get; set; }

        public bool bookable(int TTD_ID)
        {
            int counter = 0;
            foreach (TeeTimeDateGolfer TTDG in TeeTimeDateGolfer)
            {
                if (TTD_ID == TTDG.TeeTimeDate_ID)
                {
                    counter++;
                }

            }
            if (counter > 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string changeColor(int TT_ID)
        {
            string Color;
            int counter = 0;
            foreach (TeeTimeDate TTD in TeeTimeDate)
            {
                if (TTD.TeeTime_ID == TT_ID)
                {
                    foreach (TeeTimeDateGolfer TTDG in TeeTimeDateGolfer)
                    {
                        if(TTDG.TeeTimeDate_ID == TTD.Id)
                        {
                            counter++;
                        }
                    }
                }
            }
            if (counter > 3)
            {
                Color = "gray";
            }
            else
            {
                Color = "lightgreen";
            }
            return Color;
        }
       


     }
        

      
 }
