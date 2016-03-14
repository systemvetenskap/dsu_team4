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
            using (dsuteam4Entities1 databas = new dsuteam4Entities1())
            {

          
                cc.classList = databas.CompeteClass.ToList();
                List<Person> p = databas.Person.ToList();


                

                List<OneNamePerson> op = new List<OneNamePerson>();
                foreach (Person i in p)
                {
                    OneNamePerson onp = new OneNamePerson();
                    onp.Id = i.Id;
                    onp.oneName = i.firstName + " " + i.lastName;
                    onp.firstName = i.firstName;
                    onp.lastName = i.lastName;
                    onp.streetAddres = i.streetAddres;
                    onp.postalCode = i.postalCode;
                    onp.city = i.city;
                    onp.email = i.email;
                    onp.gender_ID = i.gender_ID;
                    onp.memberType_ID = i.memberType_ID;
                    op.Add(onp);
                }
                cc.contactlist = op.OrderBy(x => x.oneName);

                cc.complist = databas.Competition.ToList();
                cc.sTimes = databas.TeeTime.ToList();
                //cc.contactlist = databas.Person.ToList();

                return View(cc);
            }
            

            
        }
        public ActionResult loadComp()
        {
            using( dsuteam4Entities1 databas = new dsuteam4Entities1())
            {

                CreateComp cc = new CreateComp();
                cc.complist = databas.Competition.ToList();


                return PartialView("_sComp", cc);
            }

           
        }
        public ActionResult saveComp(CreateComp cc)
        {
            using( dsuteam4Entities1 databas = new dsuteam4Entities1())
            {
                List<TeeTime> compStarttimes = new List<TeeTime>();
                var comp = cc.newComp;
                int start = Convert.ToInt32(cc.newComp.startTime);
                int end = Convert.ToInt32(cc.newComp.endTime);
               
                compStarttimes = databas.TeeTime.Where(t => t.Id >= start).Where(t => t.Id <= end).ToList();
                
                foreach(var i in compStarttimes)
                {
                    TeeTimeDate td = new TeeTimeDate();

                    td.TeeTime_ID = i.Id;
                    td.bookingDate = cc.newComp.cDate;
                    td.Disabled = true;

                    databas.TeeTimeDate.Add(td);

                }
                
  
               databas.Competition.Add(comp); 
               databas.SaveChanges();
                
            }
            using( dsuteam4Entities1 ndatabas = new dsuteam4Entities1())
            {
                CreateComp c = new CreateComp();

                c.complist = ndatabas.Competition.ToList();
                c.currentDate = DateTime.Today;

                return View("Index", c);
            }

        }
        public void slumpa(int id)
        {
            dsuteam4Entities1 databas = new dsuteam4Entities1();

            var list = databas.CompetitionGolfer.ToList();
            
            foreach(var i in databas.CompetitionGolfer.ToList())
            {
                if(i.Competition_ID == id)
                {
                    
                }
            }
        }
        public ActionResult saveResult()
        {
            int compID = 16;
            int playerID = 666;
            int compGolfID = 1;
            RegisterComp regcomp = new RegisterComp();
            
            using (dsuteam4Entities1 databas = new dsuteam4Entities1())
            {
                foreach (var item in databas.Hole)
                {
                    HoleStats hs = new HoleStats();
                    regcomp.holes.Add(item);
                    hs.Hole = item;
                    hs.CompetitionGolfer_ID = compGolfID;
                    regcomp.holeStats.Add(hs);
                }
                //foreach (var item in databas.HoleStats)
                //{
                //    regcomp.holeStats.Add(item);
                //}

                regcomp.comp = databas.Competition.Find(compID);
            }
            return View(regcomp);
        }

        
        public ActionResult registerResult(int id)
        {
            
            using(dsuteam4Entities1 db = new dsuteam4Entities1())
            {
               Competition c = db.Competition.Find(id);

               //if (c.CompetitionGolfer.Count == 0)

                   RegisterComp rg = new RegisterComp();
                   rg.comp = c;

                   //foreach (var item in rg.comp.CompetitionGolfer)
                   //{
                   //    rg.p.Add(item.Golfer.Person);
                   //}


                   foreach (var item in db.CompetitionGolfer)
                   {
                       if (item.Competition_ID == id)
                       {
                           rg.p.Add(item.Golfer.Person);
                           rg.stroaks.Add(0);
                       }
                   }
                    
                    
                   return PartialView("_regResult", rg);


            }        
        }

        public ActionResult addHoleStats(List<HoleStats> hsList)
        {
            
            return View();
        }
        public ActionResult regResultPerson(int golfID, int compID, RegisterComp regComp)
        {

            using (dsuteam4Entities1 db = new dsuteam4Entities1())
            {
                
                int compgolf;

                RegisterComp rc = new RegisterComp();

                List<Hole> h = new List<Hole>();

                foreach (var item in db.Hole)
	                {
		                 h.Add(item);
	                }

                foreach (var item in db.CompetitionGolfer)
                {
                    if (item.Golfer_ID == 911 /*golfID*/ && item.Competition_ID == compID)
                    {
                        compgolf = item.Id;


                        for (int i = 0; i < 2; /*item.Competition.NumberOfHoles;*/ i++)
                        {
                            HoleStats hs = new HoleStats();




                            //hs.CompetitionGolfer_ID = 4;
                            //hs.Hole_ID = 1;
                            //hs.stroaks = 0;
                            //hs.Id = null;
                            rc.compgoldID = item.Id;


                            hs.CompetitionGolfer_ID = item.Id;
                            hs.Hole_ID = h[i].Id;
                            hs.stroaks = 0;

                            //db.HoleStats.Add(hs);
                            //db.SaveChanges();



                            hs.Hole = h[i];
                            hs.CompetitionGolfer = item;


                            rc.holeStats.Add(hs);
                            
                        }
                        
                    }
                }

                foreach (var item in rc.holeStats)
                {
                    db.HoleStats.Add(item);
                    db.SaveChanges();
                }

                


                return PartialView("_regResultPerson", rc);
            }
        }

        public ActionResult createComp()
        {
            CreateComp cc = new CreateComp();

            cc.currentDate = DateTime.Today;
            using (dsuteam4Entities1 databas = new dsuteam4Entities1())
            {



                cc.classList = databas.CompeteClass.ToList();
                List<Person> p = databas.Person.ToList();
                List<OneNamePerson> op = new List<OneNamePerson>();
                foreach (Person i in p)
                {
                    OneNamePerson onp = new OneNamePerson();
                    onp.Id = i.Id;
                    onp.oneName = i.firstName + " " + i.lastName;
                    onp.firstName = i.firstName;
                    onp.lastName = i.lastName;
                    onp.streetAddres = i.streetAddres;
                    onp.postalCode = i.postalCode;
                    onp.city = i.city;
                    onp.email = i.email;
                    onp.gender_ID = i.gender_ID;
                    onp.memberType_ID = i.memberType_ID;
                    op.Add(onp);
                }
                cc.contactlist = op.OrderBy(x => x.oneName);

                cc.complist = databas.Competition.ToList();

                cc.sTimes = databas.TeeTime.ToList();
             

                return PartialView("_createComp", cc);
            }
        }
        public ActionResult addPlayer(int id)
        {


            using(dsuteam4Entities1 d = new dsuteam4Entities1())
            {

                Competition c = d.Competition.Find(id);
                
                var join = from p in d.Person.ToList()
                           join g in d.Golfer.ToList()
                           on p.Id equals g.Person_ID
                           select new { 
                           
                               p.firstName, 
                               p.lastName, 
                               p.Id, 
                               Golfstring = g.golfID,
                               g.HCP,
                               Golfid = g.Id,
                               p.gender_ID,
                                                   
                           };

                var list = join.ToList();

                var persong = from p in list 
                              join g in d.Gender.ToList() 
                              on p.gender_ID equals g.Id 
                              select new 
                              { 
                                personid = p.Id,
                                fName = p.firstName,
                                lName = p.lastName,
                                p.Golfstring,
                                HCP = p.HCP,
                                Gender = g.genderName,
                                p.Golfid,
                                p.gender_ID
                              
                              
                              };
                var toView = persong.ToList();

                AddCompPlayer acp = new AddCompPlayer();
                
                foreach(var i in toView)
                {
                    PersonGolfer pg = new PersonGolfer();

                    pg.personid = i.personid;
                    pg.firstName = i.fName;
                    pg.lastName = i.lName;
                    pg.golfstring = i.Golfstring;
                    pg.HCP = i.HCP;
                    pg.gender = i.Gender;
                    pg.golfid = i.Golfid;
                    pg.gender_ID = Convert.ToInt16(i.gender_ID);

                    if (c.CompeteClass_ID == i.gender_ID || c.CompeteClass_ID == 1)
                    {
                    acp.golfers.Add(pg);
                    }

                }
               

                
                acp.comp = c;

                return View("addPlayer", acp);
            }

           
        }
        public ActionResult RegisterPlayer(int golferid , int competitionid)
        {
            CompetitionGolfer CG = new CompetitionGolfer();
            CG.Golfer_ID = golferid;
            CG.Competition_ID = competitionid;
            
            using (dsuteam4Entities1 databas = new dsuteam4Entities1())
            {
                databas.CompetitionGolfer.Add(CG);
                databas.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        public PartialViewResult searchPlayer(int id, string s)
        {
                AddCompPlayer acp = new AddCompPlayer();
                Competition c = new Competition();
            using (dsuteam4Entities1 d = new dsuteam4Entities1())
            {

                 c = d.Competition.Find(id);

                var join = from p in d.Person.ToList()
                           join g in d.Golfer.ToList()
                           on p.Id equals g.Person_ID
                           select new
                           {

                               p.firstName,
                               p.lastName,
                               p.Id,
                               Golfstring = g.golfID,
                               g.HCP,
                               Golfid = g.Id,
                               p.gender_ID,

                           };

                var list = join.ToList();

                var persong = from p in list
                              join g in d.Gender.ToList()
                              on p.gender_ID equals g.Id
                              select new
                              {
                                  personid = p.Id,
                                  fName = p.firstName,
                                  lName = p.lastName,
                                  p.Golfstring,
                                  HCP = p.HCP,
                                  Gender = g.genderName,
                                  p.Golfid,
                                  p.gender_ID


                              };
                var toView = persong.ToList();


                foreach (var i in toView)
                {
                    PersonGolfer pg = new PersonGolfer();

                    pg.personid = i.personid;
                    pg.firstName = i.fName;
                    pg.lastName = i.lName;
                    pg.golfstring = i.Golfstring;
                    pg.HCP = i.HCP;
                    pg.gender = i.Gender;
                    pg.golfid = i.Golfid;
                    pg.gender_ID = Convert.ToInt16(i.gender_ID);

                    if (c.CompeteClass_ID == i.gender_ID || c.CompeteClass_ID == 1)
                    {
                        acp.golfers.Add(pg);
                    }

                }



                acp.comp = c;
            }

            searchClass sc = new searchClass();
            AddCompPlayer adc = new AddCompPlayer();
            adc.golfers = sc.getPersonGolfers(acp.golfers, s);
            adc.comp = c;
            return PartialView("_searchPlayer", adc);

        }
        public ActionResult test()
        {
            //lazy loading
            using(dsuteam4Entities1 databas = new dsuteam4Entities1())
            {
                var personer = databas.Person;
                foreach(var rad in personer)
                {                   
                    foreach(var rad2 in rad.Golfer)
                    {
                        PersonGolfer pg = new PersonGolfer();
                        foreach(var rad3 in rad2.CompetitionGolfer)
                        {
                            pg.lastName = rad.lastName;
                            pg.HCP = rad2.HCP;
                            
                        }
                   
                    }
                    
                }
               
            }

            //Eager-loading
            using (dsuteam4Entities1 databas = new dsuteam4Entities1())
            {
                var person = databas.Person.Include("Golfer");
                foreach(var r in person)
                {
                    foreach(var r2 in r.Golfer)
                    {
                        var namn = r.firstName;
                        var hcp = r2.HCP;
                    }

                }




            }
            //Explicit 
            using (dsuteam4Entities1 databas = new dsuteam4Entities1())
            {
                var pers = databas.Person.ToList();
                
                foreach(var i in databas.Person)
                {
                    databas.Entry(i).Collection(x => x.Golfer).Load();

                    foreach(var p in i.Golfer)
                    {
                        var namn = i.firstName;
                        var hcp = p.HCP;
                    }
                }
            }


            return View();
        }

    }
}
