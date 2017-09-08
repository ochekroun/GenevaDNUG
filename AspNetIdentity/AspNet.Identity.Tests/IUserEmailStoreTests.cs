using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNet.Identity.Tests
{
    [TestClass]
    public class IUserEmailStore : TestsBase<IUserEmailStore<User, string>>
    {
        [TestMethod, TestCategory("IUserEmailStore")]
        public async Task GetEmailAsyncTest()
        {
            var user = await CreateUser();
            var email = Guid.NewGuid().ToString();
            await Store.SetEmailAsync(user, email);

            var result = await Store.GetEmailAsync(user);
            Assert.IsNotNull(result, "Email not found.");
            Assert.AreEqual(email, result, "Email mismatch.");
        }

        [TestMethod, TestCategory("IUserEmailStore")]
        public async Task SetEmailAsyncTest()
        {
            var user = await CreateUser();
            var email = Guid.NewGuid().ToString();
            await Store.SetEmailAsync(user, email);
        }

        [TestMethod, TestCategory("IUserEmailStore")]
        public async Task GetEmailConfirmedAsyncTest()
        {
            var user = await CreateUser();
            const bool emailConfirmed = false;
            await Store.SetEmailConfirmedAsync(user, emailConfirmed);

            var result = await Store.GetEmailConfirmedAsync(user);
            Assert.IsNotNull(result, "Confirmed not found.");
            Assert.IsTrue(emailConfirmed == result, "Confirmed mismatch.");
        }

        [TestMethod, TestCategory("IUserEmailStore")]
        public async Task SetEmailConfirmedAsyncTest()
        {
            var user = await CreateUser();
            const bool emailConfirmed = false;
            await Store.SetEmailConfirmedAsync(user, emailConfirmed);
        }

        [TestMethod, TestCategory("IUserEmailStore")]
        public async Task FindByEmailAsyncTest()
        {
            var user = await CreateUser();
            var email = Guid.NewGuid().ToString();
            await Store.SetEmailAsync(user, email);
            var result = await Store.FindByEmailAsync(email);

            Assert.IsNotNull(result, "User not found.");
            AssertUsers(user, result);
        }

        protected override IUserEmailStore<User, string> Store
        {
            get { return CreateUserStore(); }
        }
    }
}
