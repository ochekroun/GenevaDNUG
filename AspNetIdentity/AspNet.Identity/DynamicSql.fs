namespace AspNet.Identity

open System
open System.Data
open System.Data.Common
open System.Configuration

// --------------------------------------------------------------------------------------
// Wrappers with dynamic operators for creating SQL calls

type public DynamicSqlDataReader(reader:IDataReader) =
    member private x.Reader = reader
    member x.Read() = reader.Read()
    static member (?) (self:DynamicSqlDataReader, name:string) : 'R = 
        let value = self.Reader.[name]
        match value with
        | :? System.DBNull  -> Unchecked.defaultof<'R>
        | _ -> value :?> 'R
    interface IDisposable with
        member x.Dispose() = reader.Dispose()

type public DynamicSqlCommand(cmd:IDbCommand) = 
    member private x.Command = cmd
    static member (?<-) (self:DynamicSqlCommand, name:string, value) = 
        let param = self.Command.CreateParameter()
        param.ParameterName <- name
        param.Value <- 
            match value with
            | null -> System.DBNull.Value :> System.Object
            | _ -> value
        self.Command.Parameters.Add(param) |> ignore
    member x.ExecuteNonQuery() = cmd.ExecuteNonQuery()
    member x.ExecuteReader() = new DynamicSqlDataReader(cmd.ExecuteReader())
    member x.ExecuteScalar() = 
        let value = cmd.ExecuteScalar()
        match value with
        | :? DBNull -> null
        | _ -> value

    member x.Parameters = cmd.Parameters
    member x.Transaction 
        with get () = cmd.Transaction
        and set(value) = cmd.Transaction <- value

    interface IDisposable with
        member x.Dispose() = cmd.Dispose()

type public DynamicSqlConnection(connection:IDbConnection) =
    let mutable _transaction:IDbTransaction=null
    member private x.Connection = connection
    member private x.Transaction
        with get() = _transaction
        and set(value) = _transaction <- value
    
    static member (?) (self:DynamicSqlConnection, query:string) = 
        let command = self.Connection.CreateCommand()
        command.Transaction <- self.Transaction
        command.CommandType <- CommandType.Text
        command.CommandText <- query
        new DynamicSqlCommand(command)
    
    member x.Open() = connection.Open()
    member x.Close() = connection.Close()
    member x.BeginTransaction() = 
        x.Transaction <- connection.BeginTransaction()
        x.Transaction

    interface IDisposable with
        member x.Dispose() = connection.Dispose()

    static member CreateConnectionFromConfig(configName:string) =
        let connectionString = ConfigurationManager.ConnectionStrings.Item(configName)
        if connectionString = null then raise(new ArgumentException("Invalid connection string: " + configName))
        let factory = DbProviderFactories.GetFactory(connectionString.ProviderName)
        let connection = factory.CreateConnection()
        connection.ConnectionString <- connectionString.ConnectionString
        let result = new DynamicSqlConnection(connection:>IDbConnection)
        result.Open()
        result



    static member CreateConnection(connectionString:string, providerName:string) =
        let factory = DbProviderFactories.GetFactory(providerName)
        let connection = factory.CreateConnection()
        connection.ConnectionString <- connectionString
        connection:>IDbConnection  
        let result = new DynamicSqlConnection(connection:>IDbConnection)
        result.Open()
        result










