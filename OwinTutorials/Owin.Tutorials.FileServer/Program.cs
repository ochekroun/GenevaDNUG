using System;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;

namespace Owin.Tutorials.FileServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://localhost:8080";
            var root = @"C:\Work";
            var fileSystem = new PhysicalFileSystem(root);

            var options = new FileServerOptions
            {
                EnableDirectoryBrowsing = true,
                FileSystem = fileSystem
            };

            WebApp.Start(url, builder =>
                {
                    //builder.UseWelcomePage();
                    builder.UseHandlerAsync((req, res) =>
                    {
                        res.ContentType = "text/plain";
                        return res.WriteAsync("Hello World!");
                    });
                    //builder.UseFileServer(options);
                });
            Console.WriteLine("Listening at " + url);
            Console.ReadLine();
        }
    }
}
