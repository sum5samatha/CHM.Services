using CHM.Services.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHM.Services.Authentication
{
    public class CustomSignInManager : SignInManager<User, string>
    {
        public CustomSignInManager(CustomUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }
    }
}