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
    
    public partial class MemberType
    {
        public MemberType()
        {
            this.Person = new HashSet<Person>();
        }
    
        public int Id { get; set; }
        public string name { get; set; }
        public Nullable<int> memberFee { get; set; }
        public Nullable<int> cleaningFee { get; set; }
    
        public virtual ICollection<Person> Person { get; set; }
    }
}
