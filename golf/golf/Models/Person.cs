//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace golf.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Person
    {
        public Person()
        {
            this.AdminPerson = new HashSet<AdminPerson>();
            this.Golfer = new HashSet<Golfer>();
        }
    
        public int Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string streetAddres { get; set; }
        public string postalCode { get; set; }
        public string city { get; set; }
        public string email { get; set; }
        public Nullable<int> gender_ID { get; set; }
        public Nullable<int> memberType_ID { get; set; }
        public string PW { get; set; }
    
        public virtual ICollection<AdminPerson> AdminPerson { get; set; }
        public virtual ICollection<Golfer> Golfer { get; set; }
    }
}
