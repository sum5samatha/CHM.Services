using CHM.Services.ExceptionHandling;
using CHM.Services.Utils;
using Elmah.Contrib.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Routing;

namespace CHM.Services
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            json.SerializerSettings.MaxDepth = 5; 
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            json.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.EnableCors();

            RouteTable.Routes.Ignore("{resource}.axd/{*pathInfo}");
            config.Routes.IgnoreRoute("ELMAH", "{resource}.axd/{*pathInfo}");

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
               routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
               // routeTemplate: "api/{controller}/{Password}",
             //   defaults: new { Password = RouteParameter.Optional}
            ); 


            //Used to log 404 errors to ELMAH
            config.Routes.MapHttpRoute(
                name: "NotFound",
                routeTemplate: "{*path}",
                defaults: new { controller = "Error", action = "NotFound" }
            );


            config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());
            config.Filters.Add(new UnhandledExceptionFilter());
        }
    }
}
