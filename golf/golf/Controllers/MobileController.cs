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
               decimal playerHCP = decimal.Parse(s, CultureInfo.InvariantCulture);
               var slope = db.Slope.ToList();
               int extraStrokes = 0;

               if(playerHCP < 36)
               {

                   foreach (var i in slope)
                   {
                       string xMax = i.max.ToString();
                       string xMin = i.min.ToString();
                       decimal Max = decimal.Parse(xMax, CultureInfo.CreateSpecificCulture("sv-SE"));
                       decimal Min = decimal.Parse(xMin, CultureInfo.CreateSpecificCulture("sv-SE"));
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
                    i.calcPoints(prevPar);
                    prevPar = i.toPar;
                }

                var toPar = scrList.Where(x=>x.Id == holeid).Select(v=>v.toPar).FirstOrDefault();
                resultClass rs = new resultClass();
                
                return PartialView("_listScore");

            }

          
       

            

        }

    }
}
