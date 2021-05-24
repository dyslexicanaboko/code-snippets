<Query Kind="Program" />

string BASE_PATH;
string DataFolder = "Data";
string ServerInfoFile;
string ScriptTemplate = @"XCOPY /Y E:\Workspace\app_offline.htm \\{{SERVER}}\E$\Applications\{{CLIENT}}\";

void Main()
{
	BASE_PATH = Path.GetDirectoryName(Util.CurrentQueryPath);
	ServerInfoFile = Path.Combine(BASE_PATH, DataFolder, "ServerInfo.txt");
	
	var lst = GetData(ServerInfoFile);
	
	GenerateTakeDownScript(lst, "ClientA", "ClientB").Dump();
}

//Create a script to take down 
public string GenerateTakeDownScript(List<ServerInfo> divisionData, params string[] divisions)
{
	var sb = new StringBuilder();

	foreach (var division in divisions)
	{
		var lst = divisionData.FindAll(x => x.Client.Equals(division, StringComparison.CurrentCultureIgnoreCase));

		foreach (var s in lst)
		{
			sb.AppendLine(ScriptTemplate)
			  .Replace("{{SERVER}}", s.Web)
			  .Replace("{{CLIENT}}", s.Client);
		}
	}
	
	return sb.ToString();
}

public List<ServerInfo> GetData(string filePath)
{
	var lst = File.ReadAllLines(filePath).Select(x => {
		var arr = x.Split('|');

		return new ServerInfo()
		{
			Web = arr[0],
			Client = arr[1],
			Database = arr[2]
		};
	}).ToList();
	
	return lst;
}

// Define other methods and classes here
public class ServerInfo
{
	public string Client { get; set; }
	public string Web { get; set; }
	public string Database { get; set; }
}