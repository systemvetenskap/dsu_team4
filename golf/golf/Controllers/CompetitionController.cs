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
        public PartialViewResult saveComp(CreateComp cc)
        {
            using( dsuteam4Entities1 databas = new dsuteam4Entities1())
            {
                var comp = cc.newComp;



                //HÄR HÅRDKODAS DET------->
                comp.Person_IDc = 25;
                comp.CompeteClass_ID = 1;
                //HÄR HÅRDKODAS DET-------<

             
                databas.Competition.Add(comp);

               
              
                databas.SaveChanges();
                
   

     

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
