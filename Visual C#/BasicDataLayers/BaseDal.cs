using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
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

        public static object GetScalar(IDataReader reader, string column)
        {
            object obj = null;

            if (reader == null)
                return null;

            while (reader.Read())
                obj = reader[column];

            return obj;
        }

        protected static List<T> ToList<T>(IDataReader reader, Func<IDataReader, T> method) where T : new()
        {
            var lst = new List<T>();

            if (reader == null)
                return lst;

            while (reader.Read())
                lst.Add(method(reader));

            return lst;
        }

        protected IDataReader ExecuteReaderText(string sql, params SqlParameter[] parameters)
        {
            return ExecuteReader(CommandType.Text, sql, parameters);
        }

        protected IDataReader ExecuteReaderSproc(string sql, params SqlParameter[] parameters)
        {
            return ExecuteReader(CommandType.StoredProcedure, sql, parameters);
        }

        private IDataReader ExecuteReader(CommandType commandType, string sql, SqlParameter[] parameters)
        {
            var con = new SqlConnection(ConnectionString);

            con.Open();

            var cmd = new SqlCommand(sql, con);
            cmd.CommandType = commandType;
            cmd.Parameters.AddRange(parameters);

            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
    }
}
