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
    
    public partial class AdminType
    {
        public AdminType()
        {
            this.AdminPerson = new HashSet<AdminPerson>();
        }
    
        public int Id { get; set; }
        public string type { get; set; }
    
        public virtual ICollection<AdminPerson> AdminPerson { get; set; }
    }
}
