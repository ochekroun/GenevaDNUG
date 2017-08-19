using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;

namespace WepApi.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task GetWithServerClientTest()
        {
            using (var server = TestServer.Create<Startup>())
            {
                {
                    var response = await server.HttpClient.GetAsync("api/values");
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();
                    Assert.IsNotNull(result);
                }
            }

        }
    }
}
