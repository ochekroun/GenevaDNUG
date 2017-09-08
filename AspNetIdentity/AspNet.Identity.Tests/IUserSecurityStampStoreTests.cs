using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNet.Identity.Tests
{
    [TestClass]
    public class IUserSecurityStampStore : TestsBase<IUserSecurityStampStore<User, string>>
    {
        [TestMethod, TestCategory("IUserSecurityStampStore")]
        public async Task GetSecurityStampAsyncTest()
        {
            var user = await CreateUser();
            var securityStamp = Guid.NewGuid().ToString();
            await Store.SetSecurityStampAsync(user, securityStamp);

            var result = await Store.GetSecurityStampAsync(user);
            Assert.IsNotNull(result, "SecurityStamp not found.");
            Assert.AreEqual(securityStamp, result, "SecurityStamp mismatch.");
        }

        [TestMethod, TestCategory("IUserSecurityStampStore")]
        public async Task SetSecurityStampAsyncTest()
        {
            var user = await CreateUser();
            var securityStamp = Guid.NewGuid().ToString();
            await Store.SetSecurityStampAsync(user, securityStamp);
        }

        protected override IUserSecurityStampStore<User, string> Store
        {
            get { return CreateUserStore(); }
        }
    }
}
