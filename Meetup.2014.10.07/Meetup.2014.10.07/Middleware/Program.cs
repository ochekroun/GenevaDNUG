using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Middleware
{
    public class LogMiddleware : OwinMiddleware
    {
        public LogMiddleware(OwinMiddleware next)
            : base(next)
        {

        }

        public async override Task Invoke(IOwinContext context)
        {
            Console.WriteLine("Request begins: {0} {1}", context.Request.Method, context.Request.Uri);
            await Next.Invoke(context);
            Console.WriteLine("Request ends : {0} {1}", context.Request.Method, context.Request.Uri);
        }
    }


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

            appBuilder.Use(typeof(AuthenticationMiddleware));
            appBuilder.Use(typeof(LogMiddleware));
            appBuilder.UseWebApi(config);
            
        }
    }


    public class ValuesController : ApiController
    {
        // GET api/values 
        [Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5 
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values 
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5 
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
        }
    }

    class Program
    {
        static void Main()
        {
            string baseAddress = "http://localhost:12345/";

            // Start OWIN host 
            WebApp.Start<Startup>(url: baseAddress);

            Console.ReadLine();
        }
    }

    public class AuthenticationMiddleware : OwinMiddleware
    {
        public AuthenticationMiddleware(OwinMiddleware next) :
            base(next) { }

        public override async Task Invoke(IOwinContext context)
        {
            var response = context.Response;
            var request = context.Request;

            response.OnSendingHeaders(state =>
            {
                var resp = (OwinResponse)state;

                if (resp.StatusCode == 401)
                {
                    resp.Headers.Add("WWW-Authenticate", new[] { "Basic" });
                    //resp.StatusCode = 403;
                    //resp.ReasonPhrase = "Forbidden";
                }
            }, response);

            var header = request.Headers["Authorization"];

            if (!String.IsNullOrWhiteSpace(header))
            {
                var authHeader = System.Net.Http.Headers.AuthenticationHeaderValue.Parse(header);

                if ("Basic".Equals(authHeader.Scheme, StringComparison.OrdinalIgnoreCase))
                {
                    string parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter));
                    var parts = parameter.Split(':');

                    string userName = parts[0];
                    string password = parts[1];

                    if (ValidateUser(userName, password)) // Just a dumb check
                    {
                        var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, userName)
                    };
                        var identity = new ClaimsIdentity(claims, "Basic");

                        request.User = new ClaimsPrincipal(identity);
                    }
                }
            }

            await Next.Invoke(context);
        }

        private bool ValidateUser(string username, string password)
        {
            // do actual password validation here
            return username != password;
        }
    }

}
