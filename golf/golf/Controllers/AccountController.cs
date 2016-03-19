using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using golf.Models;
using System.Collections;
using System.Data.Entity;
using System.Data;
using System.Web.UI.WebControls;

namespace golf.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        private dsuteam4Entities1 db = new dsuteam4Entities1();


        public ActionResult Create()
        {
            CreateMember CM = new CreateMember();
            SelectList gender = new SelectList(db.Gender.ToList(), "id","genderName");
            SelectList membertype = new SelectList(db.MemberType.ToList(), "id", "name");
            
            
            CM.genderItems = gender;
            CM.memberType = membertype;


            return View(CM);
        }

        public ActionResult Index()
        {
            return RedirectToAction("MyPage");
        }


        [HttpPost]
        public ActionResult Create(CreateMember model)
        {

            if (ModelState.IsValid)
            {
  
                model.p.gender_ID = model.genderid;
                model.p.memberType_ID = model.memberID;
                model.p.Payed = false;

               
                db.Person.Add(model.p);
                db.SaveChanges();

                Golfer g = new Golfer();
                g.Person_ID = model.p.Id;
                g.HCP = "32";
                g.golfID = "12035_" + model.p.Id;

                db.Golfer.Add(g);
                db.SaveChanges();

                


                FormsAuthentication.SetAuthCookie(model.p.Id.ToString(), false);

                return RedirectToAction("MyPage", model.p);
            }
            else
            {
                return View(model.p);
            }
        }


        public ActionResult Login()
        {
            ViewBag.Message = "Logga in här";
            return View();
        }
        public ActionResult Index2()
        {
            return View();
        }
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return Redirect(Url.Content("~/"));
        }

        public ActionResult Submit()
        {
            // do something
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult MyPage()
        {

            int id = Convert.ToInt32(User.Identity.Name);

            Person person = db.Person.Find(id);
            ViewBag.User = person.firstName +" "+ person.lastName;
            //Komentar
            CreateMember cm = new CreateMember();
            cm.p = person;

            foreach (var item in db.Golfer)
            {
                if (person.Id == item.Person_ID)
                {
                    cm.golfID = item.golfID;
                }
            }


            return View(cm);
        }

        public ActionResult Edit(int id = 0)
        {
            Person person = db.Person.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                
                db.SaveChanges();
                

                return RedirectToAction("Index");
            }
            return View(person);
        }

       
        public ActionResult LogInCheck(Person model)
        {
            String email = model.email;
            String PW = model.PW;
            foreach (Person P in db.Person)
            {
                if (P.email == email && P.PW == PW)
                {
                    FormsAuthentication.SetAuthCookie(P.Id.ToString(), false);

                    //if (P.Id == 911)
                    //{
                    //    Roles.CreateRole("Admin");
                    //    Roles.AddUserToRole(P.Id.ToString(), "Admin");
                    //}
                    //else
                    //{
                    //    Roles.CreateRole("User");
                    //    Roles.AddUserToRole(P.Id.ToString(), "User");

                    //}
                    //var roles = new List<string> { "Admin", "Author", "Super" };
                    //var userRoles = Roles.GetRolesForUser(User.Identity.Name);

                    //if (userRoles.Any(u => roles.Contains(u)))
                    //{
                    //    //do something
                    //}
                    foreach (var Pa in db.AdminPerson)
                    {


                        if (P.Id == Pa.Person_ID)
                        {
                            return RedirectToAction("Index", "TeeTime");
                        }
                        else
                        {
                            return RedirectToAction("MyPage");
                        }
                    }
                   
                }
            }
      
            return View("LoginFail");
        }
    }
}
