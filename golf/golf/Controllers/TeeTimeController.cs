using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using golf.Models;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Data.Entity;


namespace golf.Controllers
{
    public class TeeTimeController : Controller
    {

        dsuteam4Entities1 databas = new dsuteam4Entities1();
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
                
                cl.currDate = DateTime.Today;
                cl.dateString = cl.currDate.ToShortDateString();
                cl.selDate = DateTime.Today;


            var join = from tdate in databas.TeeTimeDate.ToList()
                       join tgolfers in databas.TeeTimeDateGolfer.ToList()
                       on tdate.Id equals tgolfers.TeeTimeDate_ID
                       select new {TeeTimeID = tdate.TeeTime_ID, Date= tdate.bookingDate, Golferid = tgolfers.Golfer_ID};

            var list1 = join.ToList();

            var join2 = from g in databas.Golfer.Include(p => p.TeeTimeDateGolfer).ToList()
                        join li in list1 on
                        g.Id equals li.Golferid
                        select new { Date = li.Date, Golfid = li.Golferid, TeeTime = li.TeeTimeID, Personid = g.Person_ID, HCP = g.HCP };

            var list2 = join2.ToList();

            var join3 = from p in databas.Person.ToList()
                        join li in list2
                        on p.Id equals li.Personid
                        where li.Date == DateTime.Today
                        select new
                        {
                            fName = p.firstName,
                            lName = p.lastName,
                            Date = li.Date,
                            TeeTime = li.TeeTime,
                            Hcp = li.HCP,
                            Gender = p.gender_ID
                            
                        };

            var list3 = join3.ToList();

            var join4 = from g in databas.Gender.ToList()
                        join p in list3
                        on g.Id equals p.Gender
                        select new
                        {

                            fName = p.fName,
                            lName = p.lName,
                            Date = p.Date,
                            TeeTime = p.TeeTime,
                            Hcp = p.Hcp,
                            Gender = g.genderName

                        };

            var list4 = join4.ToList();

            foreach(var o in list4)
            {
                BookingInfo b = new BookingInfo();
                b.Name = o.fName + " " + o.lName;
                b.TeeTime = o.TeeTime;
                b.HCP = o.Hcp;
                b.gender = o.Gender;
                cl.bNames.Add(b);

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
