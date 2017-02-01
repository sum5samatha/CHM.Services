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
    
    public partial class User 
    {
        public User()
        {
            this.Actions = new HashSet<Action>();
            this.Actions1 = new HashSet<Action>();
            this.Actions_Days = new HashSet<Actions_Days>();
            this.Actions_Days1 = new HashSet<Actions_Days>();
            this.Intervention_Question = new HashSet<Intervention_Question>();
            this.Intervention_Question1 = new HashSet<Intervention_Question>();
            this.Intervention_Question_Answer = new HashSet<Intervention_Question_Answer>();
            this.Intervention_Question_Answer1 = new HashSet<Intervention_Question_Answer>();
            this.Intervention_Question_Answer_Task = new HashSet<Intervention_Question_Answer_Task>();
            this.Intervention_Question_Answer_Task1 = new HashSet<Intervention_Question_Answer_Task>();
            this.Intervention_Question_ParentQuestion = new HashSet<Intervention_Question_ParentQuestion>();
            this.Intervention_Question_ParentQuestion1 = new HashSet<Intervention_Question_ParentQuestion>();
            this.Interventions = new HashSet<Intervention>();
            this.Interventions1 = new HashSet<Intervention>();
            this.Interventions_Question_Answer_Summary = new HashSet<Interventions_Question_Answer_Summary>();
            this.Interventions_Question_Answer_Summary1 = new HashSet<Interventions_Question_Answer_Summary>();
            this.Interventions_Resident_Answers = new HashSet<Interventions_Resident_Answers>();
            this.Interventions_Resident_Answers1 = new HashSet<Interventions_Resident_Answers>();
            this.OrganizationGroups = new HashSet<OrganizationGroup>();
            this.OrganizationGroups1 = new HashSet<OrganizationGroup>();
            this.OrganizationGroups_Organizations = new HashSet<OrganizationGroups_Organizations>();
            this.OrganizationGroups_Organizations1 = new HashSet<OrganizationGroups_Organizations>();
            this.Organizations = new HashSet<Organization>();
            this.Organizations1 = new HashSet<Organization>();
            this.Question_ParentQuestion = new HashSet<Question_ParentQuestion>();
            this.Question_ParentQuestion1 = new HashSet<Question_ParentQuestion>();
            this.Resident_Interventions_Questions_Answers = new HashSet<Resident_Interventions_Questions_Answers>();
            this.Resident_Interventions_Questions_Answers1 = new HashSet<Resident_Interventions_Questions_Answers>();
            this.Residents = new HashSet<Resident>();
            this.Residents1 = new HashSet<Resident>();
            this.Residents_Questions_Answers = new HashSet<Residents_Questions_Answers>();
            this.Residents_Questions_Answers1 = new HashSet<Residents_Questions_Answers>();
            this.Residents_Relatives = new HashSet<Residents_Relatives>();
            this.Residents_Relatives1 = new HashSet<Residents_Relatives>();
            this.Residents_Relatives2 = new HashSet<Residents_Relatives>();
            this.Section_Intervention = new HashSet<Section_Intervention>();
            this.Section_Intervention1 = new HashSet<Section_Intervention>();
            this.Section_Intervention_Statements = new HashSet<Section_Intervention_Statements>();
            this.Section_Intervention_Statements1 = new HashSet<Section_Intervention_Statements>();
            this.Section_Summary = new HashSet<Section_Summary>();
            this.Section_Summary1 = new HashSet<Section_Summary>();
            this.Sections = new HashSet<Section>();
            this.Sections1 = new HashSet<Section>();
            this.Sections_Organizations = new HashSet<Sections_Organizations>();
            this.Sections_Organizations1 = new HashSet<Sections_Organizations>();
            this.Sections_Questions = new HashSet<Sections_Questions>();
            this.Sections_Questions1 = new HashSet<Sections_Questions>();
            this.Sections_Questions_Answers = new HashSet<Sections_Questions_Answers>();
            this.Sections_Questions_Answers1 = new HashSet<Sections_Questions_Answers>();
            this.Sections_Questions_Answers_Summary = new HashSet<Sections_Questions_Answers_Summary>();
            this.Sections_Questions_Answers_Summary1 = new HashSet<Sections_Questions_Answers_Summary>();
            this.Sections_Questions_Answers_Tasks = new HashSet<Sections_Questions_Answers_Tasks>();
            this.Sections_Questions_Answers_Tasks1 = new HashSet<Sections_Questions_Answers_Tasks>();
            this.Sections_Questions_Answers_Widget = new HashSet<Sections_Questions_Answers_Widget>();
            this.Sections_Questions_Answers_Widget1 = new HashSet<Sections_Questions_Answers_Widget>();
            this.Users1 = new HashSet<User>();
            this.Users11 = new HashSet<User>();
            this.Users_Organizations = new HashSet<Users_Organizations>();
            this.Users_Organizations1 = new HashSet<Users_Organizations>();
            this.Users_Roles = new HashSet<Users_Roles>();
            this.PainMonitorings = new HashSet<PainMonitoring>();
            this.PainMonitorings1 = new HashSet<PainMonitoring>();
        }
    
        public System.Guid ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string TelePhone { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Nullable<System.Guid> UserTypeID { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime Created { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public System.DateTime Modified { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
    
        public virtual ICollection<Action> Actions { get; set; }
        public virtual ICollection<Action> Actions1 { get; set; }
        public virtual ICollection<Actions_Days> Actions_Days { get; set; }
        public virtual ICollection<Actions_Days> Actions_Days1 { get; set; }
        public virtual ICollection<Intervention_Question> Intervention_Question { get; set; }
        public virtual ICollection<Intervention_Question> Intervention_Question1 { get; set; }
        public virtual ICollection<Intervention_Question_Answer> Intervention_Question_Answer { get; set; }
        public virtual ICollection<Intervention_Question_Answer> Intervention_Question_Answer1 { get; set; }
        public virtual ICollection<Intervention_Question_Answer_Task> Intervention_Question_Answer_Task { get; set; }
        public virtual ICollection<Intervention_Question_Answer_Task> Intervention_Question_Answer_Task1 { get; set; }
        public virtual ICollection<Intervention_Question_ParentQuestion> Intervention_Question_ParentQuestion { get; set; }
        public virtual ICollection<Intervention_Question_ParentQuestion> Intervention_Question_ParentQuestion1 { get; set; }
        public virtual ICollection<Intervention> Interventions { get; set; }
        public virtual ICollection<Intervention> Interventions1 { get; set; }
        public virtual ICollection<Interventions_Question_Answer_Summary> Interventions_Question_Answer_Summary { get; set; }
        public virtual ICollection<Interventions_Question_Answer_Summary> Interventions_Question_Answer_Summary1 { get; set; }
        public virtual ICollection<Interventions_Resident_Answers> Interventions_Resident_Answers { get; set; }
        public virtual ICollection<Interventions_Resident_Answers> Interventions_Resident_Answers1 { get; set; }
        public virtual ICollection<OrganizationGroup> OrganizationGroups { get; set; }
        public virtual ICollection<OrganizationGroup> OrganizationGroups1 { get; set; }
        public virtual ICollection<OrganizationGroups_Organizations> OrganizationGroups_Organizations { get; set; }
        public virtual ICollection<OrganizationGroups_Organizations> OrganizationGroups_Organizations1 { get; set; }
        public virtual ICollection<Organization> Organizations { get; set; }
        public virtual ICollection<Organization> Organizations1 { get; set; }
        public virtual ICollection<Question_ParentQuestion> Question_ParentQuestion { get; set; }
        public virtual ICollection<Question_ParentQuestion> Question_ParentQuestion1 { get; set; }
        public virtual ICollection<Resident_Interventions_Questions_Answers> Resident_Interventions_Questions_Answers { get; set; }
        public virtual ICollection<Resident_Interventions_Questions_Answers> Resident_Interventions_Questions_Answers1 { get; set; }
        public virtual ICollection<Resident> Residents { get; set; }
        public virtual ICollection<Resident> Residents1 { get; set; }
        public virtual ICollection<Residents_Questions_Answers> Residents_Questions_Answers { get; set; }
        public virtual ICollection<Residents_Questions_Answers> Residents_Questions_Answers1 { get; set; }
        public virtual ICollection<Residents_Relatives> Residents_Relatives { get; set; }
        public virtual ICollection<Residents_Relatives> Residents_Relatives1 { get; set; }
        public virtual ICollection<Residents_Relatives> Residents_Relatives2 { get; set; }
        public virtual ICollection<Section_Intervention> Section_Intervention { get; set; }
        public virtual ICollection<Section_Intervention> Section_Intervention1 { get; set; }
        public virtual ICollection<Section_Intervention_Statements> Section_Intervention_Statements { get; set; }
        public virtual ICollection<Section_Intervention_Statements> Section_Intervention_Statements1 { get; set; }
        public virtual ICollection<Section_Summary> Section_Summary { get; set; }
        public virtual ICollection<Section_Summary> Section_Summary1 { get; set; }
        public virtual ICollection<Section> Sections { get; set; }
        public virtual ICollection<Section> Sections1 { get; set; }
        public virtual ICollection<Sections_Organizations> Sections_Organizations { get; set; }
        public virtual ICollection<Sections_Organizations> Sections_Organizations1 { get; set; }
        public virtual ICollection<Sections_Questions> Sections_Questions { get; set; }
        public virtual ICollection<Sections_Questions> Sections_Questions1 { get; set; }
        public virtual ICollection<Sections_Questions_Answers> Sections_Questions_Answers { get; set; }
        public virtual ICollection<Sections_Questions_Answers> Sections_Questions_Answers1 { get; set; }
        public virtual ICollection<Sections_Questions_Answers_Summary> Sections_Questions_Answers_Summary { get; set; }
        public virtual ICollection<Sections_Questions_Answers_Summary> Sections_Questions_Answers_Summary1 { get; set; }
        public virtual ICollection<Sections_Questions_Answers_Tasks> Sections_Questions_Answers_Tasks { get; set; }
        public virtual ICollection<Sections_Questions_Answers_Tasks> Sections_Questions_Answers_Tasks1 { get; set; }
        public virtual ICollection<Sections_Questions_Answers_Widget> Sections_Questions_Answers_Widget { get; set; }
        public virtual ICollection<Sections_Questions_Answers_Widget> Sections_Questions_Answers_Widget1 { get; set; }
        public virtual ICollection<User> Users1 { get; set; }
        public virtual User User1 { get; set; }
        public virtual ICollection<User> Users11 { get; set; }
        public virtual User User2 { get; set; }
        public virtual ICollection<Users_Organizations> Users_Organizations { get; set; }
        public virtual ICollection<Users_Organizations> Users_Organizations1 { get; set; }
        public virtual ICollection<Users_Roles> Users_Roles { get; set; }
        public virtual UserType UserType { get; set; }
        public virtual ICollection<PainMonitoring> PainMonitorings { get; set; }
        public virtual ICollection<PainMonitoring> PainMonitorings1 { get; set; }
    }
}
