using Microsoft.Data.SqlClient;

namespace BasicDataLayers.DynamicStatements
{
    /// <summary>
    /// Dynamically generated SQL accompanied by labeled and loaded SQL Parameter objects.
    /// Intention is to use this object together with a simple ADO.Net execution.
    /// </summary>
    public class SqlParamList
    {
        public string Sql { get; set; }

        public SqlParameter[] Parameters { get; set; }
    }
}
