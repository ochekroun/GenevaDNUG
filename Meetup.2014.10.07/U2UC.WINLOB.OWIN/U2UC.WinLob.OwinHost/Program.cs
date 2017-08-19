namespace U2UC.WinLob.OwinHost
{
    using Microsoft.Owin.Hosting;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;

    // More details on hosting WebApi here:
    // http://www.asp.net/web-api/overview/hosting-aspnet-web-api/use-owin-to-self-host-web-api
    public class Program
    {
        static void Main()
        {
            string baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                // Create HttpCient and make a request to api/northwind 
                HttpClient client = new HttpClient();
                var response = client.GetStringAsync(baseAddress + "api/northwind").Result;

                List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(response);

                Console.WriteLine("{0} Northwind employees found.", employees.Count);

                Console.ReadLine(); // In the using, not after it like in the demo :-)
            }
        }
    }
}
