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
    
    public partial class Intervention_Question_ParentQuestion 
    {
        public System.Guid ID { get; set; }
        public System.Guid InterventionQuestionID { get; set; }
        public System.Guid InterventionParentQuestionID { get; set; }
        public System.Guid InterventionParentAnswerID { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime Created { get; set; }
        public System.Guid CreatedBy { get; set; }
        public Nullable<System.DateTime> Modified { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
    
        public virtual Intervention_Question Intervention_Question { get; set; }
        public virtual Intervention_Question Intervention_Question1 { get; set; }
        public virtual Intervention_Question_Answer Intervention_Question_Answer { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}