using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloOwinSelfHost
{
    namespace Owin.Tutorials.FileServer
    {
        class Program
        {
            static void Main(string[] args)
            {
                var url = "http://localhost:12345";
                var root = @"C:\Work";
                var fileSystem = new PhysicalFileSystem(root);

                var options = new FileServerOptions
                {
                    EnableDirectoryBrowsing = true,
                    FileSystem = fileSystem
                };

                WebApp.Start(url, builder => builder.UseFileServer(options));
                Console.WriteLine("Listening at " + url);
                Console.ReadLine();
            }
        }
    }
}
