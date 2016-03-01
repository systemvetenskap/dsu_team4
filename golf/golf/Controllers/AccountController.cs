using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using golf.Models;

namespace golf.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        private dsuteam4Entities1 db = new dsuteam4Entities1();
        public ActionResult Egister()
        {
            ViewBag.Message = "Egister ig är ör att li edlem";
            return View();
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
            return View();
        }
        public ActionResult LogInCheck(Person model)
        {

            String email = model.email;
            String PW = model.PW;
            foreach (Person P in db.Person)
            {
                if (P.email == email && P.PW == PW)
                {
                    FormsAuthentication.SetAuthCookie(model.email, true);
                    return View("~/Views/Home/Index.cshtml");
                }
            }
            return View("~/Views/Log/Index.cshtml");
        }
    }
}
