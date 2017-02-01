using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace CHM.Services.Authentication.Extensions
{
    public static class IdentityExtensions
    {
        public static Guid GetFarmID(this IIdentity identity)
        {
            string strFarmId = ((ClaimsIdentity)identity).FindFirst("FarmID").Value;

            return string.IsNullOrEmpty(strFarmId) ? Guid.Empty : new Guid(strFarmId);
        }
    }
}