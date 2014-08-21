namespace AspNet.Identity

open  Microsoft.AspNet.Identity

type User() = 

    let mutable email = ""

    member val Id = "" with get, set
    member val UserName = "" with get, set
    //member val Email = "" with get, set
    member val PasswordHash = "" with get, set
    member val SecurityStamp = "" with get, set
    member val EmailConfirmed = false with get, set
    member val PhoneNumberConfirmed = false with get, set
    member val TwoFactorEnabled = false with get, set
    member val LockoutEnabled = false with get, set
    member val AccessFailedCount = 0 with get, set
    member val PhoneNumber = "" with get, set

    member self.Email
        with get() = email
        and set(value) = email <- value
    
    interface IUser with
        member self.Id 
            with get() = self.Id

        member self.UserName 
            with get() = self.UserName
            and set(value) = self.UserName <- value

type Role() = 

    member val Id = "" with get, set
    member val Name = "" with get, set

    interface IRole

    interface IRole<string> with
        member self.Id 
            with get() = self.Id

        member self.Name 
            with get() = self.Name
            and set(value) = self.Name <- value

