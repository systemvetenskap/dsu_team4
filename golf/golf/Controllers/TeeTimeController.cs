using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using golf.Models;

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
        
                    cl.TeeTimeDate.Add(ttd);
                }
                foreach (TeeDate td in databas.TeeDate)
                {
                    cl.TeeDate.Add(td);
                }
                foreach (TeeTimeDateGolfer ttdg in databas.TeeTimeDateGolfer)
                {
                    cl.TeeTimeDateGolfer.Add(ttdg);
                }
                foreach (Golfer g in databas.Golfer)
                {
                    cl.Golfer.Add(g);
                }
                foreach (Person p in databas.Person)
                {
                    cl.Person.Add(p);
                }

                cl.currDate = DateTime.Today;
                cl.dateString = cl.currDate.ToShortDateString();
                cl.selDate = DateTime.Today;
                return View(cl);
            
        }
        [HttpPost]
        public ActionResult NextDate(string date)
        {
            DateTime dt = Convert.ToDateTime(date);

            return RedirectToAction("Index");
        }


    }
}
