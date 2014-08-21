using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNet.Identity.Tests
{
    [TestClass]
    public class ILoginStoreTests : TestsBase<IUserLoginStore<User, string>>
    {
        private async Task<Tuple<User, UserLoginInfo>> CreateLogin()
        {
            var loginProvider = Guid.NewGuid().ToString();
            var providerKey = Guid.NewGuid().ToString();
            var login = new UserLoginInfo(loginProvider, providerKey);

            var user = await CreateUser();
            await Store.AddLoginAsync(user, login);

            var result = new Tuple<User, UserLoginInfo>(user, login);
            return result;
        }

        [TestMethod, TestCategory("IUserLoginStore")]
        public async Task AddLoginAsyncTest()
        {
            await CreateLogin();
        }


        [TestMethod, TestCategory("IUserLoginStore")]
        public async Task GetLoginsAsyncTest()
        {
            var login = await CreateLogin();
            var identityUser = login.Item1;
            var userLoginInfo = login.Item2;

            {
                var result = await Store.GetLoginsAsync(identityUser);
                Assert.IsNotNull(result, "No logins found.");
                Assert.AreEqual(result.Count, 1, "No logins found.");
                Assert.AreEqual(result[0].LoginProvider, userLoginInfo.LoginProvider, "Login provider mismatch.");
                Assert.AreEqual(result[0].ProviderKey, userLoginInfo.ProviderKey, "Provider key value mismatch.");
            }
        }

        [TestMethod, TestCategory("IUserLoginStore")]
        public async Task FindUserByLoginAsyncTest()
        {
            var login = await CreateLogin();
            var identityUser = login.Item1;
            var userLoginInfo = login.Item2;

            {
                var result = await Store.FindAsync(userLoginInfo);
                Assert.IsNotNull(result, "Cannot find user by login.");
                AssertUsers(identityUser, result);
            }
        }

        [TestMethod, TestCategory("IUserLoginStore")]
        public async Task RemoveLoginAsyncTest()
        {
            var login = await CreateLogin();
            var identityUser = login.Item1;
            var userLoginInfo = login.Item2;

            await Store.RemoveLoginAsync(identityUser, userLoginInfo);
        }

        protected override IUserLoginStore<User, string> Store
        {
            get { return CreateUserStore(); }
        }
    }
}
