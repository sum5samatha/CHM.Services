using CHM.Services.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace CHM.Services.Authentication
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {


            try
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });


                using (AuthRepository _repo = new AuthRepository())
                {
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);


                    IList<string> lstRoles;
                    //User user = await _repo.FindUser(context.UserName, context.Password);
                    User user = await _repo.FindUserByEmailAndPassword(context.UserName, context.Password);
                    user.Users_Roles = user.Users_Roles.Where(i => i.IsActive == true).ToList<Users_Roles>();

                    if (user == null)
                    {
                        context.SetError("invalid_grant", "The user name or password is incorrect.");
                        return;
                    }


                    lstRoles = _repo.GetUserRoles(user);
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.ID)));
                    //identity.AddClaim(new Claim("OrganizationIDs", JsonConvert.SerializeObject(user.Users_Organizations.Select(obj=>obj.OrganizationID).ToList<Guid>())));
                    identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                    if (lstRoles != null)
                    {
                        foreach (string role in lstRoles)
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Role, role));
                        }
                    }

                    var props = new AuthenticationProperties(new Dictionary<string, string> { { "name", user.FirstName }, { "organizationsIds", JsonConvert.SerializeObject(user.Users_Organizations.Select(obj => obj.OrganizationID).ToList<Guid>()) }, { "Role", user.Users_Roles.Select(obj => obj.Role.Name).FirstOrDefault() }, { "UserID", user.ID.ToString() }, { "LastLogin", user.LastLogin != null ? user.LastLogin.Value.Ticks.ToString() : string.Empty } });
                    var ticket = new AuthenticationTicket(identity, props);
                    context.Validated(ticket);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }



        }


    }
}