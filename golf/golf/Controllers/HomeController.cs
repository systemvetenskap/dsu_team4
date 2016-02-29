using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace golf.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Startsida";
            return View();
        }
        public ActionResult MemberHandling()
        {
            ViewBag.Message = "Medlemshantering";
            return View();
        }
        public ActionResult Statistics()
        {
            ViewBag.Message = "Statistik";
            return View();
        }
        public ActionResult Competitions()
        {
            ViewBag.Message = "Tävlingar";
            return View();
        }
        public ActionResult TimeScheduling()
        {
            ViewBag.Message = "Tidsbokningar";
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
    }
}
