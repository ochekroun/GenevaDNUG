using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Owin.Tutorials.WebApi.Logic;

namespace Owin.Tutorials.WebApi.Tests
{
    class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);
        } 
    }
}
