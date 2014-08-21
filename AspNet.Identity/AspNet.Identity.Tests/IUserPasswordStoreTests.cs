using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNet.Identity.Tests
{
    [TestClass]
    public class IUserPasswordStoreTests : TestsBase<IUserPasswordStore<User>>
    {
            
        [TestMethod, TestCategory("IUserPasswordStore")]
        public async Task GetPasswordHashAsyncTest()
        {
            var user = await CreateUser();
            var passwordHash = Guid.NewGuid().ToString();
            await Store.SetPasswordHashAsync(user, passwordHash);

            var result = await Store.GetPasswordHashAsync(user);
            Assert.IsNotNull(result, "Password hash not found.");
            Assert.AreEqual(passwordHash, result, "Password hash mismatch.");
        }

        [TestMethod, TestCategory("IUserPasswordStore")]
        public async Task HasPasswordAsyncTest()
        {
            var user = await CreateUser();
            await Store.HasPasswordAsync(user);

            var passwordHash = Guid.NewGuid().ToString();
            await Store.SetPasswordHashAsync(user, passwordHash);

            {
                var result = await Store.HasPasswordAsync(user);
                Assert.IsTrue(result, "Password not found.");
            }
        }


        [TestMethod, TestCategory("IUserPasswordStore")]
        public async Task SetPasswordHashAsyncTest()
        {
            var user = await CreateUser();
            var passwordHash = Guid.NewGuid().ToString();
            await Store.SetPasswordHashAsync(user, passwordHash);
        }

        protected override IUserPasswordStore<User> Store
        {
            get { return CreateUserStore(); }
        }
    }
}
