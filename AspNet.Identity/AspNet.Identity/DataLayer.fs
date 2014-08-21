namespace AspNet.Identity

open System.Security.Claims
open System.Collections.Generic
open System.Threading.Tasks

open Microsoft.AspNet.Identity

module Async =
    let inline AwaitPlainTask (task: Task) = 
        // rethrow exception from preceding task if it fauled
        let continuation (t : Task) : unit =
            match t.IsFaulted with
            | true -> raise t.Exception
            | arg -> ()
        task.ContinueWith continuation |> Async.AwaitTask
 
    let inline StartAsPlainTask (work : Async<unit>) = Task.Factory.StartNew(fun () -> work |> Async.RunSynchronously)

type DataLayer(connectionStringName:string) =

    let getConnection() =
        let connection = DynamicSqlConnection.CreateConnectionFromConfig(connectionStringName)
        connection

    let readUser(reader:DynamicSqlDataReader) =
        let result = 
            match reader.Read() with
            | true ->
                let user = new User()
                user.Id <- reader?Id
                user.UserName <- reader?UserName
                user.Email <- reader?Email
                user.SecurityStamp <- reader?SecurityStamp
                user.PasswordHash <- reader?PasswordHash
                user.EmailConfirmed <- reader?EmailConfirmed
                user.PhoneNumberConfirmed <- reader?PhoneNumberConfirmed
                user.TwoFactorEnabled <- reader?TwoFactorEnabled
                user.LockoutEnabled <- reader?LockoutEnabled
                user.AccessFailedCount <- reader?AccessFailedCount
                user.PhoneNumber <- reader?PhoneNumber
                user
            | false ->
                Unchecked.defaultof<User>
        result

    let setUserCommand(user:User, command:DynamicSqlCommand)=
        command?UserName <- user.UserName
        command?Id <- user.Id
        command?Email <- user.Email
        command?PasswordHash <- user.PasswordHash
        command?SecurityStamp <- user.SecurityStamp
        command?EmailConfirmed <- user.EmailConfirmed
        command?PhoneNumberConfirmed <- user.PhoneNumberConfirmed
        command?TwoFactorEnabled <- user.TwoFactorEnabled
        command?LockoutEnabled <- user.LockoutEnabled
        command?AccessFailedCount <- user.AccessFailedCount
        command?PhoneNumber <- user.PhoneNumber

    member self.CreateRoleAsync(role:Role) = 
        if System.String.IsNullOrEmpty(role.Name) then 
            raise(new System.ArgumentException("Invalid role."))

        async{
            role.Id <- System.Guid.NewGuid().ToString()
            use connection = getConnection()
            use command = connection?("Insert into Roles (Id, Name) values (@Id, @Name)")
            command?Id <- role.Id
            command?Name <- role.Name
            command.ExecuteNonQuery() |> ignore
        } |> Async.StartAsPlainTask

    member self.FindRoleByIdAsync(roleId:string) =
        async{
            use connection = getConnection()        
            use command = connection?("Select Name from Roles where Id = @Id")
            command?Id <- roleId

            use reader = command.ExecuteReader()
            let result = 
                match reader.Read() with
                | true ->
                    let roleName = reader?Name
                    let result = new Role()
                    result.Id <- roleId
                    result.Name <- roleName
                    result
                | false ->
                    Unchecked.defaultof<Role>
            return result
        } |> Async.StartAsTask

    member self.FindRoleByNameAsync(roleName:string) =
        async{
            use connection = getConnection()        
            use command = connection?("Select * from Roles where Name = @Name")
            command?Name <- roleName

            use reader = command.ExecuteReader()
            let result = 
                match reader.Read() with
                | true ->
                    let result = new Role()
                    result.Id <- reader?Id
                    result.Name <- reader?Name
                    result
                | false ->
                    Unchecked.defaultof<Role>
            return result
        } |> Async.StartAsTask


    member self.UpdateRoleAsync(role:Role) =
        async{
            use connection = getConnection()        
            use command = connection?("Update Roles set Name = @Name where Id = @Id")
            command?Id <- role.Id
            command?Name <- role.Name

            command.ExecuteNonQuery() |> ignore
        } |> Async.StartAsPlainTask

    member self.DeleteRoleAsync(role:Role) =
        async{
            use connection = getConnection()        
            use command = connection?("Delete from Roles where Id = @Id")
            command?Id <- role.Id

            command.ExecuteNonQuery() |> ignore
        } |> Async.StartAsPlainTask

    member self.CreateUserAsync(user:User) = 
        async{
            do user.Id <- System.Guid.NewGuid().ToString()
            let query = """
                Insert into Users (UserName, Id, Email, PasswordHash, SecurityStamp, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, PhoneNumber) 
                values (@UserName, @Id, @Email, @PasswordHash, @SecurityStamp, @EmailConfirmed, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEnabled, @AccessFailedCount, @PhoneNumber)
            """

            use connection = getConnection()
            use command = connection?(query)
            setUserCommand(user, command)
            command.ExecuteNonQuery() |> ignore
        } |> Async.StartAsPlainTask
    
    member self.UpdateUserAsync(user:User) = 
        async{
            use connection = getConnection()        
            use command = connection?("Update Users set UserName = @UserName, Email = @Email, PasswordHash = @PasswordHash, SecurityStamp = @SecurityStamp, EmailConfirmed=@EmailConfirmed, PhoneNumberConfirmed=@PhoneNumberConfirmed, TwoFactorEnabled=@TwoFactorEnabled, LockoutEnabled=@LockoutEnabled, AccessFailedCount=@AccessFailedCount, PhoneNumber=@PhoneNumber WHERE Id = @Id")
            setUserCommand(user, command)            
            command.ExecuteNonQuery() |> ignore
        } |> Async.StartAsPlainTask


    member self.DeleteUserAsync(user:User) = 
        async{
            use connection = getConnection()        
            use command = connection?("Delete from Users where Id = @Id")
            command?Id <- user.Id

            command.ExecuteNonQuery() |> ignore
        } |> Async.StartAsPlainTask

    member self.FindUserByIdAsync(userId:string) = 
        async{
            use connection = getConnection()
            use command = connection?("Select * from Users where Id = @id")
            command?Id <- userId

            use reader = command.ExecuteReader()
            let result = readUser(reader)
            return result
        } |> Async.StartAsTask

    member self.FindUserByNameAsync(userName:string) = 
        async{
            use connection = getConnection()
            use command = connection?("Select * from Users where UserName = @UserName")
            command?UserName <- userName

            use reader = command.ExecuteReader()
            let result = readUser(reader)
            return result
        } |> Async.StartAsTask

    member self.AddClaimAsync(user:User, claim:Claim) = 
        async{
            use connection = getConnection()
            use command = connection?("Insert into UserClaims (ClaimValue, ClaimType, ClaimValueType, UserId) values (@Value, @Type, @ClaimValueType, @UserId)")
            command?Value <- claim.Value
            command?Type <- claim.Type
            command?ClaimValueType <- claim.ValueType
            command?UserId <- user.Id
            command.ExecuteNonQuery() |> ignore
        } |> Async.StartAsPlainTask

    member self.RemoveClaimAsync(user:User, claim:Claim) = 
        async{
            use connection = getConnection()        
            use command = connection?("Delete from UserClaims where UserId=@userId and ClaimValue=@ClaimValue and ClaimType=@ClaimType")
            command?ClaimValue <- claim.Value
            command?ClaimType <- claim.Type
            command?UserId <- user.Id

            command.ExecuteNonQuery() |> ignore
        } |> Async.StartAsPlainTask


    member self.GetClaimsAsync(user:User) = 
        async{
            let result = new List<Claim>()

            use connection = getConnection()
            use command = connection?("Select * from UserClaims where UserId = @userId")
            command?UserId <- user.Id

            use reader = command.ExecuteReader()

            let result = Seq.toArray(seq {
                while reader.Read() do
                    let claimType = reader?ClaimType
                    let claimValue = reader?ClaimValue
                    let claimValueType = reader?ClaimValueType
                    let claim = new Claim(claimType, claimValue, claimValueType)
                    yield claim
            })

            return result :> IList<Claim>
        } |> Async.StartAsTask

    member self.AddLoginAsync(user:User, login:UserLoginInfo) = 
        async{
            use connection = getConnection()
            use command = connection?("Insert into UserLogins (LoginProvider, ProviderKey, UserId) values (@loginProvider, @providerKey, @userId)")
            command?loginProvider <- login.LoginProvider
            command?providerKey <- login.ProviderKey
            command?UserId <- user.Id
            command.ExecuteNonQuery() |> ignore
        } |> Async.StartAsPlainTask

    member self.RemoveLoginAsync(user:User, login:UserLoginInfo) = 
        async{
            use connection = getConnection()        
            use command = connection?("Delete from UserLogins where UserId = @userId and LoginProvider = @loginProvider and ProviderKey = @providerKey")
            command?userId <- user.Id
            command?loginProvider <- login.LoginProvider
            command?providerKey <- login.ProviderKey

            command.ExecuteNonQuery() |> ignore
        } |> Async.StartAsPlainTask

    member self.GetLoginsAsync(user:User) = 
        async{
            let esult = new List<Claim>()

            use connection = getConnection()
            use command = connection?("Select * from UserLogins where UserId = @userId")
            command?UserId <- user.Id

            use reader = command.ExecuteReader()

            let result = Seq.toArray(seq {
                while reader.Read() do
                    let loginProvider = reader?LoginProvider
                    let providerKey = reader?ProviderKey
                    let claim = new UserLoginInfo(loginProvider, providerKey)
                    yield claim
            })

            return result :> IList<UserLoginInfo>
        } |> Async.StartAsTask
    
    member self.FindUserIdByLoginAsync(user:UserLoginInfo) = 
        async{
            use connection = getConnection()
            use command = connection?("Select UserId from UserLogins where LoginProvider = @loginProvider and ProviderKey = @providerKey")
            command?loginProvider <- user.LoginProvider
            command?ProviderKey <- user.ProviderKey

            let userId = command.ExecuteScalar() :?> string
            return userId

        } |> Async.StartAsTask


    member self.AddUserToRoleAsync(user:User, roleId:string) =
        async{
            use connection = getConnection()        
            use command = connection?("Insert into UserRoles (UserId, RoleId) values (@userId, @roleId)")
            command?userId <- user.Id
            command?roleId <- roleId

            command.ExecuteNonQuery() |> ignore
        } |> Async.StartAsPlainTask

    member self.RemoveUserFromRoleAsync(user:User, roleId:string) =
        async{
            use connection = getConnection()        
            use command = connection?("DELETE FROM UserRoles WHERE UserId=@UserId AND RoleId=@roleId")
            command?userId <- user.Id
            command?roleId <- roleId

            command.ExecuteNonQuery() |> ignore
        } |> Async.StartAsPlainTask



    member self.GetUserRolesAsync(user:User) = 
        async{
            use connection = getConnection()
            use command = connection?("Select Roles.Name from UserRoles, Roles where UserRoles.UserId = @userId and UserRoles.RoleId = Roles.Id")
            command?userId <- user.Id

            use reader = command.ExecuteReader()

            let result = Seq.toArray(seq {
                while reader.Read() do
                    let roleName = reader?Name
                    yield roleName
            })

            return result :> IList<string>

        } |> Async.StartAsTask
        

    member self.GetPasswordHashAsync(user:User) =
        async{
            use connection = getConnection()
            use command = connection?("Select PasswordHash from Users where Id = @Id")
            command?Id <- user.Id

            let passwordHash = command.ExecuteScalar() :?> string
            return passwordHash

        } |> Async.StartAsTask
        
    member self.SetPasswordHashAsync(user:User, passwordHash:string) =
        async{
            user.PasswordHash <- passwordHash
            use connection = getConnection()        
            use command = connection?("UPDATE Users SET PasswordHash=@PasswordHash WHERE Id=@Id")
            command?Id <- user.Id
            command?PasswordHash <- passwordHash
            command.ExecuteNonQuery() |> ignore
        } |> Async.StartAsPlainTask

    member self.GetSecurityStampAsync(user:User) =
        async{
            use connection = getConnection()
            use command = connection?("Select SecurityStamp from Users where Id = @Id")
            command?Id <- user.Id

            let passwordHash = command.ExecuteScalar() :?> string
            return passwordHash

        } |> Async.StartAsTask
    member self.SetSecurityStampAsync(user:User,securityStamp:string) =
        async{
            user.SecurityStamp <- securityStamp
            use connection = getConnection()        
            use command = connection?("UPDATE Users SET SecurityStamp=@SecurityStamp WHERE Id=@Id")
            command?Id <- user.Id
            command?SecurityStamp <- securityStamp
            command.ExecuteNonQuery() |> ignore
        } |> Async.StartAsPlainTask

    member self.GetTwoFactorEnabledAsync(user:User) =
        async{
            use connection = getConnection()
            use command = connection?("Select TwoFactorEnabled from Users where Id = @Id")
            command?Id <- user.Id

            let result = command.ExecuteScalar() :?> bool
            return result

        } |> Async.StartAsTask
        
    member self.SetTwoFactorEnabledAsync(user:User, twoFactorEnabled:bool) =
        async{
            use connection = getConnection()        
            use command = connection?("UPDATE Users SET TwoFactorEnabled=@TwoFactorEnabled WHERE Id=@Id")
            command?Id <- user.Id
            command?TwoFactorEnabled <- twoFactorEnabled
            command.ExecuteNonQuery() |> ignore
        } |> Async.StartAsPlainTask


    member self.GetPhoneNumberAsync(user:User) = 
        async{
            use connection = getConnection()
            use command = connection?("Select PhoneNumber from Users where Id = @Id")
            command?Id <- user.Id

            let result = command.ExecuteScalar() :?> string
            return result

        } |> Async.StartAsTask
  
    member self.GetPhoneNumberConfirmedAsync(user:User) =
        async{
            use connection = getConnection()
            use command = connection?("Select PhoneNumberConfirmed from Users where Id = @Id")
            command?Id <- user.Id

            let result = command.ExecuteScalar() :?> bool
            return result

        } |> Async.StartAsTask
    
    member self.SetPhoneNumberAsync(user:User, phoneNumber:string) =
        async{
            use connection = getConnection()        
            use command = connection?("UPDATE Users SET PhoneNumber=@PhoneNumber WHERE Id=@Id")
            command?Id <- user.Id
            command?PhoneNumber <- phoneNumber
            command.ExecuteNonQuery() |> ignore
        } |> Async.StartAsPlainTask
    
    member self.SetPhoneNumberConfirmedAsync(user:User, phoneNumberConfirmed:bool) =
        async{
            use connection = getConnection()        
            use command = connection?("UPDATE Users SET PhoneNumberConfirmed=@PhoneNumberConfirmed WHERE Id=@Id")
            command?Id <- user.Id
            command?PhoneNumberConfirmed <- phoneNumberConfirmed
            command.ExecuteNonQuery() |> ignore
        } |> Async.StartAsPlainTask

    member self.FindByEmailAsync(email:string) = 
        async{
            use connection = getConnection()
            use command = connection?("Select * from Users where Email = @Email")
            command?Email <- email

            use reader = command.ExecuteReader()
            let result = readUser(reader)
            return result

        } |> Async.StartAsTask

    member self.GetEmailAsync(user:User) = 
        async{
            use connection = getConnection()
            use command = connection?("Select Email from Users where Id = @Id")
            command?Id <- user.Id

            let result = command.ExecuteScalar() :?> string
            return result

        } |> Async.StartAsTask
    
    member self.GetEmailConfirmedAsync(user:User) = 
        async{
            use connection = getConnection()
            use command = connection?("Select EmailConfirmed from Users where Id = @Id")
            command?Id <- user.Id

            let result = command.ExecuteScalar() :?> bool
            return result

        } |> Async.StartAsTask
    member self.SetEmailConfirmedAsync(user:User, confirmed:bool) = 
        async{
            use connection = getConnection()        
            use command = connection?("UPDATE Users SET EmailConfirmed=@EmailConfirmed WHERE Id=@Id")
            command?Id <- user.Id
            command?EmailConfirmed <- confirmed
            command.ExecuteNonQuery() |> ignore
        } |> Async.StartAsPlainTask

    member self.SetEmailAsync(user:User, email:string) = 
        async{
            user.Email <- email
            use connection = getConnection()        
            use command = connection?("UPDATE Users SET Email=@Email WHERE Id=@Id")
            command?Id <- user.Id
            command?Email <- email
            command.ExecuteNonQuery() |> ignore
        } |> Async.StartAsPlainTask


    member self.GetLockoutEndDateAsync(user:User) = 
        async{
            use connection = getConnection()
            use command = connection?("Select LockoutEndDateUtc from Users where Id = @Id")
            command?Id <- user.Id

            let date = command.ExecuteScalar() :?> System.DateTime
            let result = new System.DateTimeOffset(date, new System.TimeSpan())
            return result

        } |> Async.StartAsTask
    member self.GetLockoutEnabledAsync(user:User) = 
        async{
            use connection = getConnection()
            use command = connection?("Select LockoutEnabled from Users where Id = @Id")
            command?Id <- user.Id

            let result = command.ExecuteScalar() :?> bool
            return result

        } |> Async.StartAsTask
    member self.GetAccessFailedCountAsync(user:User) = 
        async{
            use connection = getConnection()
            use command = connection?("Select AccessFailedCount from Users where Id = @Id")
            command?Id <- user.Id

            let result = command.ExecuteScalar() :?> int
            return result

        } |> Async.StartAsTask

    member self.IncrementAccessFailedCountAsync(user:User) = 
        async{
            user.AccessFailedCount <- user.AccessFailedCount + 1
            use connection = getConnection()        
            use command = connection?("UPDATE Users SET AccessFailedCount=@AccessFailedCount WHERE Id=@Id")
            command?Id <- user.Id
            command?AccessFailedCount <- user.AccessFailedCount
            command.ExecuteNonQuery() |> ignore
            
            use command = connection?("Select AccessFailedCount from Users where Id = @Id")
            command?Id <- user.Id

            let result = command.ExecuteScalar() :?> int
            return result
        } |> Async.StartAsTask

    member self.ResetAccessFailedCountAsync(user:User) = 
        async{
            use connection = getConnection()        
            use command = connection?("UPDATE Users SET AccessFailedCount=0 WHERE Id=@Id")
            command?Id <- user.Id
            command.ExecuteNonQuery() |> ignore
        } |> Async.StartAsPlainTask

    member self.SetLockoutEndDateAsync(user:User, endDate:System.DateTimeOffset) = 
        async{
            use connection = getConnection()        
            use command = connection?("UPDATE Users SET LockoutEndDateUtc=@LockoutEndDateUtc WHERE Id=@Id")
            command?Id <- user.Id
            command?LockoutEndDateUtc <- endDate.DateTime
            command.ExecuteNonQuery() |> ignore
        } |> Async.StartAsPlainTask

    member self.SetLockoutEnabledAsync(user:User, lockoutEnabled:bool) = 
        async{
            user.LockoutEnabled <- lockoutEnabled
            use connection = getConnection()        
            use command = connection?("UPDATE Users SET LockoutEnabled=@LockoutEnabled WHERE Id=@Id")
            command?Id <- user.Id
            command?LockoutEnabled <- lockoutEnabled
            command.ExecuteNonQuery() |> ignore
        } |> Async.StartAsPlainTask
