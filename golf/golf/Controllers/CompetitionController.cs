﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using golf.Models;
using System.Globalization;



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

                //foreach (var item in databas.Golfer)
                //{
                //    if (item.Person_ID.ToString() == User.Identity.Name)
                //    {
                //        PersonGolfer pg = new PersonGolfer();
                //        pg.golfid = Convert.ToInt32(item.golfID);
                //        pg.personid = Convert.ToInt32(item.Person_ID.ToString());

                //        cc.golfers.Add(pg);
                //    }
                //}


                return PartialView("_sComp", cc);
            }

           
        }
        [HttpPost]
        public ActionResult saveComp(CreateComp cc)
        {
            if(ModelState.IsValid)
            {
                using (dsuteam4Entities1 databas = new dsuteam4Entities1())
                {
                    List<TeeTime> compStarttimes = new List<TeeTime>();
                    var comp = cc.newComp;
                    int start = Convert.ToInt32(cc.newComp.startTime);
                    int end = Convert.ToInt32(cc.newComp.endTime);

                    compStarttimes = databas.TeeTime.Where(t => t.Id >= start).Where(t => t.Id <= end).ToList();

                    foreach (var i in compStarttimes)
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
                using (dsuteam4Entities1 ndatabas = new dsuteam4Entities1())
                {
                    CreateComp c = new CreateComp();

                    c.complist = ndatabas.Competition.ToList();
                    c.currentDate = DateTime.Today;

                    return View("Index", c);
                }
            }

            return PartialView("_createComp", cc);
        }
        public ActionResult generateStartTimes(int id)
        {
            List<CompetitionGolfer> CG = RandomCGStartTimes(id);

            foreach (CompetitionGolfer item in CG)
            {
                using( dsuteam4Entities1 databas = new dsuteam4Entities1())
                {

                    CompetitionGolfer cg = databas.CompetitionGolfer.Find(item.Id);
                    cg.startTime = item.startTime;

                    databas.SaveChanges();
                }
            }

            return View(registerResult(id));
        }
        public List<CompetitionGolfer> RandomCGStartTimes(int id)
        {
            dsuteam4Entities1 databas = new dsuteam4Entities1(); //Databasconnection

            //var list = databas.CompetitionGolfer.ToList(); //Lista med alla golfare anmälda till tävlingen
            //CompetitionGolfer cg = new CompetitionGolfer();
            //List<DateTime> listaDate = new List<DateTime>();
            //DateTime start = Convert.ToDateTime(ct.startTime);
            //DateTime slut = Convert.ToDateTime(ct.endTime);

            Competition ct = databas.Competition.Find(id);
            List<CompetitionGolfer> cgList = new List<CompetitionGolfer>();

            foreach (var i in databas.CompetitionGolfer)
            {
                if (i.Competition_ID == id) //Om tävlingsID = ID
                {
                    CompetitionGolfer cg = i;
                    cgList.Add(cg);

                }
            }

            int extraStartTime = 0;

            int leftOver = cgList.Count % ct.playersPerTime;
            if (leftOver > 0)
            {
                extraStartTime = 1;
            }
            int startTimeCount = ((cgList.Count - leftOver) / ct.playersPerTime) + extraStartTime;

            List<string> startTimes = new List<string>();
            TeeTime tee = databas.TeeTime.Find(Convert.ToInt32(ct.startTime));
            DateTime compStart = Convert.ToDateTime(tee.teeTime1.ToString());
            

            for (int i = 0; i < startTimeCount; i++)
            {
                if (i == 0)
                {
                    startTimes.Add((compStart.ToShortTimeString()).ToString());

                }
                else
                {
                    

                    DateTime timeCount = compStart.AddMinutes(10*i);
                    startTimes.Add((timeCount.ToShortTimeString()).ToString());
                    
                }
            }


            List<string> randomStartTimes = RandomizeStringList(startTimes);
            List<CompetitionGolfer> randomCGList = RandomizeCGList(cgList);
            int count = 0;

            for (int i = 0; i < randomCGList.Count; i++)
            {
                if (i > 1 && (i % ct.playersPerTime == 0))
                {
                    count++;
                }
                randomCGList[i].startTime = randomStartTimes[count];

            }

            return randomCGList;

        }

        public List<CompetitionGolfer> RandomizeCGList(List<CompetitionGolfer> inputList)
        {
            List<CompetitionGolfer> randomList = new List<CompetitionGolfer>();

            Random r = new Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                randomList.Add(inputList[randomIndex]); //add it to the new, random list
                inputList.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            return randomList; 

        }
        public List<string> RandomizeStringList(List<string> inputList)
        {
            List<string> randomList = new List<string>();

            Random r = new Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                randomList.Add(inputList[randomIndex]); //add it to the new, random list
                inputList.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            return randomList;

        }
        public ActionResult saveResult()
        {
            //int compID = 16;
            //int playerID = 666;
            //int compGolfID = 1;
            //RegisterComp regcomp = new RegisterComp();
            
            //using (dsuteam4Entities1 databas = new dsuteam4Entities1())
            //{
            //    foreach (var item in databas.Hole)
            //    {
            //        HoleStats hs = new HoleStats();
            //        regcomp.holes.Add(item);
            //        hs.Hole = item;
            //        hs.CompetitionGolfer_ID = compGolfID;
            //        regcomp.holeStats.Add(hs);
            //    }
                //foreach (var item in databas.HoleStats)
                //{
                //    regcomp.holeStats.Add(item);
                //}

                //regcomp.comp = databas.Competition.Find(compID);
            //}
            return View("Index");
            }

        
        public ActionResult registerResult(int id)
        {
            
            using(dsuteam4Entities1 db = new dsuteam4Entities1())
            {
               Competition c = db.Competition.Find(id);
               RegisterComp rg = new RegisterComp();

               var cg = db.CompetitionGolfer.Where(x => x.Competition_ID == c.Id).ToList();

               

               var golfer = from g in db.Golfer.ToList()
                            join u in cg.ToList()
                            on g.Id equals u.Golfer_ID
                            select new  {
                                g.golfID,  
                                g.HCP,
                                g.Id,
                                g.Person_ID, 
                                CompGoldID = u.Id,
                                u.startTime,
                                u.HoleStats,
                                u.Golfer_ID
                            };

               var pg = from p in db.Person.ToList()
                        join y in golfer.ToList()
                        on p.Id equals y.Person_ID
                        select new
                        {
                            Personid = p.Id,
                            Golfid = y.Id,
                            fName = p.firstName,
                            lName = p.lastName,
                            Golfstring = y.golfID,
                            HCP = y.HCP,
                            Gender_ID = p.gender_ID,
                            Startime = y.startTime,
                            CompGolfID = y.CompGoldID
                            
                        };

               var gender = db.Gender.ToList();
               foreach (var i in pg)
                       {
                   PersonGolfer pe = new PersonGolfer();
                   pe.personid = i.Personid;
                   pe.golfid = i.Golfid;
                   pe.firstName = i.fName;
                   pe.lastName = i.lName;
                   pe.HCP = i.HCP;
                   pe.golfstring = i.Golfstring;
                   var g = gender.Where(x => x.Id == i.Gender_ID).FirstOrDefault();
                   pe.gender = g.genderName;
                   pe.gender_ID = g.Id;
                   
                   if (i.Startime != null)
                   {
                   pe.startime = i.Startime;

                   }
                   else
                   {
                       pe.startime = "Ej lottad";
                   }
                   rg.persongolfer.Add(pe);
                   
                       }
                    
                    
               rg.comp = c;
    

                    
                    
                   return PartialView("_regResult", rg);


            }        
        }

        public ActionResult addHoleStats(RegisterComp rc)
        {
            
            return View();
        }
        public ActionResult regResultPerson(int personid, int compID, RegisterComp regComp)
        {

            using (dsuteam4Entities1 db = new dsuteam4Entities1())
            {
                
                var golfer = from p in db.Person.ToList()
                             join g in db.Golfer.ToList()
                             on p.Id equals g.Person_ID
                             where p.Id == personid
                             select new{ 
                                 Golfid = g.Id, 
                                 Personid = p.Id, 
                                 fName = p.firstName, 
                                 lName = p.lastName, 
                                 HCP = g.HCP,
                                 Golfstring = g.golfID,
                                 Gender_ID = p.gender_ID,


                             };

                var player = golfer.FirstOrDefault();

                var compG = db.CompetitionGolfer.Where(x => x.Competition_ID == compID && x.Golfer_ID == player.Golfid).FirstOrDefault();

                var currComp = db.Competition.Where(x => x.Id == compG.Competition_ID).FirstOrDefault();

                RegisterComp rg = new RegisterComp();
                rg.Holes = currComp.NumberOfHoles;
                rg.comp = currComp;
                List<Hole> h = new List<Hole>();

                for (int i = 1; i <= currComp.NumberOfHoles; i++)
                    {
                    string n = i.ToString();
                    var hole = db.Hole.Where(x => x.Number == n).FirstOrDefault();
                    h.Add(hole);
                   
                    }

                List<HoleStats> createPlayerHoles = new List<HoleStats>();
                foreach(var i in h)
                {
                    HoleStats hs = new HoleStats();
                    hs.CompetitionGolfer_ID = compG.Id;
                    hs.Hole_ID = i.Id;
                            
                    createPlayerHoles.Add(hs);
                }

                rg.holeresult = createPlayerHoles;
                var gend = db.Gender.Where(x => x.Id == player.Gender_ID).FirstOrDefault();
             
                PersonGolfer pg = new PersonGolfer();

                pg.firstName = player.fName;
                pg.lastName = player.lName;
                pg.HCP = player.HCP;
                pg.personid = player.Personid;
                pg.golfid = player.Golfid;
                pg.golfstring = player.Golfstring;
                pg.gender_ID = gend.Id;
                pg.gender = gend.genderName;
                if(compG.startTime != null)
                {
                    pg.startime = compG.startTime;
                }
                else
                {
                    pg.startime = "Ej lottad";
                }
                rg.currPerson = pg;

                resultClass rs = new resultClass();
                rs.currentPerson = pg;
                rs.comp = currComp;
                rs.CompetitionGolferID = compG.Id;
                rs.holeresult = createPlayerHoles;
                




                        
                return PartialView("_addResult", rs);
      }
   }
        [HttpPost]
        public ActionResult regResultPerson(resultClass r)
        {
            using(dsuteam4Entities1 db = new dsuteam4Entities1())
            {

                string s = r.currentPerson.HCP;

                decimal playerHCP  = decimal.Parse(s, CultureInfo.InvariantCulture);
                foreach(var i in r.holeresult)
                {
                    HoleStats hst = new HoleStats();
                    hst.CompetitionGolfer_ID = r.CompetitionGolferID;
                    hst.Hole_ID = i.Hole_ID;
                    hst.stroaks = i.stroaks;
                    db.HoleStats.Add(hst);
                    db.SaveChanges();
                }

                List<Slope> sl = new List<Slope>();
                var slope = db.Slope.ToList();
                foreach(var i in slope)
                {
                    string xMax = i.max.ToString();
                    string xMin = i.min.ToString();
                    decimal Max = decimal.Parse(xMax, CultureInfo.CreateSpecificCulture("sv-SE"));
                    decimal Min = decimal.Parse(xMin, CultureInfo.CreateSpecificCulture("sv-SE"));
                   if(playerHCP>= Min && playerHCP<= Max && i.Gender_ID == r.currentPerson.gender_ID)
                   {
                       sl.Add(i);
                   }

                }
               var strokes = sl.FirstOrDefault();
               int extraStrokes = Convert.ToInt32(strokes.gameHCP);

               var Hcpindex = from i in r.holeresult
                              join h in db.Hole
                              on i.Hole_ID equals h.Id
                              orderby h.HCPind ascending
                              select new { 
                                  Index = h.HCPind, 
                                  Par = h.par, 
                                  Holenr = h.Number, 
                                  HoleId = h.Id, 
                                  PlayerStrokes = i.stroaks, 
                                  HolestatID=i.Id,
                                  CompetitionG = i.CompetitionGolfer_ID
                              };

               var oList = Hcpindex.ToList();
               List<ScoreCardClass> scrList = new List<ScoreCardClass>();
               foreach(var i in oList)
               {
                   ScoreCardClass scr = new ScoreCardClass();
                   scr.Id = i.HoleId;
                   scr.CompetetionGolfer_ID = i.CompetitionG;
                   scr.HCPind = i.Index;
                   scr.Number = i.Holenr;
                   scr.par = i.Par;
                   scr.playerStrokes = i.PlayerStrokes;
                   scrList.Add(scr);
               }

                if(extraStrokes > 0)
                {
                    for (var i = 0; i < extraStrokes; i++)
                    {
                        if (i < r.comp.NumberOfHoles)
                        {
                            scrList[i].addedStrokes += 1;
                        }
                        else
                        {
                            int z = i - r.comp.NumberOfHoles;
                            scrList[z].addedStrokes += 1;
                        }
                    }

                }
                else
                {
                    int x = extraStrokes * -1;
                    for(var i = 0; i < x; i++)
                    {
                        scrList[i].addedStrokes -= 1;
                    }

                }
              
                foreach(var i in scrList)
                {
                    i.calcPoints();
                }


                var compid = r.CompetitionGolferID;
                CompetitionGolfer cg = db.CompetitionGolfer.Find(compid);
                cg.net = (from i in scrList select i.net).Sum();
                cg.points = (from i in scrList select i.points).Sum();

                db.SaveChanges();

                RegisterComp rg = new RegisterComp();

                return RedirectToAction("Index");
                
            }




            return RedirectToAction("Index");
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


        public ActionResult addYourself(bool confirm, int id)
        {

            CompetitionGolfer CG = new CompetitionGolfer();
           
           

            using (dsuteam4Entities1 databas = new dsuteam4Entities1())
            {
                foreach (var golfer in databas.Golfer)
                {
                    if (User.Identity.Name == golfer.Person_ID.ToString())
                    {
                        CG.Golfer_ID = golfer.Id;
                    }
                }

                CG.Competition_ID = id;

                databas.CompetitionGolfer.Add(CG);
                databas.SaveChanges();
            }

            return RedirectToAction("Index");
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

            using (dsuteam4Entities1 d = new dsuteam4Entities1())
            {

                Competition c = d.Competition.Find(competitionid);

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

                AddCompPlayer acp = new AddCompPlayer();

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

                return View("addPlayer", acp);
            }

        }
        public PartialViewResult searchPlayer(int id, string s)
        {
                AddCompPlayer acp = new AddCompPlayer();
                Competition c = new Competition();
                List<CompetitionGolfer> cg = new List<CompetitionGolfer>();
            using (dsuteam4Entities1 d = new dsuteam4Entities1())
            {

                foreach (var i in d.CompetitionGolfer)
                {
                    if (i.Competition_ID == id)
                    {
                        cg.Add(i);
                    }
                }

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
                        int count = 0;
                        foreach(var item in cg)
                        {
                            if(pg.golfid == item.Golfer_ID)
                            {
                                count++;
                            }

                        }
                        if (count == 0)
                        {
                            acp.golfers.Add(pg);
                        }
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
        public PartialViewResult detailsComp(int id)
        {

            using(dsuteam4Entities1 db = new dsuteam4Entities1())
            {
                Competition c  = db.Competition.Find(id);

                return PartialView("_detailsComp", c);
            }


        }
        public PartialViewResult editComp(int id)
        {
            using(dsuteam4Entities1 db = new dsuteam4Entities1())
            {
                Competition c = db.Competition.Find(id);

                return PartialView("_editComp", c);

            }

        }
        [HttpPost]
        public ActionResult editComp(Competition c)
        {
            using(dsuteam4Entities1 db = new dsuteam4Entities1())
            {
                if(ModelState.IsValid)
                {
                    db.Entry(c).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                return View("Error");
            }

        }
        public PartialViewResult showResult(int id)
        {
            using(dsuteam4Entities1 db = new dsuteam4Entities1())
            {
                var cg = db.CompetitionGolfer.Where(x => x.Competition_ID == id).ToList();

                var pg = from i in db.Golfer.ToList()
                         join p in cg.ToList()
                         on i.Id equals p.Golfer_ID
                         orderby p.net descending
                         select new {Hcp = i.HCP, Net = p.net, Points = p.points, Personid = i.Person_ID, CompGid= p.Id};

                var list = pg.ToList();

                var j = from i in list
                        join p in db.Person.ToList() on i.Personid equals p.Id
                        select new {fName = p.firstName, lName = p.lastName, Hcp = i.Hcp, Net = i.Net, Points = i.Points, i.CompGid};

                List<resultClass> rslist = new List<resultClass>();
                foreach(var i in j)
                {
            
                    resultClass rs = new resultClass();
                    rs.comp = db.Competition.Find(id);
                    rs.net = i.Net;
                    rs.points = i.Points;
                    rs.CompetitionGolferID = i.CompGid;
                    PersonGolfer pge = new PersonGolfer();
                    pge.firstName = i.fName;
                    pge.lastName = i.lName;
                    pge.HCP = i.Hcp;
                    rs.currentPerson = pge;

                    rslist.Add(rs);
                }
                

                return PartialView("_showResult", rslist);
            }


           
        }
    }
}
