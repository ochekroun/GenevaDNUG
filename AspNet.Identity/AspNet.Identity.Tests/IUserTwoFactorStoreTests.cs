using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNet.Identity.Tests
{
    [TestClass]
    public class IUserTwoFactorStoreTests : TestsBase<IUserTwoFactorStore<User, string>>
    {
        protected override IUserTwoFactorStore<User, string> Store
        {
            get { return CreateUserStore(); }
        }

        [TestMethod, TestCategory("IUserTwoFactorStoreTests")]
        public async Task GetTwoFactorEnabledAsyncTest()
        {
            var user = await CreateUser();
            const bool twoFactorEnabled = true;
            await Store.SetTwoFactorEnabledAsync(user, twoFactorEnabled);

            var result = await Store.GetTwoFactorEnabledAsync(user);
            Assert.IsNotNull(result, "IUserTwoFactorStoreTests not found.");
            Assert.AreEqual(twoFactorEnabled, result, "IUserTwoFactorStoreTests mismatch.");
        }

        [TestMethod, TestCategory("IUserTwoFactorStoreTests")]
        public async Task SetTwoFactorEnabledAsyncTest()
        {
            var user = await CreateUser();
            const bool twoFactorEnabled = false;
            await Store.SetTwoFactorEnabledAsync(user, twoFactorEnabled);
        }
    }
}