using CHM.Services.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHM.Services.Authentication
{
    public class CustomUserManager : UserManager<User>
    {
        public CustomUserManager(IUserStore<User> store)
            : base(store)
        {
        }
    }
}