using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace golf.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        
        public ActionResult Egister()
        {
            ViewBag.Message = "Egister g är ör att li edlem";
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
    }
}
