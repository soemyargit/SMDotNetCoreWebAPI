namespace SMDotNetCoreADO.Configurations
{
    public class DbConfig
    {
        public static string DbConnection { get; } =
        @"Server=Soe\mssql;Database=testDb;User ID=sa;Password=sasa@123;TrustServerCertificate=True;";
    }
}
