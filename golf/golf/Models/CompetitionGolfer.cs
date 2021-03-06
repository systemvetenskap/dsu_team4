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
    
    public partial class CompetitionGolfer
    {
        public CompetitionGolfer()
        {
            this.HoleStats = new HashSet<HoleStats>();
            this.MobileStats = new HashSet<MobileStats>();
        }
    
        public int Id { get; set; }
        public int Competition_ID { get; set; }
        public int Golfer_ID { get; set; }
        public string startTime { get; set; }
        public Nullable<int> net { get; set; }
        public Nullable<int> points { get; set; }
    
        public virtual Competition Competition { get; set; }
        public virtual Golfer Golfer { get; set; }
        public virtual ICollection<HoleStats> HoleStats { get; set; }
        public virtual ICollection<MobileStats> MobileStats { get; set; }
    }
}
