﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace golf.Models
{
    public class resultClass
    {
        public PersonGolfer currentPerson { get; set; }
        public IList<HoleStats> holeresult { get; set; }
        public Competition comp { get; set; }

    }
}