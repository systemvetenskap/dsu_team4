using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;


namespace golf.Models
{

    public class CalendarClass
    {

        public DateTime selDate { get; set; }
        public string number { get; set; }
        public string dateString { get; set; }
        
        public List<BookingInfo> bNames = new List<BookingInfo>();
        public List<BookingInfo> newBookings = new List<BookingInfo>();
        

        public IEnumerable<TeeTime> TeeTime = new List<TeeTime>();
        public IEnumerable<TeeTimeDate> TeeTimeDate = new List<TeeTimeDate>();
        public IEnumerable<TeeDate> TeeDate = new List<TeeDate>();
        public IEnumerable<TeeTimeDateGolfer> TeeTimeDateGolfer = new List<TeeTimeDateGolfer>();
        public IEnumerable<Golfer> Golfer = new List<Golfer>();
        public IEnumerable<Person> Person = new List<Person>();

        
        

     

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

        public string changeColor(int TT_ID, DateTime d)
        {
       
            string Color = "";
      
            int counter = 0;
            number = "0";
            foreach (TeeTimeDate TTD in TeeTimeDate)
            {
                if (TTD.TeeTime_ID == TT_ID && TTD.bookingDate == d)
                {
                    foreach (TeeTimeDateGolfer TTDG in TeeTimeDateGolfer)
                    {
                        if(TTDG.TeeTimeDate_ID == TTD.Id)
                        {
                            counter++;
                            if(counter > 0)
                            {
                                number = counter.ToString();
                            }
                           
                        }
                    }
                }
            }
            if (counter > 3)
            {
                Color = "grey";
            }
            else
            {
                Color = "lightgreen";


            }

            
           

            return Color;
        }

       


     }
        

      
 }
