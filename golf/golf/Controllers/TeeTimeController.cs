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
            
            
           
            return View(databas.TeeTime.ToList());
        }

    }
}
