﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using golf.Models;
using golf.person;




namespace golf.Controllers
{
    public class MemberManagementController : Controller
    {

        //Kontext, databas, EF 
        dsuteam4Entities1 databas = new dsuteam4Entities1();
 
        //Lista Personer från persontabellen    
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                string id = User.Identity.Name;
                
                
                if(IsAdmin(id))
                {
                    return View(databas.Person.ToList());
            }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            
        
        }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public ActionResult searchMember(string searchstring)
        {

            searchClass sc = new searchClass();

           List<Person> p = sc.getPersons(searchstring);

            return PartialView("_searchMember", p);

        }
     

        //Visa detaljer om personer
        public ActionResult Details(int id)
        {
            
            Person person = databas.Person.Find(id);
            Golfer golf = databas.Golfer.Where(x => x.Person_ID == id).FirstOrDefault();


            ViewBag.gid = golf.golfID.ToString();
            ViewBag.HCP = golf.HCP.ToString();
          
           
           
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }


        //Öppna skapa View(Vy)
        public ActionResult Create()
        {
            CreateMember CM = new CreateMember();
            SelectList gender = new SelectList(databas.Gender.ToList(), "id", "genderName");


            CM.genderItems = gender;

            return View(CM);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateMember person)
        {
            if (ModelState.IsValid)
            {


                person.p.gender_ID = person.genderid;     
                databas.Person.Add(person.p);
                databas.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(person);
        }


        public ActionResult Edit(int id)
        {
            CreateMember CM = new CreateMember();
            CM.p = databas.Person.Find(id);
            SelectList gender = new SelectList(databas.Gender.ToList(), "id", "genderName");


            var selectListItems = new List<SelectListItem>();

           
           

            selectListItems.Add(new SelectListItem() { Text = "Betalt", Value = bool.TrueString });
            selectListItems.Add(new SelectListItem() { Text = "Inte betalt", Value = bool.FalseString });

            CM.payed = selectListItems;
            

            CM.genderItems = gender;
            if (CM.p == null)
            {
                return HttpNotFound();
            }
            return View(CM);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateMember CM)
        {
            if (ModelState.IsValid)
            {
                
                SelectList gender = new SelectList(databas.Gender.ToList(), "id", "genderName");

                //Hårdkodad for now, måste göra droplist eller om vi baserar det på ålder
                CM.p.memberType_ID = 1;
                
                
                CM.genderItems = gender;
                CM.p.gender_ID = CM.p.gender_ID;
                databas.Entry(CM.p).State = EntityState.Modified;
                databas.SaveChanges();

                string name = CM.p.firstName + " " + CM.p.lastName;

                TempData["editP"] = name;


                return RedirectToAction("Index");
            }
            return View(CM);
        }



        public ActionResult Delete(int id = 0)
        {
            Person person = databas.Person.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }

            //foreach (var item in databas.Gender)
            //{
            //    if (person.gender_ID == item.Id)
            //    {
            //        person.Gender = item;
            //    }
            //}
            return View(person);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = databas.Person.Find(id);

            int gId = -1;

            foreach (var item in databas.Golfer)
            {
                if (item.Person_ID == id)
                {
                    gId = item.Id;
                }
            }

            Golfer g = databas.Golfer.Find(gId);


            foreach (var item in databas.CompetitionGolfer)
            {
                if (g.Id == item.Golfer_ID)
                {
                    databas.CompetitionGolfer.Remove(item);
                }
            }
            foreach (var item in databas.TeeTimeDateGolfer)
            {
                if (g.Id == item.Golfer_ID)
                {
                    databas.TeeTimeDateGolfer.Remove(item);
                }
            }


            string name = person.firstName + " " + person.lastName;

            TempData["deletedP"] = name;



            databas.Golfer.Remove(g);

            databas.Person.Remove(person);

            databas.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            databas.Dispose();
            base.Dispose(disposing);
        }

        public bool IsAdmin(string id)
        {
            using (dsuteam4Entities1 databas = new dsuteam4Entities1())
            {

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
        
    }
}