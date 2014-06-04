using Microsoft.Owin.Cors;

namespace Owin.Tutorials.SignalR.SelfHost
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}