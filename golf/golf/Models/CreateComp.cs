using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace golf.Models
{
    public class CreateComp
    {
        public List<Competition> complist = new List<Competition>();
        public Competition newComp { get; set; }
    }
}