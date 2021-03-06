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
    
    public partial class Resident 
    {
        public Resident()
        {
            this.Actions = new HashSet<Action>();
            this.Interventions_Resident_Answers = new HashSet<Interventions_Resident_Answers>();
            this.Resident_Interventions_Questions_Answers = new HashSet<Resident_Interventions_Questions_Answers>();
            this.Residents_Questions_Answers = new HashSet<Residents_Questions_Answers>();
            this.Residents_Relatives = new HashSet<Residents_Relatives>();
            this.PainMonitorings = new HashSet<PainMonitoring>();
        }
    
        public System.Guid ID { get; set; }
        public System.Guid OrganizationID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string Gender { get; set; }
        public System.DateTime DOB { get; set; }
        public Nullable<System.DateTime> DOJ { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string GPDetails { get; set; }
        public string Nok { get; set; }
        public string NokTelephoneNumber { get; set; }
        public string NokAddress { get; set; }
        public string NokPreferred { get; set; }
        public string SocialWorker { get; set; }
        public string ReasonForAdmission { get; set; }
        public Nullable<bool> IsAccepted { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime Created { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime Modified { get; set; }
        public System.Guid ModifiedBy { get; set; }
        public Nullable<System.DateTime> AdmittedFrom { get; set; }
        public string NHS { get; set; }
        public string MedicalHistory { get; set; }
        public Nullable<System.DateTime> LeavingDate { get; set; }
        public string ReasonForLeaving { get; set; }
    
        public virtual ICollection<Action> Actions { get; set; }
        public virtual ICollection<Interventions_Resident_Answers> Interventions_Resident_Answers { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual ICollection<Resident_Interventions_Questions_Answers> Resident_Interventions_Questions_Answers { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual ICollection<Residents_Questions_Answers> Residents_Questions_Answers { get; set; }
        public virtual ICollection<Residents_Relatives> Residents_Relatives { get; set; }
        public virtual ICollection<PainMonitoring> PainMonitorings { get; set; }
    }
}
