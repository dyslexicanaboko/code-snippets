using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicDataLayers.DynamicStatements
{
    public class AutoBuildInsert
        : AutoBuildSqlBase
    {
        public SqlParamList GenerateSql<T>(IList<T> source, string schema, string tableName, string primaryKey)
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
    }
}
