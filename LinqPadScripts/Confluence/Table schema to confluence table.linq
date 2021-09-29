<Query Kind="Program" />

void Main()
{
	var dict = GetTables();
	
	GetConfluenceTables(dict);
}

public void GetConfluenceTables(Dictionary<string, List<Schema>> schemas)
{
	var sb = new StringBuilder();
	
	foreach (var kvp in schemas)
	{
		sb.AppendLine($"h4. {kvp.Key}")
		  .AppendLine()
		  .AppendLine("||Column||Type||Size||Nullable||");
		
		foreach (var row in kvp.Value)
		{
			sb.Append("|").Append(row.Column)
			.Append("|").Append(row.Type)
			.Append("|").Append(string.IsNullOrEmpty(row.Size) ? " " : row.Size)
			.Append("|").Append(row.Nullable)
			.Append("|")
			.AppendLine();
		}
		
		sb.AppendLine();
	}
	
	var content = sb.ToString();
	
	content.Dump();
}

public Dictionary<string, List<Schema>> GetTables()
{
	using (var con = new SqlConnection(@"Server=.;Database=DatabaseHere;Integrated Security=SSPI;"))
	{
		con.Open();
		
		using (var cmd = new SqlCommand(_query, con))
		{
			using (var dr = cmd.ExecuteReader())
			{
				var dict = new Dictionary<string, List<Schema>>();
				
				while (dr.Read())
				{
					var m = new Schema
					{
						Table = Convert.ToString(dr["TableName"]),
						Column = Convert.ToString(dr["ColumnName"]),
						Type = Convert.ToString(dr["Type"]),
						Size = Convert.ToString(dr["Size"]),
						Nullable = Convert.ToBoolean(dr["Nullable"])
					};

					if (dict.ContainsKey(m.Table))
					{
						dict[m.Table].Add(m);
					}
					else
					{
						dict.Add(m.Table, new List<Schema> { m });
					}
				}
				
				return dict;
			}
		}
	}
}

public class Schema
{
	public string Table { get; set; }
	
	public string Column { get; set; }

	public string Type { get; set; }

	public string Size { get; set; }
	
	public bool Nullable { get; set; }
}

const string _query = @"
SELECT
	   s.name AS SchemaName
	  ,t.name AS TableName
      ,c.name AS ColumnName
	  ,y.name AS [Type]
	  ,CASE c.user_type_id 
		WHEN 36 THEN ''
		WHEN 42 THEN CAST(c.scale AS VARCHAR(10))
		WHEN 56 THEN ''
		WHEN 104 THEN ''
		WHEN 231 THEN CAST(c.max_length/2 AS VARCHAR(10))
		ELSE CAST(c.max_length AS VARCHAR(10))
		END AS [Size]
	  ,c.is_nullable AS Nullable
FROM sys.schemas s
	INNER JOIN sys.tables t
		ON t.schema_id = s.schema_id
	INNER JOIN sys.columns c
		ON c.object_id = t.object_id
	INNER JOIN sys.types y
		ON c.user_type_id = y.user_type_id
WHERE s.name = 'dbo'
ORDER BY
	  t.name
";