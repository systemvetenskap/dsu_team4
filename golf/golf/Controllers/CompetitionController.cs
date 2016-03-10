using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using golf.Models;


namespace golf.Controllers
{
    public class CompetitionController : Controller
    {
        //
        // GET: /Competition/

        public ActionResult Index()
        {
           CreateComp cc = new CreateComp();

           cc.currentDate = DateTime.Today;
            using (dsuteam4Entities1 databas = new dsuteam4Entities1())
            {

                

                cc.classList = databas.CompeteClass.ToList();
                List<Person> p = databas.Person.ToList();
                List<OneNamePerson> op = new List<OneNamePerson>();
                foreach (Person i in p)
                {
                    OneNamePerson onp = new OneNamePerson();
                    onp.Id            =  i.Id;           
                    onp.oneName       =  i.firstName + " " + i.lastName;      
                    onp.firstName     =  i.firstName;    
                    onp.lastName      =  i.lastName;     
                    onp.streetAddres  =  i.streetAddres ;
                    onp.postalCode    =  i.postalCode;  
                    onp.city          =  i.city;
                    onp.email         =  i.email;  
                    onp.gender_ID     =  i.gender_ID;
                    onp.memberType_ID =  i.memberType_ID;
                    op.Add(onp);
                }
                cc.contactlist = op.OrderBy(x => x.oneName);
                

                //cc.contactlist = databas.Person.ToList();

            }
            

            return View(cc);
        }
        public PartialViewResult saveComp(CreateComp cc)
        {
            using( dsuteam4Entities1 databas = new dsuteam4Entities1())
            {
                var comp = cc.newComp;

                //HÄR HÅRDKODAS DET------->
                //comp.Person_IDc = 25;
                //comp.CompeteClass_ID = 1;
                //HÄR HÅRDKODAS DET-------<
            
                databas.Competition.Add(comp); 
                databas.SaveChanges();
                
            }
            using( dsuteam4Entities1 ndatabas = new dsuteam4Entities1())
            {
                CreateComp c = new CreateComp();

                c.complist = ndatabas.Competition.ToList();
                c.currentDate = DateTime.Today;

                return PartialView("_sComp", c);
            }

        }
        public void getData()
        {

        }

    }
}
