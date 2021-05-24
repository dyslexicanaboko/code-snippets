<Query Kind="Program">
  <Connection>
    <ID>eac9ecfc-eff2-4a24-81af-e3ace612cbda</ID>
    <Server>.</Server>
    <IsProduction>true</IsProduction>
    <Database>EmployeeDefense</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <NuGetReference>CsvHelper</NuGetReference>
  <Namespace>CsvHelper</Namespace>
</Query>

void Main()
{
	var path = @"C:\Dev\GitHub\dyslexicanaboko.visualstudio.com\employee-defense\Dyslexic.EmployeeDefense.Test\EffortCsvData\";

	//Pull data raw from a CSV
	var lines = GetTokens(Path.Combine(path, "BillingCategory.csv"));
	
	SerializeDataAsCSharpObjectDeclarations(lines).Dump();	
}

public List<string[]> GetTokens(string csvPath, bool skipHeader = true)
{
	var lst = new List<string[]>();
	
	using (var sr = File.OpenText(csvPath))
	{
		//Using this CSV NuGet package because it handles my least favorite thing in the world "Qualified text"
		using (var reader = new CsvReader(sr, System.Globalization.CultureInfo.CurrentCulture))
		{
			reader.Configuration.HasHeaderRecord = skipHeader;

			while (reader.Read())
			{
				var arr = new[]
				{
					reader.GetField(0),
					reader.GetField(1),
					reader.GetField(2),
					reader.GetField(3),
					reader.GetField(4),
					reader.GetField(5),
					reader.GetField(6),
					reader.GetField(7)
				};
				
				lst.Add(arr);
			}
		}
	}

	if (!skipHeader) return lst;
	
	lst.RemoveAt(0);
	
	return lst;
}

/* 
	The problem with this idea is depending on the data input you don't know what the type is. 
	The only good news is you can map it once and produce the declarations you need	
	For the Effort Framework specifically you have to watch out for escape characters.
*/
public string SerializeDataAsCSharpObjectDeclarations(List<string[]> lines)
{
	/* Regex for stripping out everything except the property names: 
			public \w+ (\w+) { get; set; }
			$1 = ,
	*/
	var sb = new StringBuilder();

	sb.AppendLine("var lst = new List<BillingCategory>")
	  .AppendLine("{");

	foreach (var line in lines)
	{
		var t = line;
		
		sb.Append($@"new BillingCategory
		{{
			BillingCategoryId = {t[0]},
			Name = ""{t[1]}"",
			Description = ""{t[2]}"",
			IsEnabled = {t[3].ToLower()},
			UserId = {t[4]},
			CreatedOnUtc = DateTime.Parse(""{t[5]}""),
			Order = {t[6]},
			IsStock = {t[7].ToLower()}
		}},	
		");
	}

	sb.AppendLine("};");

	var content = sb.ToString();
	
	return content;
}

