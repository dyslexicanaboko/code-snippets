<Query Kind="Program" />

string BASE_PATH;
string BATCH_FILES;
string DESTINATION_WEBSITE_PATH = @"\\{server}\E$\Applications\{client}\";
string APP_OFFLINE_HTM = "app_offline.htm";
string APP_OFFLINE_HTM_PATH = @"C:\Workspace\app_offline.htm";

void Main()
{
	BASE_PATH = Path.GetDirectoryName(Util.CurrentQueryPath);
	BATCH_FILES = Path.Combine(BASE_PATH, "Batch files");
	
	List<ServerAddress> lstServers = LoadServers();
	List<string> lstClients = LoadClients();

	foreach(string client in lstClients)
		CreateCmdScripts(client, lstServers);
}

public void CreateCmdScripts(string targetDivision, List<ServerAddress> servers)
{
	var sbC = new StringBuilder();
	var sbD = new StringBuilder();
	Script s = null;

	string strServerIsBlank = DESTINATION_WEBSITE_PATH.Replace("{client}", targetDivision);

    foreach (ServerAddress sa in servers)
	{
		s = GetCmdScript(strServerIsBlank, sa);

		sbC.AppendLine(s.Copy);
		sbD.AppendLine(s.Delete);
	}

	File.WriteAllText(Path.Combine(BATCH_FILES, targetDivision + " - take offline.bat"), sbC.ToString());
	File.WriteAllText(Path.Combine(BATCH_FILES, targetDivision + " - bring back online.bat"), sbD.ToString());

	sbC.AppendLine();
	sbC.Append(sbD);

	sbC.ToString().Dump();
}

public Script GetCmdScript(string formatString, ServerAddress serverAddress)
{
	string strDest = formatString.Replace("{server}", serverAddress.Name);
	string strDel = Path.Combine(strDest, APP_OFFLINE_HTM);
	
	return new Script()
	{
		Copy = $"XCOPY /Y \"{APP_OFFLINE_HTM_PATH}\" \"{strDest}\"", 
		Delete = $"DEL \"{strDel}\""
	};
}

public List<string> LoadClients()
{
	return new List<string>(File.ReadAllLines(Path.Combine(BASE_PATH, "TargetClients.txt")));
}

public List<ServerAddress> LoadServers()
{
	string[] arrLines = File.ReadAllLines(Path.Combine(BASE_PATH, "TargetServers.txt"));

	var lst = new List<ServerAddress>();

	string[] arr = null;	

	foreach (string line in arrLines)
	{
		arr = line.Split(',');
		
		lst.Add(new ServerAddress() {
			Name = arr[0],
			IP4 = arr[1]
		});
	}
	
	return lst;
}

// Define other methods and classes here
public class ServerAddress
{
	public string Name { get; set; }
	public string IP4 { get; set; }
}

public class Script
{
	public string Copy { get; set; }
	public string Delete { get; set; }
}