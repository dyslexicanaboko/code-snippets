using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BasicDataLayers
{
    /// <summary>
    /// The Base DAL only exists to provide access to the connection string and for basic ADO.NET functionality
    /// to the other Data Layer implementations.
    /// NuGet Dependencies:
    ///     Microsoft.Data.SqlClient, Version="3.0.0"
    ///     Microsoft.Extensions.Configuration, Version="5.0.0"
    ///     Microsoft.Extensions.Configuration.FileExtensions, Version="5.0.0"
    ///     Microsoft.Extensions.Configuration.Json, Version="5.0.0"
    /// </summary>
    public abstract class BaseDal
    {
        protected readonly string ConnectionString;

        protected BaseDal()
        {
            ConnectionString = LoadConnectionString();
        }

        private static string LoadConnectionString()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = builder.Build();

            var connectionString = configuration.GetConnectionString("ScratchSpace");

            return connectionString;
        }
        
        protected void ExecuteNonQuery(string sql, SqlParameter[] parameters)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddRange(parameters);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
