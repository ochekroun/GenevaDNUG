using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNet.Identity.Tests
{
    [TestClass]
    public class IUserPhoneNumberStore : TestsBase<IUserPhoneNumberStore<User, string>>
    {
        [TestMethod, TestCategory("IUserPhoneNumberStore")]
        public async Task GetPhoneNumberAsync()
        {
            var user = await CreateUser();
            var phoneNumber = Guid.NewGuid().ToString();
            await Store.SetPhoneNumberAsync(user, phoneNumber);

            var result = await Store.GetPhoneNumberAsync(user);
            Assert.IsNotNull(result, "PhoneNumber not found.");
            Assert.AreEqual(phoneNumber, result, "PhoneNumber mismatch.");
        }

        [TestMethod, TestCategory("IUserPhoneNumberStore")]
        public async Task SetPhoneNumberAsync()
        {
            var user = await CreateUser();
            var phoneNumber = Guid.NewGuid().ToString();
            await Store.SetPhoneNumberAsync(user, phoneNumber);
        }

        [TestMethod, TestCategory("IUserPhoneNumberStore")]
        public async Task GetPhoneNumberConfirmedAsync()
        {
            var user = await CreateUser();
            const bool phoneNumberConfirmed = false;
            await Store.SetPhoneNumberConfirmedAsync(user, phoneNumberConfirmed);

            var result = await Store.GetPhoneNumberConfirmedAsync(user);
            Assert.IsNotNull(result, "Confirmed not found.");
            Assert.IsTrue(phoneNumberConfirmed == result, "Confirmed mismatch.");
        }

        [TestMethod, TestCategory("IUserPhoneNumberStore")]
        public async Task SetPhoneNumberConfirmedAsync()
        {
            var user = await CreateUser();
            const bool phoneNumberConfirmed = false;
            await Store.SetPhoneNumberConfirmedAsync(user, phoneNumberConfirmed);
        }

        protected override IUserPhoneNumberStore<User, string> Store
        {
            get { return CreateUserStore(); }
        }
    }
}
