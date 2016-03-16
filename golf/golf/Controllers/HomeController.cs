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
            
            na.newarticle.Reverse();

            //if (Request.IsAuthenticated)
            //{
            //        using(dsuteam4Entities1 db2 = new dsuteam4Entities1())
            //        {
            //            foreach (var item in db2.AdminPerson)
            //            {
            //                if (User.Identity.Name == item.Person_ID.ToString())
            //                {
            //                    na.logedPerson = db2.Person.Find(item.Person_ID);
            //                }
            //                else
            //                {

            //                }
            //            }
            //        }
                
            //}


            return View(na);
        }

        // can be HttpGet
        public ActionResult admin(string id)
        {
            bool isValid = false;

            using (dsuteam4Entities1 db = new dsuteam4Entities1())
            {
                if (Request.IsAuthenticated)
                {
                   
                        foreach (var item in db.AdminPerson)
                        {
                            if (User.Identity.Name == item.Person_ID.ToString())
                            {
                                isValid = true;   
                            }                      
                       }
                    

                }
            }



            var obj = new
            {
                valid = isValid
            };
            return Json(obj);

            //return Json(
            //new { isCalculateTax = isValid },
            //JsonRequestBehavior.AllowGet
            //);

        }
        public ActionResult AddNews()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNews(NewsClass news)
        {
            if (ModelState.IsValid)
            {
                NewsArticle newA = new NewsArticle();
                Images newI = new Images();

                newA.header = news.NA.header;
                newA.content = news.NA.content;
                newA.newsDate = DateTime.Today.Date;
                newA.Person_ID = Convert.ToInt32(User.Identity.Name);
                db.NewsArticle.Add(newA);
                db.SaveChanges();
                if (news.bild.imgURL!=null)
                {
                    NewsArticleImage nai = new NewsArticleImage();
                    newI.imgURL = news.bild.imgURL;
                    db.Images.Add(newI);
                    db.SaveChanges();
                    nai.Images_ID = newI.Id;
                    nai.NewsArticle_ID = newA.Id;
                    db.NewsArticleImage.Add(nai);
                    db.SaveChanges();
                }                                      
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
