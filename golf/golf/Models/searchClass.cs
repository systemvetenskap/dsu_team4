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
               
                List<OneNamePerson> op = new List<OneNamePerson>();
                foreach (Person i in databas.Person.ToList())
                {
                    OneNamePerson onp = new OneNamePerson();
                    onp.Id = i.Id;
                    onp.oneName = i.firstName + " " + i.lastName;
                    op.Add(onp);
                }

                var pers = op.Where(p => p.oneName.ToLower().Contains(s.ToLower()));
           
                foreach(var i in pers)
                {

                    var selp = databas.Person.Find(i.Id);

                    person.Add(selp);
                    
                }

                return person;
            }

        }

        public List<Person> getPersonGolfers(List<PersonGolfer> pg, string s)
        {
            using (dsuteam4Entities1 databas = new dsuteam4Entities1())
            {

                List<OneNamePerson> op = new List<OneNamePerson>();
                foreach (PersonGolfer i in pg)
                {
                    OneNamePerson onp = new OneNamePerson();
                    onp.Id = i.personid;
                    onp.oneName = i.firstName + " " + i.lastName;
                    op.Add(onp);

                    

                }

                var pers = op.Where(p => p.oneName.ToLower().Contains(s.ToLower()));

                foreach (var i in pers)
                {
                    var selP = databas.PersonGolfer.Find(i.gol);

                    person.Add(selP);

                }

                return person;
            }

        }

    }
}