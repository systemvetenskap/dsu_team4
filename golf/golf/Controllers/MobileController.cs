using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using golf.Models;
using System.Data.Entity;
using System.Globalization;

namespace golf.Controllers
{
    public class MobileController : Controller
    {
        //
        // GET: /Mobile/

     
        public ActionResult MobileStart(int cid)
        {
           

            int id = Convert.ToInt32(User.Identity.Name);

           

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

                var cmp = db.Competition.Where(x => x.name == "sffesfe").FirstOrDefault();
                rs.comp = cmp;
                rs.holeinfo = db.Hole.Take(cmp.NumberOfHoles).ToList();
                rs.currentPerson = pg;
                rs.CompetitionGolferID = db.CompetitionGolfer.Where(x => x.Golfer_ID == golfer.Id && x.Competition_ID == cmp.Id).Select(x => x.Id).FirstOrDefault();

                return View("MobileStart", rs);
            }
           
        }
        [Authorize]
        public ActionResult Index()
        {

            using(dsuteam4Entities1 db = new dsuteam4Entities1())
            {

                var comp = db.Competition.Where(x => x.cDate == DateTime.Today).ToList();
                resultClass rs = new resultClass();
                rs.compList = comp;

                return View("Index", rs);


            }

        }
        public PartialViewResult updateResult(int compid, int compgid, int strokesIn, int holeid)
        {
            List<Slope> sl = new List<Slope>();
            List<ScoreCardClass> scrList = new List<ScoreCardClass>();
            
         

            using(dsuteam4Entities1 db = new dsuteam4Entities1())
            {
                

               int golfid = db.CompetitionGolfer.Where(j => j.Id == compgid).Select(u => u.Golfer_ID).FirstOrDefault();
               string s =  db.Golfer.Where(x=>x.Id == golfid).Select(y => y.HCP).FirstOrDefault();
               int pid = db.Golfer.Where(x=>x.Id == golfid).Select(y => y.Person_ID).FirstOrDefault();
               Nullable<int> gender = db.Person.Where(x=>x.Id == pid).Select(u => u.gender_ID).FirstOrDefault();
               var competition = db.Competition.Where(x=>x.Id == compid).FirstOrDefault();             
               var slope = db.Slope.ToList();
               int extraStrokes = 0;
               string test1 = s.Replace(".",",");
               decimal playerHCP = decimal.Parse(test1);

               if(playerHCP < 36)
               {

                   foreach (var i in slope)
                   {
                       string xMax = i.max.ToString();
                       string xMin = i.min.ToString();
          
                       decimal Min = decimal.Parse(xMin);
                       decimal Max = decimal.Parse(xMax);
  
                       if (playerHCP >= Min && playerHCP <= Max && i.Gender_ID == gender)
                       {
                           sl.Add(i);
                       }

                   }
                   var strokes = sl.FirstOrDefault();
                   extraStrokes = Convert.ToInt32(strokes.gameHCP);

               }
               else
               {
                   extraStrokes = 40;
               }

              var Hcpindex = db.Hole.OrderBy(x=>x.HCPind).ToList();               

                foreach(var i in Hcpindex)
                {
                  ScoreCardClass scr = new ScoreCardClass();
                  scr.Id = i.Id;
                
                  scr.HCPind = i.HCPind;
                  scr.Number = i.Number;
                  scr.par = i.par;
                  scr.CompetetionGolfer_ID = compgid;
                  scrList.Add(scr);

                }
            
                if(extraStrokes > 0)
                {
                    for (var i = 0; i < extraStrokes; i++)
                    {
                        if (i < competition.NumberOfHoles)
                        {
                            scrList[i].addedStrokes += 1;
                        }
                        else if(i < (competition.NumberOfHoles  * 2))
                        {
                            int z = i - competition.NumberOfHoles;
                            scrList[z].addedStrokes += 1;
                        }
                        else
                        {
                            int x = i - (competition.NumberOfHoles*2);
                            scrList[x].addedStrokes += 1;
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

                var getPar = db.MobileStats.Where(x => x.CompetitionGolfer_ID == compgid).ToList();

                if(getPar.Count == 0)
                {
                        //
                        int getPrevHole = 0;
                                   
                        foreach (var i in scrList.Where(x => x.Id == holeid))
                        {
                            i.playerStrokes = strokesIn;
                            i.calcPoints(getPrevHole);

                        }
                     var toPar = scrList.Where(x=>x.Id == holeid).FirstOrDefault();
                 
                        MobileStats ms = new MobileStats();
                        ms.CompetitionGolfer_ID = compgid;
                        ms.Hole_ID = holeid;
                        ms.strokes = strokesIn;
                        ms.plusMinus =toPar.toPar;
                        db.MobileStats.Add(ms);
                        db.SaveChanges();
                    
                        
                }
                else if(getPar.Count > 0)
                {
                    var get = getPar.Where(x=>x.CompetitionGolfer_ID == compgid && x.Hole_ID == holeid).FirstOrDefault();
                    if(get == null)
                    {
                        int getPrevHole = db.MobileStats.OrderByDescending(x => x.Id).Select(x => x.plusMinus).First();

                        foreach (var i in scrList.Where(x => x.Id == holeid))
                        {
                            i.playerStrokes = strokesIn;
                            i.calcPoints(getPrevHole);

                        }
                        var toPar = scrList.Where(x => x.Id == holeid).FirstOrDefault();
                        MobileStats m = new MobileStats();
                        m.Hole_ID = holeid;
                        m.CompetitionGolfer_ID = compgid;
                        m.strokes = strokesIn;
                        m.plusMinus = toPar.toPar;
                        db.MobileStats.Add(m);
                        db.SaveChanges();

                    }
                    else
                    {
                        string cHole = db.Hole.Where(x => x.Id == holeid).Select(x=>x.Number).FirstOrDefault();
                        int pHole = Convert.ToInt32(cHole) - 1;
                        var prevHolePar = 0;
                        if(pHole > 0)
                        {
                            string s1 = pHole.ToString();
                            var getHole = db.Hole.Where(x => x.Number == s1).Select(x => x.Id).FirstOrDefault();
                            prevHolePar = db.MobileStats.Where(x => x.CompetitionGolfer_ID == compgid && x.Hole_ID == getHole).Select(x => x.plusMinus).FirstOrDefault();
                        }
               
                        foreach (var i in scrList.Where(x => x.Id == holeid))
                        {
                            i.playerStrokes = strokesIn;
                            i.calcPoints(prevHolePar);

                        }
                        var toPar = scrList.Where(x => x.Id == holeid).FirstOrDefault();
                        MobileStats m = db.MobileStats.Where(x => x.Hole_ID == holeid && x.CompetitionGolfer_ID == compgid).First();         
                        m.strokes = strokesIn;
                        m.plusMinus = toPar.toPar;
                       
                        db.SaveChanges();    


                    }
   
                }

               




               
                
            }

            using(dsuteam4Entities1 dbo = new dsuteam4Entities1())
            {
                dbo.Configuration.LazyLoadingEnabled = true;
                List<PersonGolfer> listPlayers = new List<PersonGolfer>();
                var competitiongolfer = dbo.CompetitionGolfer.Where(x => x.Competition_ID == compid).ToList();

                var g = dbo.Golfer.ToList();

                var m = dbo.MobileStats.ToList();

                List<MobileStats> mobilesta = new List<MobileStats>();
                 
                foreach(var i in competitiongolfer)
                {
                    MobileStats n = new MobileStats();
                    List<MobileStats> add = m.Where(x => x.CompetitionGolfer_ID == i.Id).ToList();
                    MobileStats getH = add.OrderByDescending(x => x.Hole_ID).FirstOrDefault();
                    n = getH;
                    if(n != null)
                    {
                        mobilesta.Add(n);
                    }
                  
                
                }
 

                var join1 = from c in competitiongolfer
                            join p in g
                            on c.Golfer_ID equals p.Id
                            select new { Cgid = c.Id, p.Person_ID };

                var t1 = join1.ToList();

                var join2 = from c in mobilesta.ToList()
                            join j in t1.ToList()
                            on c.CompetitionGolfer_ID equals j.Cgid
                            select new { c.plusMinus, c.strokes, j.Person_ID };

                var t2 = join2.ToList();

                var join3 = from p in dbo.Person.ToList()
                            join j in t2.ToList()
                            on p.Id equals j.Person_ID
                            select new { p.firstName, p.lastName, j.plusMinus, j.strokes };

                var t3 = join3.ToList();
                var toview = join3.ToList();
                foreach(var i in toview)
                {
                    PersonGolfer pg = new PersonGolfer();
                    pg.firstName = i.firstName;
                    pg.lastName = i.lastName;
                    pg.toPar = i.plusMinus;
                    pg.points = i.strokes;
                    listPlayers.Add(pg);
                }

             



                return PartialView("_listScore", listPlayers);

            }

          
       

            

        }
        //public PartialViewResult prevHole(int compid, int compgid, int holeid)
        //{

        //    using (dsuteam4Entities1 db = new dsuteam4Entities1())
        //    {
        //        string cHole = db.Hole.Where(x => x.Id == holeid).Select(x=>x.Number).FirstOrDefault();
        //        int pHole = Convert.ToInt32(cHole) - 1;
        //        var getHole = db.Hole.Where(x => x.Number == pHole.ToString()).Select(x => x.Id).FirstOrDefault();
        //        var currHole = db.MobileStats.Where(x => x.CompetitionGolfer_ID == compgid && x.Hole_ID == getHole).FirstOrDefault();

        //        resultClass rs = new resultClass();
        //        rs.comp = db.Competition.Where()


        //    }

        //    return PartialView("_listScore", rs);
        //}

    }
}
