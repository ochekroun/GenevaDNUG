using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNet.Identity.Tests
{
    [TestClass]
    public class IClaimStoreTests : TestsBase<IUserClaimStore<User>>
    {
        //private IClaimStoreTests<User> store;

        private async Task<Tuple<User, Claim>> CreateClaim()
        {
            var claimType = Guid.NewGuid().ToString();
            var claimValue = Guid.NewGuid().ToString();
            var claim = new Claim(claimType, claimValue);

            var user = await CreateUser();
            await Store.AddClaimAsync(user, claim);

            var result = new Tuple<User, Claim>(user, claim);
            return result;
        }

        [TestMethod, TestCategory("IUserClaimStore")]
        public async Task AddClaimAsyncTest()
        {
            await CreateClaim();
        }

        [TestMethod, TestCategory("IUserClaimStore")]
        public async Task GetClaimsAsyncTest()
        {
            var _ = await CreateClaim();
            var identityUser = _.Item1;
            var claim = _.Item2;

            var result = await Store.GetClaimsAsync(identityUser);
            Assert.IsNotNull(result, "No claims found.");
            Assert.AreEqual(result.Count, 1, "No claims found.");
            Assert.AreEqual(result[0].Type, claim.Type, "Claim type mismatch.");
            Assert.AreEqual(result[0].Value, claim.Value, "Claim value mismatch.");
        }


        [TestMethod, TestCategory("IUserClaimStore")]
        public async Task RemoveClaimAsyncTest()
        {
            var _ = await CreateClaim();
            var identityUser = _.Item1;
            var claim = _.Item2;

            await Store.RemoveClaimAsync(identityUser, claim);
        }

        protected override IUserClaimStore<User> Store
        {
            get { return CreateUserStore(); }
        }
    }
}
