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
        public PartialViewResult saveComp(CreateComp ccomp)
        {
            using( dsuteam4Entities1 databas = new dsuteam4Entities1())
            {
                Competition cc = ccomp.newComp;

                cc.Person_IDc = 1;
                cc.CompeteClass_ID = 1;
            
                databas.Competition.Add(cc);
                try
                {
                    databas.SaveChanges();
                }
               catch(System.Data.Entity.Infrastructure.DbUpdateException e)
                {
                    string s = e.ToString();
                }

     

            }
            using( dsuteam4Entities1 ndatabas = new dsuteam4Entities1())
            {
                CreateComp c = new CreateComp();

                c.complist = ndatabas.Competition.ToList();
                c.currentDate = DateTime.Today;

                return PartialView("_sComp", c);
            }

        }
        public void getData()
        {

        }

    }
}
