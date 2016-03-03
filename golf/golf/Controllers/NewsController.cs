using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using golf.Models;

namespace golf.Controllers
{
    public class NewsController : Controller
    {
        //
        // GET: /News/
        private dsuteam4Entities1 db = new dsuteam4Entities1();
        public ActionResult Index()
        {
            NewsClass na = new NewsClass();

            foreach (NewsArticle article in db.NewsArticle)
            {
                na.newarticle.Add(article);
            }
            return View(na);
        }

        public ActionResult AddNews()
        {
            return View();
        }

    }
}
