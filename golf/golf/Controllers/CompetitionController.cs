using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using golf.Models;

namespace golf.Controllers
{
    public class CompetitionController : Controller
    {
        //
        // GET: /Competition/

        public ActionResult Index()
        {
           CreateComp cc = new CreateComp();

           cc.currentDate = DateTime.Today;

            return View(cc);
        }
        public void getData()
        {

        }

    }
}
