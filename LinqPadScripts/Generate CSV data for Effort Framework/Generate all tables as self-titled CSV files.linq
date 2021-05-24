<Query Kind="Program">
  <Connection>
    <ID>eac9ecfc-eff2-4a24-81af-e3ace612cbda</ID>
    <Server>.</Server>
    <IsProduction>true</IsProduction>
    <Database>EmployeeDefense</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

/* Convert table row data into self-titled files as CSV files.
 * This is for the Effort Framework to consume https://entityframework-effort.net/ 
 * The goal is to keep it SIMPLE. */
string BasePath = Path.GetDirectoryName(Util.CurrentQueryPath);
string CsvFiles = @"C:\Dump\CsvFiles\";
const string D = ","; //Delimiter 

void Main()
{
	Directory.CreateDirectory(CsvFiles);

	//var lstTables = GetTables();
	var lstTables = new List<TableInfo> 
	{ 
		new TableInfo("dbo", "EventLogParticipant") 
	};
	
	GenerateCsvFiles(lstTables);
}

public void GenerateCsvFiles(List<TableInfo> tables)
{
	foreach (var t in tables)
	{
		GenerateCsvFile(t);
		
		Console.WriteLine();
	}
}

public void GenerateCsvFile(TableInfo tableInfo)
{
	var dt = GetData(tableInfo.QualifiedSchemaAndTable);

	Console.WriteLine($"Table: {tableInfo.QualifiedSchemaAndTable} -> Rows: {dt.Rows.Count:n0}");

	string[] arr = dt.Columns
		.Cast<DataColumn>()
		.Select(x => x.ColumnName)
		.ToArray();

	var columns = string.Join(D, arr);
	
	var sb = new StringBuilder();

	sb.AppendLine(columns);

	foreach (var row in dt.Select())
	{
		for (var c = 0; c < dt.Columns.Count; c++)
		{
			var str = FormatData(row[c]);
			
			sb.Append(str).Append(D);
		}
		
		//Remove trailing comma
		sb.Remove(sb.Length - 1, 1);
		
		sb.AppendLine();
	}
	
	var text = sb.ToString();

	var saveAs = Path.Combine(CsvFiles, tableInfo.TableName + ".csv");

	Console.WriteLine($"\tSaved as: {saveAs} -> Bytes: {text.Length:n0}");

	File.WriteAllText(saveAs, text, Encoding.ASCII);
}

public string FormatData(object columnData)
{
	//If it is DB NULL then return null
	if (columnData == DBNull.Value)
		return null;

	//Convert the data to the string equivalent
	var str = Convert.ToString(columnData);

	//If the data converted to string is whitespace don't continue
	if (string.IsNullOrWhiteSpace(str))
		return str;

	//If the original data is NOT a string don't continue
	if (!(columnData is string))
		return str;

	//It appears that hard quotes don't need to be escaped for Effort

	//If the string contains soft quotes they need to be escaped
	if (str.Contains("\""))
		str = str.Replace("\"", "\"\"");

	//If the string contains a back slash it needs to be escaped - this is an effort specific problem
	if (str.Contains("\\"))
		str = str.Replace("\\", "\\\\");

	//If the string does not contain the delimeter or new line characters, then it doesn't need to be text qualified
	if (!str.Contains(D) && !str.Contains("\n") && !str.Contains("\r"))
		return str;

	//If the delimiter is found then the data has to be text qualified
	str = $"\"{str}\"";
	
	return str;
}

public List<TableInfo> GetTables()
{
	using (var r = ExecuteReader("GetTables.sql"))
	{
		var lst = new List<TableInfo>();
		
		while (r.Read())
		{
			var m = new TableInfo();

			m.SchemaName = Convert.ToString(r["SchemaName"]);
			m.TableName = Convert.ToString(r["TableName"]);
			m.QualifiedSchemaAndTable = Convert.ToString(r["QualifiedSchemaAndTable"]);
			
			lst.Add(m);
		}
		
		return lst;
	}
}

public SqlDataReader ExecuteReader(string queryFileName)
{
	var connection = new SqlConnection(this.Connection.ConnectionString);

	connection.Open();

	var query = GetQuery(queryFileName);

	var command = new SqlCommand(query, connection);

	command.CommandType = CommandType.Text;

	var dr = command.ExecuteReader(CommandBehavior.CloseConnection);
	
	return dr;
}

private string GetQuery(string fileName)
{
	var text = File.ReadAllText(Path.Combine(BasePath, fileName));
	
	return text;
}

private DataTable GetData(string schemaAndTableName)
{
	var dt = new DataTable(schemaAndTableName);

	using (var connection = new SqlConnection(this.Connection.ConnectionString))
	{
		connection.Open();

		var query = "SELECT * FROM " + schemaAndTableName;

		try
		{	        
			using (var command = new SqlCommand(query, connection))
			{
				using (var da = new SqlDataAdapter(command))
				{
					da.Fill(dt);
				}
			}
		}
		catch
		{
			Console.WriteLine($"Query -> {query}");
			
			throw;
		}
	}

	return dt;
}

public class TableInfo
{
	public TableInfo()
	{
	
	}
	
	public TableInfo(string schemaName, string tableName)
	{
		SchemaName = schemaName;
		TableName = tableName;
		QualifiedSchemaAndTable = $"[{schemaName}].[{tableName}]";
	}
	
	public string SchemaName { get; set; }
	public string TableName { get; set; }
	public string QualifiedSchemaAndTable { get; set; }
}