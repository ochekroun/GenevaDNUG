using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Owin.Tutorials.HelloOwin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseHandlerAsync((req, res) =>
            {
                res.ContentType = "text/plain";
                return res.WriteAsync("Hello World!");
            });
        }

    }
}