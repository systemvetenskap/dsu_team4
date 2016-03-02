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

                foreach(TeeTime t in databas.TeeTime)
                {
                    cl.TeeTime.Add(t);
                }           
                cl.currDate = DateTime.Today;
                cl.dateString = cl.currDate.ToShortDateString();
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
