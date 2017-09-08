using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace Owin.Tutorials.SignalR.Client.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            HubConnection connection = new HubConnection("http://localhost:9100");
            IHubProxy proxy = connection.CreateHubProxy("myHub");
            connection.Start().ContinueWith(task =>
            {
                proxy.Invoke("Send", ".NET Client Connected");

                proxy.On<string>("sendMessage", (message) =>
                {
                    System.Console.WriteLine(message);
                });
            });
            System.Console.Read();
        }
    }
}
