<Query Kind="Program">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
</Query>

void Main()
{
	var jDef = JObject.Parse(@"{'Client':'System.String','FilePatterns':[{'FtpServersId':'System.Int32','RemotePath':'System.String','FilePattern':'System.String','IsRequired': 'System.Boolean','CompressionFolderName':'System.String'}],'ContactList': [{'Email': 'System.String'}, {'Email': 'System.String'}, {'Email': 'System.String'}]}");

	Traverse(jDef);
}

private void Traverse(JObject jObject)
{
	Traverse(jObject.Properties());
}

private void Traverse(IEnumerable<JProperty> properties)
{
	foreach (var p in properties)
	{
		Traverse(p);
	}
}

private void Traverse(JProperty property)
{
	Console.WriteLine($"K -> {property.Name}");

	Traverse(property.Value);
}

private void Traverse(JToken token)
{
	if (!token.HasValues)
	{
		Console.WriteLine($"V.T -> {token.ToString()}");

		return;
	}

	Traverse(token.Children());
}

private void Traverse(IEnumerable<JToken> tokens)
{
	foreach (var t in tokens)
	{
		if (t.HasValues)
		{
			Traverse(t);
			
			continue;
		}

		var p = t.Parent as JProperty;

		Traverse(p);
	}
}
