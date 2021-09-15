<Query Kind="Program" />

void Main()
{
	var lst = GetColumns();

	var lstTables = GetTargets(lst);
	
	GenerateExtendedProperties(lstTables);
}

public void GenerateExtendedProperties(List<Target> tables)
{
	var sb = new StringBuilder();
	
	foreach (var t in tables)
	{
		var header = $"{t.Schema}.{t.Table}";
		var horizontalBreak = $"-- ".PadRight(header.Length * 2, '=');

		sb.AppendLine(horizontalBreak)
		  .AppendLine($"-- {t.Schema}.{t.Table}")
		  .AppendLine(horizontalBreak)
		  .AppendLine($"SET @table = N'{t.Table}';").AppendLine();
		  
		foreach (var column in t.Columns)
		{
			//Break down each column name into words if it is in pascal case
			var result = column
				.SelectMany((c, i) =>
					i != 0 &&
					char.IsUpper(c) &&
					!char.IsUpper(column[i - 1]) ? new char[] { ' ', char.ToLower(c) } : new char[] { c });
			
			var arr = result.ToArray();
			
			//Only capitalize the first word in the sentence
			arr[0] = char.ToUpper(arr[0]);

			//Better than nothing comment to start us off
			var comment = new string(arr);

			sb.AppendLine(
$@"EXEC [dbo].[DocumentationColumnUpsert]
	 @schema = @schema
	,@table = @table
	,@column = N'{column}'
	,@comments = N'{comment}';").AppendLine();
		}
	}
	
	var content = sb.ToString();
	
	content.Dump();
}

public List<Target> GetTargets(List<Flat> flatObjects)
{
	var lst = flatObjects
		.GroupBy(x => new { x.Schema, x.Table })
		.Select(x => new Target
		{
			Schema = x.Key.Schema,
			Table = x.Key.Table,
			Columns = x.Select(c => c.Column).ToList()
		}).ToList();
		
	return lst;
}

public List<Flat> GetColumns()
{
	using (var con = new SqlConnection(@"Server=.;Database=SomeDatabase;Integrated Security=SSPI;"))
	{
		con.Open();

		using (var cmd = new SqlCommand(_query, con))
		{
			using (var dr = cmd.ExecuteReader())
			{
				var lst = new List<Flat>();
				
				while (dr.Read())
				{
					//var obj = Convert.ToInt32(dr[""]);
					var t = new Target();

					var f = new Flat
					{
						Schema = Convert.ToString(dr["SchemaName"]),
						Table = Convert.ToString(dr["TableName"]),
						Column = Convert.ToString(dr["ColumnName"])
					};
					
					lst.Add(f);
				}
				
				return lst;
			}
		}
	}
}

public class Flat
{
	public string Schema { get; set; }

	public string Table { get; set; }

	public string Column { get; set; }
}

public class Target
{
	public string Schema { get; set; }
	
	public string Table { get; set; }
	
	public List<string> Columns { get; set; } = new List<string>();
}

public string _query = @"
	SELECT
		   s.name AS SchemaName
		  ,t.name AS TableName
	      ,c.name AS ColumnName
	FROM sys.schemas s
		INNER JOIN sys.tables t
			ON t.schema_id = s.schema_id
		INNER JOIN sys.columns c
			ON c.object_id = t.object_id
	WHERE s.name = 'dbo' AND t.name <> '__RefactorLog'
	ORDER BY
		  t.name
		 ,c.name
";