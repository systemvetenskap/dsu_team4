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
    using Newtonsoft.Json;
    
    public partial class TeeTime
    {
        public TeeTime()
        {
            this.TeeTimeDate = new HashSet<TeeTimeDate>();
        }
    
        public int Id { get; set; }
        public string teeTime1 { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<TeeTimeDate> TeeTimeDate { get; set; }
    }
}
