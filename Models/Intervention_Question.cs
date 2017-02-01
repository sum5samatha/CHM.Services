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
    
    public partial class Intervention_Question 
    {
        public Intervention_Question()
        {
            this.Intervention_Question_Answer = new HashSet<Intervention_Question_Answer>();
            this.Intervention_Question_Answer_Task = new HashSet<Intervention_Question_Answer_Task>();
            this.Intervention_Question_ParentQuestion = new HashSet<Intervention_Question_ParentQuestion>();
            this.Intervention_Question_ParentQuestion1 = new HashSet<Intervention_Question_ParentQuestion>();
            this.Interventions_Question_Answer_Summary = new HashSet<Interventions_Question_Answer_Summary>();
        }
    
        public System.Guid ID { get; set; }
        public System.Guid Section_InterventionID { get; set; }
        public string Question { get; set; }
        public string AnswerType { get; set; }
        public Nullable<int> MinScore { get; set; }
        public Nullable<int> MaxScore { get; set; }
        public Nullable<int> Score { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public Nullable<bool> IsInAssessment { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime Created { get; set; }
        public System.Guid CreatedBy { get; set; }
        public Nullable<System.DateTime> Modified { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
    
        public virtual ICollection<Intervention_Question_Answer> Intervention_Question_Answer { get; set; }
        public virtual ICollection<Intervention_Question_Answer_Task> Intervention_Question_Answer_Task { get; set; }
        public virtual ICollection<Intervention_Question_ParentQuestion> Intervention_Question_ParentQuestion { get; set; }
        public virtual ICollection<Intervention_Question_ParentQuestion> Intervention_Question_ParentQuestion1 { get; set; }
        public virtual Section_Intervention Section_Intervention { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual ICollection<Interventions_Question_Answer_Summary> Interventions_Question_Answer_Summary { get; set; }
    }
}