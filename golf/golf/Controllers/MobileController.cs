using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using golf.Models;
using System.Data.Entity;


namespace golf.Controllers
{
    public class MobileController : Controller
    {
        //
        // GET: /Mobile/

        public ActionResult Index()
        {
            //Hårdkodning Person---------Fixa detta från inloggning

            int id = 922;

            //Hårkodning------------------

            resultClass rs = new resultClass();

            using(dsuteam4Entities1 db = new dsuteam4Entities1())
            {


               
                Person p = db.Person.Include("Golfer").Where(x=>x.Id == id).FirstOrDefault();
                PersonGolfer pg = new PersonGolfer();
                var golfer = p.Golfer.FirstOrDefault();

                pg.personid = p.Id;
                pg.golfid = golfer.Id;
                pg.firstName = p.firstName;
                pg.lastName = p.lastName;
                
                rs.compList = db.Competition.Where(x => x.cDate == DateTime.Today).ToList();
                rs.currentPerson = pg;


            }
            return View("Index",rs);
        }

    }
}
