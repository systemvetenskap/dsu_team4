﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using golf.Models;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Data.Entity;


namespace golf.Controllers
{
    public class TeeTimeController : Controller
    {


     
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                string id = User.Identity.Name;
              
                    ViewBag.Message = "Tidsbokning";
                    string date = DateTime.Today.ToShortDateString();
                    CalendarClass cl = new CalendarClass();
                    cl.selDate = DateTime.Today;
                    cl.dateString = DateTime.Today.ToShortDateString();

                    return View(cl);
           
           
        }
            else
        {
                return RedirectToAction("Login", "Account" );
            }
            
          
     
        }
        public PartialViewResult loadTeetimes()
        {
            using (dsuteam4Entities1 databas = new dsuteam4Entities1())
            {
                
                CalendarClass cl = new CalendarClass();

                cl.TeeTime = databas.TeeTime.ToList();
                cl.TeeTimeDate = databas.TeeTimeDate.ToList();        
                cl.TeeTimeDateGolfer = databas.TeeTimeDateGolfer.ToList();
                cl.Golfer = databas.Golfer.ToList();
                DateTime max = DateTime.Today.AddMonths(1);
             
                cl.maxDate = max.ToShortDateString();

                cl.selDate = DateTime.Today;
                cl.dateString = DateTime.Today.ToShortDateString();
                #region
                var join = from tdate in databas.TeeTimeDate.ToList()
                           join tgolfers in databas.TeeTimeDateGolfer.ToList()
                           on tdate.Id equals tgolfers.TeeTimeDate_ID
                           select new { TeeTimeID = tdate.TeeTime_ID, Date = tdate.bookingDate, Golferid = tgolfers.Golfer_ID, Admin = tgolfers.Person_IDa };

                var list1 = join.ToList();

                var join2 = from g in databas.Golfer.ToList()
                            join li in list1 on
                            g.Id equals li.Golferid
                            select new { Date = li.Date, Golfid = li.Golferid, TeeTime = li.TeeTimeID, Personid = g.Person_ID, HCP = g.HCP, li.Admin };

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
                                Gender = p.gender_ID,
                                Golfid = li.Golfid,
                                li.Admin
                               

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
                                Gender = g.genderName,
                                Golfid = p.Golfid,
                                p.Admin
                            };

                var list4 = join4.ToList();



                foreach (var o in list4)
                {
                    BookingInfo b = new BookingInfo();
                    b.Name = o.fName + " " + o.lName;
                    b.TeeTime = o.TeeTime;
                    b.HCP = o.Hcp;
                    b.gender = o.Gender;
                    b.date = DateTime.Today.ToShortDateString();
                    b.Golferid = o.Golfid;
                    b.admin = o.Admin;
                    cl.bNames.Add(b);

                }
                #endregion
                var vg = from v in databas.Golfer.ToList()
                         join b in databas.Person.ToList()
                         on v.Person_ID equals b.Id
                         select new
                         {
                             Golfid = v.golfID,
                             HCP = v.HCP,
                             GID = v.Id,
                             PID = v.Person_ID,
                             Gender = b.gender_ID

                         };

                var li1 = vg.ToList();

                var v1 = from v in databas.Gender.ToList()
                         join u in li1 on v.Id equals u.Gender
                         select new
                         {
                             u.Golfid,
                             u.HCP,
                             u.GID,
                             u.PID,
                             Gender = v.genderName
                         };


                var viewGolfers = v1.ToList();

                string id = User.Identity.Name;
                if(IsAdmin(id))
                {
                    cl.userId = Convert.ToInt32(id);
                    return PartialView("loadTeetimesMember", cl);
                }
                else
                {
                    return PartialView(cl);
                }

                
            }

           
        }
       
        public PartialViewResult saveBooking(string golfid, string teeid, string date, string personid)
        {
            using( dsuteam4Entities1 databas = new dsuteam4Entities1())
            {

           
            var TeeTimeDate1 = new TeeTimeDate() { TeeTime_ID = Convert.ToInt32(teeid), bookingDate = Convert.ToDateTime(date) };
            
            databas.TeeTimeDate.Add(TeeTimeDate1);

            databas.SaveChanges();

            var id = TeeTimeDate1.Id;

            var TeeTimeDateGolfer1 = new TeeTimeDateGolfer() { Golfer_ID = Convert.ToInt32(golfid), TeeTimeDate_ID = id, Person_IDa = Convert.ToInt32(personid) };
            
            databas.TeeTimeDateGolfer.Add(TeeTimeDateGolfer1);
            databas.SaveChanges();
            CalendarClass cl = loadData(Convert.ToDateTime(date));
            
            
            return PartialView("loadTeetimes", cl);

            }

            
        }
        public PartialViewResult deleteBooking(string golfid, string teeid, string date)
        {
            using (dsuteam4Entities1 databas = new dsuteam4Entities1())
            {


                int id = 0;
                var query = from td in databas.TeeTimeDate.ToList()
                            join tg in databas.TeeTimeDateGolfer.ToList()
                                on td.Id equals tg.TeeTimeDate_ID
                                where td.bookingDate == Convert.ToDateTime(date) 
                                select new{ td.TeeTime_ID, tg.Golfer_ID, tg.Id};
                

                var query2 = from td in query.ToList()
                             where td.TeeTime_ID == Convert.ToInt32(teeid)
                             select new {td.Id , td.Golfer_ID, td.TeeTime_ID};

                var query3 = from td in query2.ToList()
                             where td.Golfer_ID == Convert.ToInt32(golfid)
                             select td;
                var getid = query3.ToList();

                foreach(var i in getid)
                {
                    id = i.Id;
                }

                var TeeTimeDateGolfer1 = databas.TeeTimeDateGolfer.Find(id);

                databas.TeeTimeDateGolfer.Remove(TeeTimeDateGolfer1);

                databas.SaveChanges();

                CalendarClass cl = loadData(Convert.ToDateTime(date));
                return PartialView("loadTeetimes", cl);
            }


        }
        public CalendarClass loadData(DateTime c)
        {
            using(dsuteam4Entities1 databas = new dsuteam4Entities1())
            {

       
            
            CalendarClass cl = new CalendarClass();

            cl.TeeTimeDate = databas.TeeTimeDate.ToList();
            cl.TeeTime = databas.TeeTime.ToList();
            cl.TeeTimeDateGolfer = databas.TeeTimeDateGolfer.ToList();
            cl.Golfer = databas.Golfer.ToList();
            DateTime max = DateTime.Today.AddMonths(1);
         
            cl.maxDate = max.ToShortDateString();
            cl.selDate = c;

            cl.dateString = c.ToShortDateString();
            #region
            var join = from tdate in databas.TeeTimeDate.ToList()
                       join tgolfers in databas.TeeTimeDateGolfer.ToList()
                       on tdate.Id equals tgolfers.TeeTimeDate_ID
                       select new { TeeTimeID = tdate.TeeTime_ID, Date = tdate.bookingDate, Golferid = tgolfers.Golfer_ID };

            var list1 = join.ToList();

            var join2 = from g in databas.Golfer.ToList()
                        join li in list1 on
                        g.Id equals li.Golferid
                        select new { Date = li.Date, Golfid = li.Golferid, TeeTime = li.TeeTimeID, Personid = g.Person_ID, HCP = g.HCP };

            var list2 = join2.ToList();

            var join3 = from p in databas.Person.ToList()
                        join li in list2
                        on p.Id equals li.Personid
                        where li.Date == cl.selDate
                        select new
                        {
                            fName = p.firstName,
                            lName = p.lastName,
                            Date = li.Date,
                            TeeTime = li.TeeTime,
                            Hcp = li.HCP,
                            Gender = p.gender_ID,
                            Golfid = li.Golfid

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
                            Gender = g.genderName,
                            Golfid = p.Golfid

                        };

            var list4 = join4.ToList();



            foreach (var o in list4)
            {
                BookingInfo b = new BookingInfo();
                b.Name = o.fName + " " + o.lName;
                b.TeeTime = o.TeeTime;
                b.HCP = o.Hcp;
                b.gender = o.Gender;
                b.date = DateTime.Today.ToShortDateString();
                b.Golferid = o.Golfid;
                cl.bNames.Add(b);

            }
            #endregion
            var vg = from v in databas.Golfer.ToList()
                     join b in databas.Person.ToList()
                     on v.Person_ID equals b.Id
                     select new
                     {
                         Golfid = v.golfID,
                         HCP = v.HCP,
                         GID = v.Id,
                         PID = v.Person_ID,
                         Gender = b.gender_ID

                     };

            var li1 = vg.ToList();

            var v1 = from v in databas.Gender.ToList()
                     join u in li1 on v.Id equals u.Gender
                     select new
                     {
                         u.Golfid,
                         u.HCP,
                         u.GID,
                         u.PID,
                         Gender = v.genderName
                     };


            var viewGolfers = v1.ToList();

         
           return cl;
            }
            
        }

        public PartialViewResult ChangeSelDate(string newDate)
        {
            DateTime dt = Convert.ToDateTime(newDate);

            CalendarClass cl = loadData(dt);

            return PartialView("loadTeetimes", cl);
        }

        public bool IsAdmin(string id)
        {
            using(dsuteam4Entities1 databas = new dsuteam4Entities1()){

                foreach (AdminPerson AP in databas.AdminPerson)
                {
                    if (Convert.ToInt16(id) == AP.Person_ID)
                    {
                        return true;
                    }

                }
            }
        
            return false;

    }
        public string loadTeetimesAorM(string id)
        {
            if (IsAdmin(id))
            {
                return "loadTeetimes";
            }
            else
            {
               return "loadTeetimesMember";
            }
        }
    }
    
}

