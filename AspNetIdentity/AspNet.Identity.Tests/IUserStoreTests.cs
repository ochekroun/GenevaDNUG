using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNet.Identity.Tests
{
    [TestClass]
    public class IUserStoreTests : TestsBase<IUserStore<User>>
    {
        //private IUserStore<User> store;

        [TestMethod, TestCategory("IUserStore")]
        public async Task CreateAsyncTest()
        {
            await CreateUser();
        }

        [TestMethod, TestCategory("IUserStore")]
        public async Task FindByIdAsyncTest()
        {
            var user = await CreateUser();
            {
                var result = await Store.FindByIdAsync(user.Id);
                Assert.IsNotNull(result, "Cannot find user by id.");
                AssertUsers(user, result);
            }
        }

        [TestMethod, TestCategory("IUserStore")]
        public async Task FindByNameAsyncTest()
        {
            var user = await CreateUser();
            {
                var result = await Store.FindByNameAsync(user.UserName);
                Assert.IsNotNull(result, "Cannot find user by name.");
                AssertUsers(user, result);
            }
        }

        [TestMethod, TestCategory("IUserStore")]
        public async Task DeleteAsyncTest()
        {
            var user = await CreateUser();
            await Store.DeleteAsync(user);
        }


        [TestMethod, TestCategory("IUserStore")]
        public async Task UpdateAsyncTest()
        {
            var user = await CreateUser();
                        
            var userName = Guid.NewGuid().ToString();
            var passwordHash = Guid.NewGuid().ToString();

            user.UserName = userName;
            user.PasswordHash = passwordHash;

            await Store.UpdateAsync(user);
            var result = await Store.FindByIdAsync(user.Id);

            Assert.IsNotNull(result, "Cannot find user by id.");
            AssertUsers(user, result);
        }

        [TestMethod, TestCategory("IUserStore")]
        public void Dispose()
        {
            Store.Dispose();
        }

        protected override IUserStore<User> Store
        {
            get { return CreateUserStore(); }
        }
    }
}
