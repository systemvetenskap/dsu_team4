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
        //public ActionResult saveComp(Competition cc)
        //{
        //    //using( dsuteam4Entities1 databas = new dsuteam4Entities1())
        //    //{
        //    //    databas.Competition.Add(cc);
        //    //    databas.SaveChanges();
        //    //    return View
                
        //    //}


        //}
        public void getData()
        {

        }

    }
}
