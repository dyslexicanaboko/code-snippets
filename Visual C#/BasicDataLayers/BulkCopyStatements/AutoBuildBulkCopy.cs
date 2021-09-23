using BasicDataLayers.DynamicStatements;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace BasicDataLayers.BulkCopyStatements
{
    public class AutoBuildBulkCopy
        : AutoBuildSqlBase
    {
        public void BulkInsert<T>(IList<T> source, string schema, string tableName, string primaryKey)
        {
            var t = typeof(T);

            var properties = GetProperties(t, primaryKey);

            var dt = ToDataTable(source, properties);

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

            var properties = GetProperties(t, primaryKey);

            var dt = ToDataTable(source, properties);

            var sqlTemplate = GetUpdateTemplate(schema, tableName, primaryKey, properties);

            using (var connection = new SqlConnection(ConnectionString))
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

        private SqlParamList GetUpdateTemplate(string schema, string tableName, string primaryKey, PropertyInfo[] properties)
        {
            //Column set count plus PK
            var arr = new SqlParameter[properties.Length + 1];

            var lstSetCols = new List<string>(properties.Length);

            string sqlVariable;

            for (var c = 0; c < properties.Length; c++)
            {
                sqlVariable = $"@c{c}";

                var colProperty = properties[c];

                lstSetCols.Add($"{colProperty.Name} = {sqlVariable}");

                var p = GetParam(colProperty, sqlVariable);

                arr[c] = p;
            }

            sqlVariable = "@pk";

            var pk = GetPrimaryKey(properties, primaryKey);

            arr[properties.Length] = GetParam(pk, sqlVariable);

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
