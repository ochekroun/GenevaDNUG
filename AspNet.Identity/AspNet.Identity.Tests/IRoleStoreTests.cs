using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNet.Identity.Tests
{
    [TestClass]
    public class IRoleStoreTests : TestsBase<IRoleStore<Role>>
    {
        private void AssertRole(IRole<string> source, IRole<string> target)
        {
            Assert.AreEqual(source.Id, target.Id);
            Assert.AreEqual(source.Name, target.Name);
        }

        [TestMethod, TestCategory("IRoleStore")]
        public async Task CreateAsyncTest()
        {
            await CreateRole();
        }

        [TestMethod, TestCategory("IRoleStore")]
        public async Task DeleteAsyncTest()
        {
            var role = await CreateRole();
            await Store.DeleteAsync(role);
        }

        [TestMethod, TestCategory("IRoleStore")]
        public async Task FindByIdAsyncTest()
        {
            var role = await CreateRole();
            {
                var result = await Store.FindByIdAsync(role.Id);
                Assert.IsNotNull(result, "Cannot find role by id.");
                AssertRole(role, result);
            }
        }

        [TestMethod, TestCategory("IRoleStore")]
        public async Task FindByNameAsyncTest()
        {
            var role = await CreateRole();
            {
                var result = await Store.FindByNameAsync(role.Name);
                Assert.IsNotNull(result, "Cannot find role by name.");
                AssertRole(role, result);
            }
        }

        [TestMethod, TestCategory("IRoleStore")]
        public async Task UpdateAsyncTest()
        {
            var role = await CreateRole();

            var roleName = Guid.NewGuid().ToString();

            role.Name = roleName;

            await Store.UpdateAsync(role);
            var result = await Store.FindByIdAsync(role.Id);

            Assert.IsNotNull(result, "Cannot find role by id.");
            AssertRole(role, result);
        }

        [TestMethod, TestCategory("IRoleStore")]
        public void Dispose()
        {
            Store.Dispose();
        }

        protected override IRoleStore<Role> Store
        {
            get { return CreateRoleStore(); }
        }
    }
}
