﻿using System;
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

                var cmp = db.Competition.Where(x => x.name == "sffesfe").FirstOrDefault();
                rs.comp = cmp;
                rs.holeinfo = db.Hole.Take(cmp.NumberOfHoles).ToList();
                rs.currentPerson = pg;
                rs.CompetitionGolferID = db.CompetitionGolfer.Where(x => x.Golfer_ID == golfer.Id && x.Competition_ID == cmp.Id).Select(x => x.Id).FirstOrDefault();


            }
            return View("Index",rs);
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

              var Hcpindex = db.Hole.OrderByDescending(x=>x.HCPind).ToList();               

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

                foreach(var i in scrList.Where(x=>x.Id == holeid))
                {
                    i.playerStrokes = strokesIn;

                }

                var prevPar = 0;
                var order = scrList.OrderBy(x => x.Id).ToList();
                foreach(var i in order)
                {
                    if(i.playerStrokes != 0 || i.playerStrokes !=null)
                    {
                        i.calcPoints(prevPar);
                        prevPar = i.toPar;
                    }
                    else
                    {
                        break;
                    }
                  
                }

                var toPar = scrList.Where(x=>x.Id == holeid).Select(v=>v.toPar).FirstOrDefault();

                MobileStats ms = new MobileStats();
                ms.CompetitionGolfer_ID = compgid;
                ms.Hole_ID = holeid;
                ms.strokes = strokesIn;
                ms.plusMinus = toPar;
                if(db.MobileStats.Where(x=>x.Hole_ID == holeid && x.CompetitionGolfer_ID == compgid).FirstOrDefault() == null)
                {
                    db.MobileStats.Add(ms);
                    db.SaveChanges();
                }
                else
                {
                    MobileStats m = db.MobileStats.Where(x => x.Hole_ID == holeid && x.CompetitionGolfer_ID == compgid).FirstOrDefault();
                    m.strokes = strokesIn;
                    m.plusMinus = toPar;
                    db.SaveChanges();
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

    }
}