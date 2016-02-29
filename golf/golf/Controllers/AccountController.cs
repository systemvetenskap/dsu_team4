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
            ViewBag.Message = "Egister Här kan du regga dig om du vill det och tycker att det är kul att reggas på en hemisdan på internet där det kan bokas tider för golf, regga dig";
            return View();
        }

    }
}
