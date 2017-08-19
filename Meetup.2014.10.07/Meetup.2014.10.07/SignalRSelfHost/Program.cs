using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRSelfHost
{
    public class MyHub : Hub
    {
        public void Send(string message)
        {
            Console.WriteLine(message);
            Clients.All.sendMessage(message);
        }
    }

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }

    class Program
    {
        // just for demo purposes, will "beep" every 5 seconds
        private static Timer timer = new Timer(o =>
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            var message = string.Format(DateTime.Now.ToLongTimeString() + " beep");
            context.Clients.All.sendMessage(message);
        });

        static void Main(string[] args)
        {
            using (WebApp.Start("http://localhost:12345"))
            {
                // just for this demo
                TimeSpan ts = TimeSpan.FromSeconds(5);
                timer.Change(ts, ts);

                Console.WriteLine("Server is running, Press <Enter> to stop");
                Console.ReadLine();
            }
        }
    }
}
