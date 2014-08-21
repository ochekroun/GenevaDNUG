using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNet.Identity.Tests
{
    public abstract class TestsBase<T>
    {
        private const string ConnectionStringName = "DefaultConnection";

        protected abstract T Store { get; }

        protected UserStore CreateUserStore()
        {
            return new UserStore(ConnectionStringName);
        }

        protected RoleStore CreateRoleStore()
        {
            return new RoleStore(ConnectionStringName);
        }

        protected async Task<User> CreateUser()
        {
            var userStore = new UserStore(ConnectionStringName) as IUserStore<User>;

            var userId = Guid.NewGuid().ToString();
            var userName = Guid.NewGuid().ToString();
            var passwordHash = Guid.NewGuid().ToString();
            var securityStamp = Guid.NewGuid().ToString();
            var phoneNumber = Guid.NewGuid().ToString();
            
            var user = new User
                {
                    Id = userId,
                    UserName = userName,
                    Email = Guid.NewGuid().ToString(),
                    PasswordHash = passwordHash,
                    SecurityStamp = securityStamp,
                    PhoneNumber = phoneNumber,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = true,
                    LockoutEnabled = true,
                    AccessFailedCount = -1
                };


            await userStore.CreateAsync(user);

            return user;
        }

        protected void AssertUsers(User source, User target)
        {
            Assert.AreEqual(source.Id, target.Id, "Id mismatch.");
            Assert.AreEqual(source.UserName, target.UserName, "UserName mismatch.");
            Assert.AreEqual(source.PasswordHash, target.PasswordHash, "Passowrd hash mismatch.");
            Assert.AreEqual(source.Email, target.Email, "Email mismatch.");
            Assert.AreEqual(source.SecurityStamp, target.SecurityStamp, "Security stamp mismatch.");
            Assert.AreEqual(source.EmailConfirmed, target.EmailConfirmed, "EmailConfirmed mismatch.");
            Assert.AreEqual(source.PhoneNumberConfirmed, target.PhoneNumberConfirmed, "PhoneNumberConfirmed mismatch.");
            Assert.AreEqual(source.TwoFactorEnabled, target.TwoFactorEnabled, "TwoFactorEnabled mismatch.");
            Assert.AreEqual(source.LockoutEnabled, target.LockoutEnabled, "LockoutEnabled mismatch.");
            Assert.AreEqual(source.AccessFailedCount, target.AccessFailedCount, "AccessFailedCount mismatch.");
            Assert.AreEqual(source.PhoneNumber, target.PhoneNumber, "PhoneNumber mismatch.");

        }

        protected async Task<Role> CreateRole()
        {
            var roleStore = new RoleStore(ConnectionStringName) as IRoleStore<Role>;

            var roleId = Guid.NewGuid().ToString();
            var roleName = Guid.NewGuid().ToString();
            var role = new Role { Id=roleId, Name =roleName};

            await roleStore.CreateAsync(role);

            return role;
        }
    }
}
