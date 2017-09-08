using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;

namespace Owin.Tutorials.SignalR.SelfHost
{
    class Program
    {
        // just for demo purposes, will "beep" every 5 seconds
        private static Timer timer = new Timer(o =>
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            context.Clients.All.sendMessage(DateTime.Now.ToLongTimeString() + " beep");
        });

        static void Main(string[] args)
        {
            using (WebApp.Start("http://localhost:9100"))
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
