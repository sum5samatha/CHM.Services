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
    
    public partial class Sections_Questions_Answers_Widget 
    {
        public System.Guid ID { get; set; }
        public System.Guid Section_QuestionID { get; set; }
        public System.Guid Section_Question_AnswerID { get; set; }
        public string Widget { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> Modified { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
    
        public virtual Sections_Questions Sections_Questions { get; set; }
        public virtual Sections_Questions_Answers Sections_Questions_Answers { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
