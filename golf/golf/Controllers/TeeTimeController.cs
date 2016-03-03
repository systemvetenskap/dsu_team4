using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using golf.Models;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;


namespace golf.Controllers
{
    public class TeeTimeController : Controller
    {

        private dsuteam4Entities1 databas = new dsuteam4Entities1();
        //
        // GET: /TeeTime/

        public ActionResult Index()
        {
        
                string date = DateTime.Today.ToShortDateString();
                CalendarClass cl = new CalendarClass();
                
            

                
                cl.TeeTime = databas.TeeTime.ToList();
                cl.TeeTimeDate = databas.TeeTimeDate.ToList();
                cl.TeeDate = databas.TeeDate.ToList();
                cl.TeeTimeDateGolfer = databas.TeeTimeDateGolfer.ToList();
                cl.Golfer = databas.Golfer.ToList();
                cl.Person = databas.Person.ToList();
                
                TeeTime tt = new TeeTime();
                

                cl.currDate = DateTime.Today;
                cl.dateString = cl.currDate.ToShortDateString();
                cl.selDate = DateTime.Today;

                foreach(var t in databas.TeeTimeDate)
                {
                    BookingInfo bf = new BookingInfo();
                    if(t.bookingDate == DateTime.Today)
                    {
                        foreach(var g in databas.TeeTimeDateGolfer)
                        {
                            if(g.TeeTimeDate_ID == t.Id)
                            {
                                foreach(var gf in databas.Golfer)
                                {
                                    if(gf.Id == g.Golfer_ID)
                                    {
                                        foreach(var p in databas.Person)
                                        {
                                            if(p.Id == gf.Person_ID)
                                            {
                                                bf.Name = p.firstName+" "+p.lastName;
                                                bf.TeeTime = t.TeeTime_ID;
                                                cl.bNames.Add(bf);
                                  
                                            }
                                        }
                                    }
                                }
                                
                            }
                        }
                       
                    }
                }


                return View(cl);
            
        }
        [HttpPost]
        public ActionResult test(TeeTime tt)
        {

            return View();
        }


    }
}
