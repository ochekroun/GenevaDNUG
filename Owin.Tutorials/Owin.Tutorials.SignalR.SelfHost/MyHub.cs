using Microsoft.AspNet.SignalR;

namespace Owin.Tutorials.SignalR.SelfHost
{
    public class MyHub : Hub
    {
        public void Send(string message)
        {
            Clients.All.sendMessage(message);
        }
    }
}