using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicDataLayers.DynamicStatements
{
    public class AutoBuildDml
        : AutoBuildSqlBase
    {
        public SqlParamList GetSelectByPrimaryKeySql<T>(T target, string schema, string tableName, string primaryKey)
            where T : new()
        {
            var t = target.GetType();

            var properties = GetProperties(t);

            var lstSetCols = properties.Select(p => p.Name).ToList();

            var pk = GetPrimaryKey(t, primaryKey);

            var pkVariable = $"@{pk.Name}";

            var pkParam = GetParam(pk, pkVariable, target);

            var selectList = string.Join("," + Environment.NewLine, lstSetCols);

            var sql =
                "SELECT" + Environment.NewLine +
                selectList + Environment.NewLine +
                $"FROM {schema}.{tableName}" + Environment.NewLine +
                $"WHERE {pk.Name} = {pkVariable}";

            var values = new SqlParamList
            {
                Sql = sql,
                Parameters = new [] {pkParam}
            };

            return values;
        }

        public SqlParamList GetSelectAllSql<T>(T target, string schema, string tableName)
            where T : new()
        {
            var t = target.GetType();

            var properties = GetProperties(t);

            var lstSetCols = properties.Select(p => p.Name).ToList();

            var selectList = string.Join("," + Environment.NewLine, lstSetCols);

            var sql =
                "SELECT" + Environment.NewLine +
                selectList + Environment.NewLine +
                $"FROM {schema}.{tableName}";

            var values = new SqlParamList
            {
                Sql = sql,
                Parameters = new SqlParameter[0]
            };

            return values;
        }

        public SqlParamList GetInsertSql<T>(IList<T> source, string schema, string tableName, string primaryKey)
            where T : new()
        {
            var t = source.First().GetType();

            var properties = GetProperties(t, primaryKey);

            var columnNames = properties.Select(x => x.Name).ToArray();

            var parameterList = string.Join(", ", columnNames);

            var arr = new SqlParameter[source.Count * columnNames.Length];

            var sb = new StringBuilder();

            sb.AppendLine($"INSERT INTO {schema}.{tableName} ({parameterList}) VALUES ");

            var pCount = 0;

            for (var r = 0; r < source.Count; r++)
            {
                var row = source[r];

                sb.Append("(");

                for (var c = 0; c < properties.Length; c++)
                {
                    var colProperty = properties[c];
                    var colType = colProperty.PropertyType;

                    var sqlVariable = $"@r{r}c{c}";

                    sb.Append(sqlVariable).Append(",");

                    var p = new SqlParameter();
                    p.ParameterName = sqlVariable;
                    p.SqlDbType = TypeMap[colType];
                    p.Value = colProperty.GetValue(row);

                    arr[pCount] = p;

                    pCount++;
                }
                
                //Trim trailing comma
                sb.Remove(sb.Length - 1, 1);

                sb.AppendLine("),");
            }

            //Trim trailing comma and newline \r\n
            sb.Remove(sb.Length - 3, 3);

            var sql = sb.ToString();

            var values = new SqlParamList
            {
                Sql = sql,
                Parameters = arr
            };

            return values;
        }

        public SqlParamList GetUpdateSql<T>(T target, string schema, string tableName, string primaryKey)
            where T : new()
        {
            var t = target.GetType();

            var properties = GetProperties(t, primaryKey);

            //Column set count plus PK
            var arr = new SqlParameter[properties.Length + 1];

            var lstSetCols = new List<string>(properties.Length);

            string sqlVariable;

            for (var c = 0; c < properties.Length; c++)
            {
                sqlVariable = $"@c{c}";

                var colProperty = properties[c];

                lstSetCols.Add($"{colProperty.Name} = {sqlVariable}");

                var p = GetParam(colProperty, sqlVariable, target);

                arr[c] = p;
            }

            sqlVariable = "@pk";

            var pk = GetPrimaryKey(t, primaryKey);

            arr[properties.Length] = GetParam(pk, sqlVariable, target);

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
