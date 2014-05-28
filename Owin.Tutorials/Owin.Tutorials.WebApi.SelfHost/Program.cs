using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Owin.Tutorials.WebApi.Logic;

namespace Owin.Tutorials.WebApi.SelfHost
{
    class Program
    {
        static void Main()
        {
            Type valuesControllerType = typeof(ValuesController); 
            string baseAddress = "http://localhost:8080/";
            
            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                // Create HttpCient and make a request to api/values 
                HttpClient client = new HttpClient();

                var response = client.GetAsync(baseAddress + "api/values").Result;

                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }

            Console.ReadLine();
        }
    }
}
