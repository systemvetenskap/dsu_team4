﻿using System;
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
        public PartialViewResult saveComp(CreateComp cc)
        {
            using( dsuteam4Entities1 databas = new dsuteam4Entities1())
            {
                var comp = cc.newComp;

                //HÄR HÅRDKODAS DET------->
                //comp.Person_IDc = 25;
                //comp.CompeteClass_ID = 1;
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

               RegisterComp rg = new RegisterComp();

               rg.comp = c;

               return PartialView("_regResult", rg);

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

                //cc.contactlist = databas.Person.ToList();

                return PartialView("_createComp", cc);
            }
        }

    }
}
