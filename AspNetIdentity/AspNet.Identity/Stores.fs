namespace AspNet.Identity

open System

open Microsoft.AspNet.Identity

type RoleStore(connectionStringName) = 
    let dal = new DataLayer(connectionStringName)
    
    interface IRoleStore<Role> with
        member self.CreateAsync(role) = dal.CreateRoleAsync(role)
        member self.UpdateAsync(role) = dal.UpdateRoleAsync(role)
        member self.DeleteAsync(role) = dal.DeleteRoleAsync(role)
        member self.FindByIdAsync(roleId) = dal.FindRoleByIdAsync(roleId)
        member self.FindByNameAsync(roleName) = dal.FindRoleByNameAsync(roleName)
        member self.Dispose() = ()

type UserStore(connectionStringName) = 
    let dal = new DataLayer(connectionStringName)

    interface IUserStore<User>         
    
    interface IUserStore<User, string> with
        member self.CreateAsync(user) = dal.CreateUserAsync(user)
        member self.UpdateAsync(user) = dal.UpdateUserAsync(user)
        member self.DeleteAsync(user) = dal.DeleteUserAsync(user)
        member self.FindByIdAsync(userId) = dal.FindUserByIdAsync(userId)
        member self.FindByNameAsync(userName) = dal.FindUserByNameAsync(userName)
        member self.Dispose() = ()

    interface IUserClaimStore<User> with
        member self.AddClaimAsync(user, claim) = dal.AddClaimAsync(user, claim)
        member self.GetClaimsAsync(user) = dal.GetClaimsAsync(user)
        member self.RemoveClaimAsync(user, claim) = dal.RemoveClaimAsync(user, claim)

    interface IUserLoginStore<User, string> with
        member self.AddLoginAsync(user, login) = dal.AddLoginAsync(user, login)        
        member self.GetLoginsAsync(user) = dal.GetLoginsAsync(user)
        member self.RemoveLoginAsync(user, login) = dal.RemoveLoginAsync(user, login)
        member self.FindAsync(login) = 
            async{
                let! userId = dal.FindUserIdByLoginAsync(login) |> Async.AwaitTask
                let! result = dal.FindUserByIdAsync(userId) |> Async.AwaitTask
                return result
                } |> Async.StartAsTask

    interface IUserRoleStore<User> with
        member self.AddToRoleAsync(user, roleName) = dal.AddUserToRoleAsync(user, roleName)
        member self.RemoveFromRoleAsync(user, roleName) = dal.RemoveUserFromRoleAsync(user, roleName)
        member self.GetRolesAsync(user) = dal.GetUserRolesAsync(user)
        member self.IsInRoleAsync(user, roleName) = 
            async{
                let! roles = dal.GetUserRolesAsync(user) |> Async.AwaitTask
                let result = roles.Contains(roleName)
                return result
                } |> Async.StartAsTask
        
    interface IUserPasswordStore<User> with
        member self.GetPasswordHashAsync(user) = dal.GetPasswordHashAsync(user)
        member self.SetPasswordHashAsync(user, passwordHash) = dal.SetPasswordHashAsync(user, passwordHash)
        member self.HasPasswordAsync(user) = 
            async{
                let! passwordHash = dal.GetPasswordHashAsync(user) |> Async.AwaitTask
                let result = not(String.IsNullOrEmpty(passwordHash))
                return result
                } |> Async.StartAsTask
        
    interface IUserSecurityStampStore<User, string> with
        member self.GetSecurityStampAsync(user) = dal.GetSecurityStampAsync(user)
        member self.SetSecurityStampAsync(user, securityStamp) = dal.SetSecurityStampAsync(user, securityStamp)

    interface IUserTwoFactorStore<User, string> with
        member self.GetTwoFactorEnabledAsync(user) = dal.GetTwoFactorEnabledAsync(user)
        member self.SetTwoFactorEnabledAsync(user, enabled) = dal.SetTwoFactorEnabledAsync(user, enabled)

    interface IUserPhoneNumberStore<User, string> with
        member self.GetPhoneNumberAsync(user) = dal.GetPhoneNumberAsync(user)
        member self.GetPhoneNumberConfirmedAsync(user) = dal.GetPhoneNumberConfirmedAsync(user)
        member self.SetPhoneNumberAsync(user, phoneNumber) = dal.SetPhoneNumberAsync(user, phoneNumber) 
        member self.SetPhoneNumberConfirmedAsync(user, phoneNumber) = dal.SetPhoneNumberConfirmedAsync(user, phoneNumber)

    interface IUserEmailStore<User, string> with
        member self.FindByEmailAsync(email) = dal.FindByEmailAsync(email)
        member self.GetEmailAsync(user) = dal.GetEmailAsync(user)
        member self.SetEmailAsync(user, email) = dal.SetEmailAsync(user, email)
        member self.GetEmailConfirmedAsync(user) = dal.GetEmailConfirmedAsync(user)
        member self.SetEmailConfirmedAsync(user, confirmed) = dal.SetEmailConfirmedAsync(user, confirmed)

    interface IUserLockoutStore<User, string> with
        member self.GetLockoutEndDateAsync(user) = dal.GetLockoutEndDateAsync(user)
        member self.GetLockoutEnabledAsync(user) = dal.GetLockoutEnabledAsync(user)
        member self.GetAccessFailedCountAsync(user) = dal.GetAccessFailedCountAsync(user)
        member self.IncrementAccessFailedCountAsync(user) = dal.IncrementAccessFailedCountAsync(user)
        member self.ResetAccessFailedCountAsync(user) = dal.ResetAccessFailedCountAsync(user)
        member self.SetLockoutEnabledAsync(user, lockoutEnabled) = dal.SetLockoutEnabledAsync(user, lockoutEnabled)
        member self.SetLockoutEndDateAsync(user, endDate) = dal.SetLockoutEndDateAsync(user, endDate)
