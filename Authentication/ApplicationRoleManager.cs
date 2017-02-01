using CHM.Services.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHM.Services.Authentication
{
    public class ApplicationRoleManager : RoleManager<Role>
    {
        public ApplicationRoleManager(IRoleStore<Role, string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var appRoleManager = new ApplicationRoleManager(new RoleStoreService());

            return appRoleManager;
        }
    }
}   