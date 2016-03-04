using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using golf.Models;

namespace golf.Controllers
{
    public class HomeController : Controller
    {
        private dsuteam4Entities1 db = new dsuteam4Entities1();
        public ActionResult Index()
        {
            NewsClass na = new NewsClass();

            foreach (NewsArticle article in db.NewsArticle)
            {
                na.newarticle.Add(article);
            }

            //Nya nyheter först
            na.newarticle.Reverse();

            return View(na);
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
