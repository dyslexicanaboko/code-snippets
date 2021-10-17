using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Microsoft.Data.SqlClient;

namespace BasicDataLayers.Lib.DynamicStatements
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
            {typeof(TimeSpan), SqlDbType.Time},
            {typeof(Guid), SqlDbType.UniqueIdentifier}
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

        protected PropertyInfo[] GetProperties(IEnumerable<PropertyInfo> properties, string primaryKeyToExclude)
        {
            var pk = primaryKeyToExclude;

            if (pk != null)
            {
                properties = properties.Where(x => !x.Name.Equals(pk, StringComparison.InvariantCultureIgnoreCase));
            }

            var arr = properties
                .OrderBy(x => x.Name)
                .ToArray();

            return arr;
        }

        protected SqlParameter GetParam(PropertyInfo column, string parameterName, object objectSource)
        {
            var p = GetParam(column, parameterName);
            p.Value = column.GetValue(objectSource);

            return p;
        }

        protected SqlParameter GetParam(PropertyInfo column, string parameterName)
        {
            var p = new SqlParameter();
            p.ParameterName = parameterName;
            p.SqlDbType = TypeMap[column.PropertyType];

            return p;
        }

        public void ExecuteNonQuery(SqlParamList values)
        {
            ExecuteNonQuery(values.Sql, values.Parameters);
        }

        protected DataTable GetSchema(PropertyInfo[] properties)
        {
            var dt = new DataTable("Table1");

            foreach (var p in properties)
            {
                var dc = new DataColumn(p.Name, p.PropertyType);

                dt.Columns.Add(dc);
            }

            return dt;
        }

        protected DataTable ToDataTable<T>(IEnumerable<T> source, PropertyInfo[] properties, bool forUpdate)
        {
            var dt = GetSchema(properties);

            foreach (var row in source)
            {
                var dr = dt.NewRow();

                for (var c = 0; c < properties.Length; c++)
                {
                    var colProperty = properties[c];

                    dr[c] = colProperty.GetValue(row);
                }

                dt.Rows.Add(dr);
            }

            if (!forUpdate) return dt;
            
            //To avoid getting an error during update the following things have to happen to the
            //data table so that the Data Adapter doesn't think this is an insert operation
            dt.AcceptChanges();

            foreach (DataRow r in dt.Rows)
            {
                r.SetModified();
            }

            return dt;
        }

        protected IList<SqlBulkCopyColumnMapping> GetColumnMapping(PropertyInfo[] properties)
        {
            var lst = new List<SqlBulkCopyColumnMapping>(properties.Length);

            lst.AddRange(properties
                .Select(colProperty =>
                    new SqlBulkCopyColumnMapping(
                        colProperty.Name,
                        colProperty.Name)));

            return lst;
        }
    }
}
