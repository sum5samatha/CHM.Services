//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CHM.Services.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Organization 
    {
        public Organization()
        {
            this.OrganizationGroups_Organizations = new HashSet<OrganizationGroups_Organizations>();
            this.Residents = new HashSet<Resident>();
            this.Sections_Organizations = new HashSet<Sections_Organizations>();
            this.PainMonitorings = new HashSet<PainMonitoring>();
            this.Configurations = new HashSet<Configuration>();
        }
    
        public System.Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime Created { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime Modified { get; set; }
        public System.Guid ModifiedBy { get; set; }
    
        public virtual ICollection<OrganizationGroups_Organizations> OrganizationGroups_Organizations { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual ICollection<Resident> Residents { get; set; }
        public virtual ICollection<Sections_Organizations> Sections_Organizations { get; set; }
        public virtual ICollection<PainMonitoring> PainMonitorings { get; set; }
        public virtual ICollection<Configuration> Configurations { get; set; }
    }
}