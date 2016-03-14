using System;
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
            
            using(dsuteam4Entities1 databas = new dsuteam4Entities1())
            {
                searchClass sp = new searchClass();
                
                


                return View();
            }
            
        
        }
        public searchMember(string searchstring)
        {

        }

        //Visa detaljer om personer
        public ActionResult Details(int id = 0)
        {
            
            Person person = databas.Person.Find(id);
            Golfer golf = databas.Golfer.Find(id);


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
                CM.p.gender_ID = CM.genderid;
                databas.Entry(CM.p).State = EntityState.Modified;
                databas.SaveChanges();
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
            return View(person);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = databas.Person.Find(id);
            databas.Person.Remove(person);
            databas.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            databas.Dispose();
            base.Dispose(disposing);
        }
    }
}