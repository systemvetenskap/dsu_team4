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

        public ActionResult Medlemshantering()
        {
            ViewBag.Message = "Medlemshantering";

            return View();
        }

        public ActionResult Statistik()
        {
            ViewBag.Message = "Statitik";

            return View();
        }
        public ActionResult Tavlingar()
        {
            ViewBag.Message = "Tävlingar";

            return View();
        }
        public ActionResult Tidsbokning()
        {
            ViewBag.Message = "Tidsbokningar";

            return View();
        }
    }
}
