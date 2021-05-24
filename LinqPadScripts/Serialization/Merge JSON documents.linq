<Query Kind="Program">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
</Query>

void Main()
{
	var lstDef = GetAllJobDefinitions();

	foreach (var def in lstDef)
	{
		try
		{	        
			Console.WriteLine(def.Description);
			
			foreach (var defInst in def.DefinitionInstances)
			{
				Merge(def, defInst);
			}
	
			//Save per definition just in case there is a failure
			SubmitChanges();
	
			Console.WriteLine($"Instances updated {def.DefinitionInstances.Count}");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Failure for DefinitionsId {def.DefinitionsId}{Environment.NewLine}{ex.ToString()}");
		}
		
		Console.WriteLine();
	}
}

private void Merge(Definitions definition, DefinitionInstances definitionInstance)
{
	var d = JObject.Parse(definition.Parameters);
	var di = JObject.Parse(definitionInstance.Values);
	
	var mergedJson = Merge(d, di);
	
	definitionInstance.Values = mergedJson;
	definitionInstance.ModifiedOnUtc = DateTime.UtcNow;
}

private string Merge(JObject master, JObject target)
{
	var s = new JsonMergeSettings 
	{ 
		MergeNullValueHandling = MergeNullValueHandling.Merge, 
		MergeArrayHandling = MergeArrayHandling.Merge 
	};
	
	master.Merge(target, s);
	
	var strJson = master.ToString(Newtonsoft.Json.Formatting.None);
	
	return strJson;
}

private void TestMerge()
{
	//var jDef = JObject.Parse(@"{'Client':'System.String','ImportPath':'System.String','FilePatterns':[{'FtpServersId':'System.Int32','RemotePath':'System.String','FilePattern':'System.String','IsRequired': 'System.Boolean','CompressionFolderName':'System.String'}],'ArchivePolicy': {'FtpServersId': 'System.Int32', 'RemotePath': 'System.String', 'IsCompressed': 'System.Boolean','CompressionMode':'System.String','CompressionFolderName':'System.String'},'NotificationPreferences': {'NotifyOnSuccess': 'System.Boolean','ContactList': [{'Email': 'System.String'}]}}");
	var jDef = JObject.Parse(@"{'Client':'System.String','FilePatterns':[{'FtpServersId':'System.Int32','RemotePath':'System.String','FilePattern':'System.String','IsRequired': 'System.Boolean','CompressionFolderName':'System.String','NewProperty':'System.String'}],'ContactList': [{'Email': 'System.String'}, {'Email': 'System.String'}, {'Email': 'System.String'}],'Property2':'System.Int32'}");
	var jValue = JObject.Parse(@"{'Client':'ClientA','FilePatterns':[{'FtpServersId':12,'RemotePath':'/ta/import/','FilePattern':'Blah*.txt','IsRequired': 'true','CompressionFolderName':'SomeFolder'}],'ContactList': [{'Email': 'e@a.com'}, {'Email': 'eli@gmail.com'}]}");
	//var jValue = JObject.Parse(@"{'Client':'ClientB','ImportPath':'C:\\Data\\FolderA','FilePatterns':[{'FtpServersId':11,'RemotePath':'/FS/Census/CAPITAL','FilePattern':'FileA*.txt','IsRequired': true}],'ArchivePolicy': {'FtpServersId': '11', 'RemotePath': '/archivePath/', 'IsCompressed': true },'NotificationPreferences': {'NotifyOnSuccess': false,'ContactList': [{'Email': 'e3@e3.com'},{'Email': 'e2@e2.com'},{'Email': 'e1@e1.com'}]}}");

	//Traverse(jDef);
	//The trick here is that the order of the merge matters.
	//The definition is merging the value into itself, so the value takes precedence over the definition which is perfect
	Merge(jDef, jValue);
	//Merge(jValue, jDef);
}

private List<Definitions> GetAllJobDefinitions()
{
	var lst = Definitions
		//.Where(x => x.DefinitionsId == 1)
		.ToList();

	return lst;
}