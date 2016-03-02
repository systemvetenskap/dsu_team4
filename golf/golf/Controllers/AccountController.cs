﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using golf.Models;
using System.Collections;

namespace golf.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        private dsuteam4Entities1 db = new dsuteam4Entities1();
        //public ActionResult Egister()
        //{
        //    ViewBag.Message = "Registrera dig här för att bli medlem";
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult RegisterNew(Person model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Person.Add(model);
        //        db.SaveChanges();
        //        return RedirectToAction("MyPage");
        //    }

        //    return View(model);
        //}
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Person model)
        {

            if (ModelState.IsValid)
            {
                db.Person.Add(model);
                db.SaveChanges();



                FormsAuthentication.SetAuthCookie(model.Id.ToString(), false);

                return RedirectToAction("MyPage", model);
            }
            else
            {
                return View(model);
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

            return RedirectToAction("Login");
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
        //public ActionResult MyPage(Person P)
        //{

        //    int id = Convert.ToInt32(User.Identity.Name);

        //    Person person = db.Person.Find(id);

        //    //List<Person> pr = new List<Person>();
        //    //pr.Add(person);


        //    return View(P);
        //}


        public ActionResult LogInCheck(Person model)
        {

            String email = model.email;
            String PW = model.PW;
            foreach (Person P in db.Person)
            {
                if (P.email == email && P.PW == PW)
                {
                    FormsAuthentication.SetAuthCookie(P.Id.ToString(), false);

                    //List<Person> tempP = new List<Person>();

                    ////ICollection ic = new ICollection
                    //Golfer g = new Golfer();

                    //foreach (Golfer golf in db.Golfer)
                    //{
                    //    if (golf.Person_ID == P.Id)
                    //    {
                    //        P.Golfer.Add(g);
                    //    }
                    //}


                    //tempP.Add(P);


                    return RedirectToAction("MyPage");
                }
            }
            return View("~/Shared/Error.cshtml");
        }
    }
}
