using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Owin.Tutorials.WebApi.Logic;

namespace Owin.Tutorials.WebApi.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            Type valuesControllerType = typeof(ValuesController); 
            using (var server = TestServer.Create<Startup>())
            {
                using (var client = new HttpClient(server.Handler))
                {
                    var response = await client.GetAsync("http://testserver/api/values");
                    var result = await response.Content.ReadAsStringAsync();
                    Assert.IsTrue(result.Any());
                }
            }

        }
    }
}
