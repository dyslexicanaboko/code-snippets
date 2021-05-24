<Query Kind="Program">
  <Connection>
    <ID>eac9ecfc-eff2-4a24-81af-e3ace612cbda</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <IsProduction>true</IsProduction>
    <Database>EmployeeDefense</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

/* In some cases a table cannot be altered and needs to be dropped and recreated.
   Unfortunately the stock VS compare really sucks and RedGate compare costs
   like a million dollars. This script helps template out the basics of storing
   existing data into a temporary table, dropping the table and re-inserting. 
   However this is just a TEMPLATE meaning you still have to review it before
   running it. */
void Main()
{
	BasePath = Path.GetDirectoryName(Util.CurrentQueryPath);
	SqlTemplate = LoadTemplate("sqlTemplate.sql");

	var t = new TableInfo
	{
		SchemaName = "dbo",
		TableName = "AppSettingsConfig"
	};
	
	//Make sure to connect a database first
	t.TableSchema = GetSchema(t.GetFullTableName());
	
	//This is what the table is composed of
	t.TableSchema.Columns.Dump();
	
	ComposeTransferScript(t).Dump();
}

string BasePath;
string SqlTemplate;
const string TableSchema = "<TABLE_SCHEMA>";
const string TableName = "<TABLE_NAME>";
const string ColumnsNew = "<COLUMNS_NEW>";
const string ColumnsOld = "<COLUMNS_OLD>";

private string LoadTemplate(string fileName)
{
	var path = Path.Combine(BasePath, fileName);
	
	var str = File.ReadAllText(path);
	
	return str;
}

private string ComposeTransferScript(TableInfo tableInfo)
{
	var ti = tableInfo;
	var d = "," + Environment.NewLine;

	//Get column names
	var colOld = string.Join(d, 
		ti.TableSchema
			.Columns
			.Cast<DataColumn>()
			.Select(x => "[" + x.ColumnName + "]")
			.ToArray());

	var strFilledOut = SqlTemplate
		.Replace(TableSchema, ti.SchemaName)
		.Replace(TableName, ti.TableName)
		.Replace(ColumnsNew, colOld)
		.Replace(ColumnsOld, colOld);
	
	return strFilledOut;
}

// Define other methods and classes here
private DataTable GetSchema(string schemaAndTableName)
{
	var dt = new DataTable(schemaAndTableName);
	
	using (var connection = new SqlConnection(this.Connection.ConnectionString))
	{
		connection.Open();

		//Don't pull any data on purpose
		var query = "SELECT TOP 0 * FROM " + schemaAndTableName;

		using (var command = new SqlCommand(query, connection))
		{
			using (var da = new SqlDataAdapter(command))
			{
				da.Fill(dt);
			}
		}
	}
	
	return dt;
}

public class TableInfo
{
	public string SchemaName { get; set; }
	
	public string TableName { get; set; }
	
	public DataTable TableSchema { get; set; }
	
	public string GetFullTableName()
	{
		var t = SchemaName + "." + TableName;
		
		return t;
	}
}