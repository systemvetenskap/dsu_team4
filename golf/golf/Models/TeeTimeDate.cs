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
    
    public partial class TeeTimeDate
    {
        public TeeTimeDate()
        {
            this.TeeTimeDateGolfer = new HashSet<TeeTimeDateGolfer>();
        }
    
        public int Id { get; set; }
        public int TeeTime_ID { get; set; }
        public Nullable<bool> Disabled { get; set; }
        public System.DateTime bookingDate { get; set; }
    
        public virtual TeeTime TeeTime { get; set; }
        public virtual ICollection<TeeTimeDateGolfer> TeeTimeDateGolfer { get; set; }
    }
}
