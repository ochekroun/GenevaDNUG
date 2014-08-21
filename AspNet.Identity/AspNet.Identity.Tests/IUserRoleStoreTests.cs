using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNet.Identity.Tests
{
    [TestClass]
    public class IUserRoleStoreTests : TestsBase<IUserRoleStore<User>>
    {
        //private IUserRoleStore<User> store;

        [TestMethod, TestCategory("IUserRoleStore")]
        public async Task AddToRoleAsyncTest()
        {
            var user = await CreateUser();
            var role = await CreateRole();

            await Store.AddToRoleAsync(user, role.Id);
        }

        [TestMethod, TestCategory("IUserRoleStore")]
        public async Task RemoveFromRoleAsyncTest()
        {
            var user = await CreateUser();
            var role = await CreateRole();

            await Store.AddToRoleAsync(user, role.Id);
            await Store.RemoveFromRoleAsync(user, role.Id);
        }

        [TestMethod, TestCategory("IUserRoleStore")]
        public async Task IsInRoleAsyncTest()
        {
            var user = await CreateUser();
            var role = await CreateRole();

            await Store.AddToRoleAsync(user, role.Id);
            await Store.IsInRoleAsync(user, role.Name);
        }

        [TestMethod, TestCategory("IUserRoleStore")]
        public async Task GetRolesAsyncTest()
        {
            var user = await CreateUser();
            var role = await CreateRole();

            await Store.AddToRoleAsync(user, role.Id);
            var roles = await Store.GetRolesAsync(user);

            Assert.IsNotNull(roles, "Roles not found.");
            Assert.IsTrue(roles.Count == 1, "Roles not found.");
            Assert.AreEqual(roles[0], role.Name, "Roles mismatch");
        }

        protected override IUserRoleStore<User> Store
        {
            get { return CreateUserStore(); }
        }
    }
}
