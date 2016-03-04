﻿using System;
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

           
            CM.genderItems = gender;

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
                db.Person.Add(model.p);
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

            return View(person);
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

                    foreach (var item in db.AdminPerson)
                    {
                        if (item.Person_ID == P.Id)
                        {
                            
                        }
                    }

                    return RedirectToAction("MyPage");
                }
            }
            return View("~/Shared/Error.cshtml");
        }
    }
}
