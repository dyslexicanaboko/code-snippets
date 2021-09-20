using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Microsoft.Data.SqlClient;

namespace BasicDataLayers.DynamicStatements
{
    public abstract class AutoBuildSqlBase
        : BaseDal
    {
        /// <summary>
        /// This list needs revision and needs to be tested for integrity. I threw it together, but I am
        /// sure it is flawed.
        /// </summary>
        protected static readonly Dictionary<Type, SqlDbType> TypeMap = new Dictionary<Type, SqlDbType>
        {
            {typeof(string), SqlDbType.NVarChar},
            {typeof(char[]), SqlDbType.NVarChar},
            {typeof(int), SqlDbType.Int},
            {typeof(short), SqlDbType.SmallInt},
            {typeof(long), SqlDbType.BigInt},
            {typeof(byte[]), SqlDbType.VarBinary},
            {typeof(bool), SqlDbType.Bit},
            {typeof(DateTime), SqlDbType.DateTime2},
            {typeof(DateTimeOffset), SqlDbType.DateTimeOffset},
            {typeof(decimal), SqlDbType.Decimal},
            {typeof(double), SqlDbType.Float},
            {typeof(byte), SqlDbType.TinyInt},
            {typeof(TimeSpan), SqlDbType.Time}
        };

        protected PropertyInfo GetPrimaryKey(Type type, string primaryKey)
        {
            return GetPrimaryKey(type.GetProperties(), primaryKey);
        }

        protected PropertyInfo GetPrimaryKey(PropertyInfo[] properties, string primaryKey)
        {
            var pk = properties
                .Single(x => x.Name.Equals(primaryKey, StringComparison.InvariantCultureIgnoreCase));

            return pk;
        }

        protected PropertyInfo[] GetProperties(Type type, string primaryKeyToExclude = null)
        {
            var pk = primaryKeyToExclude;

            var q = type.GetProperties().AsEnumerable();

            if (pk != null)
            {
                q = q.Where(x => !x.Name.Equals(pk, StringComparison.InvariantCultureIgnoreCase));
            }
            
            var arr = q
                .OrderBy(x => x.Name)
                .ToArray();

            return arr;
        }

        protected SqlParameter GetParam(PropertyInfo column, string parameterName, object objectSource)
        {
            var p = new SqlParameter();
            p.ParameterName = parameterName;
            p.SqlDbType = TypeMap[column.PropertyType];
            p.Value = column.GetValue(objectSource);

            return p;
        }

        public void ExecuteNonQuery(SqlParamList values)
        {
            ExecuteNonQuery(values.Sql, values.Parameters);
        }
    }
}
