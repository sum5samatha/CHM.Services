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
    
    public partial class Users_Roles 
    {
        public System.Guid ID { get; set; }
        public System.Guid UserID { get; set; }
        public System.Guid RoleID { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
