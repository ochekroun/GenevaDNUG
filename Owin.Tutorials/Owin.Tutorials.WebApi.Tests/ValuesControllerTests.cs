using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Owin.Tutorials.WebApi.Controllers;

namespace Owin.Tutorials.WebApi.Tests
{
    [TestClass]
    public class ValuesControllerTests
    {
        [TestInitialize]
        public void HttpClientSetup()
        {
            System.Reflection.Assembly.Load("Owin.Tutorials.WebApi.Controllers");
        }

        [TestMethod]
        public async Task GetWithServerClientTest()
        {            
            using (var server = TestServer.Create<Startup>())
            {
                {
                    var response = await server.HttpClient.GetAsync("api/values");
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();
                    Assert.IsTrue(result.Any());
                }
            }

        }

        [TestMethod]
        public async Task GetWithServerHandlerTest()
        {
            using (var server = TestServer.Create<Startup>())
            {
                using (var client = new HttpClient(server.Handler))
                {
                    var response = await client.GetAsync("http://testserver/api/values");
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();
                    Assert.IsTrue(result.Any());
                }
            }

        }
    }
}
