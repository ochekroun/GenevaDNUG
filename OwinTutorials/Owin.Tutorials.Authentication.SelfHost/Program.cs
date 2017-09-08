using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Hosting;

namespace Owin.Tutorials.Authentication.SelfHost
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use(typeof(AuthenticationMiddleware));

            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultWebApi", "{controller}/{id}", new { id = RouteParameter.Optional });

            app.UseWebApi(config);

        }
    }

    [Authorize]
    public class ValuesController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse<string>("Hello, " + User.Identity.Name);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            const string baseUrl = "http://localhost:12345/";

            using (WebApp.Start<Startup>(new StartOptions(baseUrl)))
            {
                Console.WriteLine("Press Enter to terminate.");
                Console.Read();
            }
        }
    }
}
