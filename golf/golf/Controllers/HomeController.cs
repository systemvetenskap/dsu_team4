using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using golf.Models;
using System.Web.Security;

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
            foreach (Images i in db.Images)
            {
                na.images.Add(i);
            }
            foreach (NewsArticleImage ai in db.NewsArticleImage)
            {
                na.ArticleImages.Add(ai);
            }
         
            //Nya nyheter först
            na.newarticle.Reverse();

            return View(na);
        }
        public ActionResult AddNews()
        {

            return View();
        }

        
        [HttpPost]
        public ActionResult AddNews(NewsArticle na)
        {
            if (ModelState.IsValid)
            {
                    na.Person_ID = Convert.ToInt32(User.Identity.Name);
                    na.newsDate = DateTime.Now.Date;
                    db.NewsArticle.Add(na);
                    db.SaveChanges();
                    return RedirectToAction("Index");
            }
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
