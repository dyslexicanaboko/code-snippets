using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace BasicDataLayers.Lib.DynamicStatements
{
    public class AutoBuildBulkCopy
        : AutoBuildSqlBase
    {
        public void BulkInsert<T>(IList<T> source, string schema, string tableName, string primaryKey)
        {
            var t = typeof(T);

            var properties = GetProperties(t, primaryKey);

            var dt = ToDataTable(source, properties, false);

            var map = GetColumnMapping(properties);

            var fullTableName = $"{schema}.{tableName}";

            using (var bc = new SqlBulkCopy(ConnectionString))
            {
                bc.BatchSize = 5000;
                bc.DestinationTableName = fullTableName;

                foreach (var m in map)
                {
                    bc.ColumnMappings.Add(m);
                }

                bc.WriteToServer(dt);
            }
        }

        public void BulkUpdate<T>(IList<T> source, string schema, string tableName, string primaryKey)
        {
            var t = typeof(T);

            var properties = GetProperties(t);

            var dt = ToDataTable(source, properties, true);

            var sqlTemplate = GetUpdateTemplate(schema, tableName, primaryKey, properties);

            //This is a slow operation, so the connection and command wait time should be high
            var cb = new SqlConnectionStringBuilder(ConnectionString);
            cb.CommandTimeout = 0;
            cb.ConnectTimeout = 0;

            var cs = cb.ToString();

            using (var connection = new SqlConnection(cs))
            {
                using (var adapter = new SqlDataAdapter())
                {
                    adapter.UpdateBatchSize = 5000;
                    adapter.UpdateCommand = new SqlCommand(sqlTemplate.Sql, connection);
                    adapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
                    adapter.UpdateCommand.Parameters.AddRange(sqlTemplate.Parameters);

                    adapter.Update(dt);
                }
            }
        }

        private SqlParamList GetUpdateTemplate(string schema, string tableName, string primaryKey, PropertyInfo[] propertiesAll)
        {
            var arr = new SqlParameter[propertiesAll.Length];

            var propertiesNoPk = GetProperties(propertiesAll, primaryKey);

            var lstSetCols = new List<string>(propertiesAll.Length);

            string sqlVariable;

            for (var c = 0; c < propertiesNoPk.Length; c++)
            {
                var p = propertiesNoPk[c];

                sqlVariable = $"@{p.Name}";

                var colProperty = propertiesNoPk[c];

                lstSetCols.Add($"{colProperty.Name} = {sqlVariable}");

                var parameter = GetParam(colProperty, sqlVariable);
                parameter.SourceColumn = p.Name;
                
                arr[c] = parameter;
            }

            var pk = GetPrimaryKey(propertiesAll, primaryKey);
            
            sqlVariable = $"@{pk.Name}";

            var pkParameter = GetParam(pk, sqlVariable);
            pkParameter.SourceColumn = pk.Name;

            arr[propertiesAll.Length - 1] = pkParameter;

            var sets = string.Join("," + Environment.NewLine, lstSetCols);

            var sql =
                $"UPDATE {schema}.{tableName} SET " + Environment.NewLine +
                sets + Environment.NewLine +
                $"WHERE {pk.Name} = {sqlVariable}";

            var values = new SqlParamList
            {
                Sql = sql,
                Parameters = arr
            };

            return values;
        }
    }
}
