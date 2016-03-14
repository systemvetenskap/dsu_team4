using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using golf.Models;
namespace golf.Models
{
    public class searchClass
    {
        List<PersonGolfer> persongolfer = new List<PersonGolfer>();
        List<Person> person = new List<Person>();
        public string searchstring {get; set;}

        public List<Person> getPersons(string s)
        {
            using(dsuteam4Entities1 databas = new dsuteam4Entities1())
            {

                var pers = databas.Person.Where(p => p.firstName.ToLower().Contains(s.ToLower()) || p.lastName.ToLower().Contains(s.ToLower()));
                foreach(var i in pers)
                {

                    person.Add(i);

                }

                return person;
            }

        }
    }
}