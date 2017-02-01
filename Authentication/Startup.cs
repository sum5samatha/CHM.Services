using CHM.Services.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

[assembly: OwinStartup(typeof(CHM.Services.Authentication.Startup))]
namespace CHM.Services.Authentication
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            //       UserManagerFactory = () =>
            //new UserManager<AppUser>(new UserStoreService()) { PasswordHasher = new MyPasswordHasher() };

            //RoleManagerFactory = () => new ApplicationRoleManager(new RoleStoreService());
            ConfigureOAuth(app);
            WebApiConfig.Register(config);
            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                //TokenEndpointPath = new PathString("/CHMLadymead/token"),
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),                
                Provider = new SimpleAuthorizationServerProvider()
            };



            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
    }
}