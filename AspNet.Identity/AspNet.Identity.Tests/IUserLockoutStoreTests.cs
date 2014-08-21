using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNet.Identity.Tests
{
    [TestClass]
    public class IUserLockoutStoreTests : TestsBase<IUserLockoutStore<User, string>>
    {

        [TestMethod, TestCategory("IUserLockoutStoreTests")]
        public async Task GetLockoutEndDateAsyncTest()
        {
            var user = await CreateUser();
            var lockoutEndDate = System.DateTimeOffset.UtcNow;

            await Store.SetLockoutEndDateAsync(user, lockoutEndDate);


            var result = await Store.GetLockoutEndDateAsync(user);

            Assert.AreEqual(lockoutEndDate.Year, result.Year);
            Assert.AreEqual(lockoutEndDate.Month, result.Month);
            Assert.AreEqual(lockoutEndDate.Day, result.Day);
            Assert.AreEqual(lockoutEndDate.Hour, result.Hour);
            Assert.AreEqual(lockoutEndDate.Minute, result.Minute);
            Assert.AreEqual(lockoutEndDate.Second, result.Second);
            //Assert.AreEqual(lockoutEndDate.Millisecond, result.Millisecond);
            //Assert.AreEqual(lockoutEndDate.Ticks, result.Ticks);
        }

        [TestMethod, TestCategory("IUserLockoutStoreTests")]
        public async Task GetLockoutEnabledAsyncTest()
        {
            var user = await CreateUser();
            const bool lockoutEnabled = false;
            await Store.SetLockoutEnabledAsync(user, lockoutEnabled);
            var result = await Store.GetLockoutEnabledAsync(user);

            Assert.AreEqual(lockoutEnabled, result);
        }

        [TestMethod, TestCategory("IUserLockoutStoreTests")]
        public async Task GetAccessFailedCountAsyncTest()
        {
            var user = await CreateUser();
            await Store.GetAccessFailedCountAsync(user);
        }

        [TestMethod, TestCategory("IUserLockoutStoreTests")]
        public async Task IncrementAccessFailedCountAsyncTest()
        {
            var user = await CreateUser();
            var count0 = await Store.GetAccessFailedCountAsync(user);
            await Store.IncrementAccessFailedCountAsync(user);
            var count1 = await Store.GetAccessFailedCountAsync(user);
            Assert.IsTrue(count0 + 1 == count1, "No increment.");
        }

        [TestMethod, TestCategory("IUserLockoutStoreTests")]
        public async Task ResetAccessFailedCountAsyncTest()
        {
            var user = await CreateUser();
            await Store.IncrementAccessFailedCountAsync(user);
            await Store.ResetAccessFailedCountAsync(user);
            var count1 = await Store.GetAccessFailedCountAsync(user);
            Assert.IsTrue(count1 == 0, "Bad reset.");
        }

        [TestMethod, TestCategory("IUserLockoutStoreTests")]
        public async Task SetLockoutEnabledAsyncTest()
        {
            var user = await CreateUser();
            const bool lockoutEnabled = false;
            await Store.SetLockoutEnabledAsync(user, lockoutEnabled);
        }

        [TestMethod, TestCategory("IUserLockoutStoreTests")]
        public async Task SetLockoutEndDateAsyncTest()
        {
            var user = await CreateUser();
            var lockoutEndDate = System.DateTimeOffset.UtcNow;
            await Store.SetLockoutEndDateAsync(user, lockoutEndDate);
        }

        protected override IUserLockoutStore<User, string> Store
        {
            get { return CreateUserStore(); }
        }
    }
}
