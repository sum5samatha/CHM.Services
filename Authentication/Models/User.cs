using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace CHM.Services.Models
{
    public partial class User : IUser
    {
        string _id = Guid.NewGuid().ToString();

        public virtual string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User,string> manager, string authenticationType)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //    var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
        //    // Add custom user claims here
        //    return userIdentity;
        //}
    }
}