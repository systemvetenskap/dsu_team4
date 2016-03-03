using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using golf.Models;
using System.Web.Mvc;

namespace golf.Models
{
    public class CreateMember
    {
        public Person p { get; set; }
        public List<Gender> GenderL = new List<Gender>();
        //public SelectListItem GList = new SelectListItem
        //{

        //};
    }
}