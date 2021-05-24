<Query Kind="Program">
  <Reference>C:\Dev\Linqpad workspace\Compare TFS vars to Web.config\Newtonsoft.Json.dll</Reference>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
</Query>

string BASE_PATH;
string WEB_CONFIG_FOLDER;
string TFS_JSON;
string OUTPUT;

/*
	Use this script to read the JSON files that are exported from TFS
	It contains ALL environments in it with the variables we care about
*/
void Main()
{
	BASE_PATH = Path.GetDirectoryName(Util.CurrentQueryPath);
	WEB_CONFIG_FOLDER = Path.Combine(BASE_PATH, "Prod Web configs");
	TFS_JSON = Path.Combine(BASE_PATH, "TFS variables");
	OUTPUT = Path.Combine(BASE_PATH, "Output");
	
	AnalyzeTfsVsWcFiles();
}

public void AnalyzeTfsVsWcFiles()
{
	//1. Get all Json files
	var lstTfsFiles = GetFileNames(TFS_JSON, "*.json");

	//2. Get all Web Config files
	var lstWcFiles = GetFileNames(WEB_CONFIG_FOLDER, "*.config");

	var r = new Regex("FS - ([A-Za-z ]+)\\.json");

	Console.WriteLine($"Number of JSON files: {lstTfsFiles.Count}\n\n");

	//3. Loop through the Json files
	foreach (var tfs in lstTfsFiles)
	{
		//4. Get the division name via RegEx
		var division = r.Match(tfs).Groups[1].Value.Replace(" ", string.Empty);
		
		//5. Lookup the associated Web Config
		var wc = lstWcFiles.FirstOrDefault(x => x.Contains(division));

		//If there is no matching Web Config then skip this one
		if (wc == null)
		{
			Console.WriteLine($"Skipping {wc} - could not complimentary Web.config");
			
			continue;
		}

		Console.WriteLine($"{division}, {tfs} = {wc}");

		//6. Run process
		var lstTfs = GetVariablesFromTfs(tfs);

		var lstWc = GetVariablesFromWebConfig(wc, lstTfs);

		var fn = division + ".txt";

		//7. Save files using Division name
		SaveResults(lstTfs, "TFS", fn);

		SaveResults(lstWc, "WC", fn);
	}
}

public List<string> GetFileNames(string path, string pattern)
{
	return new DirectoryInfo(path)
		.GetFiles(pattern, SearchOption.TopDirectoryOnly)
		.Select(x => x.Name)
		.ToList();
}

#region constants
string[] _jsonBlackList = new string[]
{
	"IISHostName",
	"IISPhysicalPath",
	"IISPort",
	"SiteName",
	"TempDestinationPath",
	"WebServers"
};

string[] _rootVariables = new string[]
{
	"BasicHttpBinding_WcfService"
};
#endregion

public void SaveResults(List<Kvp> data, string container, string saveAsFileName)
{
	var sb = new StringBuilder();

	foreach (var d in data)
		sb.Append(d.Key).Append("|").AppendLine(d.Value);
		
	string strPath = Path.Combine(OUTPUT, container);
	
	if(!Directory.Exists(strPath))
		Directory.CreateDirectory(strPath);
		
	File.WriteAllText(Path.Combine(strPath, saveAsFileName), sb.ToString());	
}

public List<Kvp> GetVariablesFromWebConfig(string divisionWebConfigFileName, IList<Kvp> tfsVariables)
{
	var element = XElement.Load(Path.Combine(WEB_CONFIG_FOLDER, divisionWebConfigFileName));

	var lst = new List<Kvp>();
	lst.AddRange(GetAppSettings(element));
	lst.AddRange(GetClientEndpoints(element));
	lst.Add(GetEinManager(element));
	lst.Add(GetAuthenticationName(element));
	lst.Add(GetConnectionString(element));
	lst.Add(GetInitializeDataValue(element));

	//These are the only permitted variables (for now)
	var whiteListOfKeys = tfsVariables.Select(x => x.Key).ToList();

	//Removing all keys that don't show up from Tfs
	for (int i = lst.Count - 1; i >= 0; i--)
	{
		var k = lst[i];
		
		//If the lst of keys contains this key, then set the order
		if (whiteListOfKeys.Contains(k.Key))
		{
			var o = tfsVariables.FirstOrDefault(x => x.Key == k.Key);
			
			if(o == null)
				continue;
				
			//Set the order so that these are in sync	
			k.Order = o.Order;
		}
		else //If the key was not found then remove this entry
			lst.RemoveAt(i);
	}

	lst = lst.OrderBy(x => x.Order).ToList();
	
	return lst;
}

public Kvp GetInitializeDataValue(XElement element)
{
	//Because the xmlns attribute is present, this has to be treated differently.
	XElement e = element.XPathSelectElements("system.diagnostics/sources/source/listeners/*")
	.First();

	return new Kvp
	{
		Key = "initializeData",
		Value = e.Attribute(XName.Get("initializeData")).Value
	};
}

public Kvp GetAuthenticationName(XElement element)
{
	//Because the xmlns attribute is present, this has to be treated differently.
	XElement e = element.XPathSelectElements("system.web/authentication/*")
	.First();

	return new Kvp
	{
		Key = "AuthFormsName",
		Value = e.Attribute(XName.Get("name")).Value
	};
}

public Kvp GetEinManager(XElement element)
{
	//Because the xmlns attribute is present, this has to be treated differently.
	XElement e = element.XPathSelectElements("/*[name()='unity']/*[name()='container']/*")
	.First(x => x.Attribute(XName.Get("type")).Value == "ClassName");

	return new Kvp
	{
		Key = "ClassName",
		Value = e.Attribute(XName.Get("mapTo")).Value
	};
}

public Kvp GetConnectionString(XElement element)
{
	//system.web/authentication[@mode='Forms']/forms/@name
	XElement e = element.XPathSelectElements("connectionStrings/*")
	.First(x => x.Attribute(XName.Get("name")).Value == "Entities");

	return new Kvp
	{
		Key = "Entities",
		//For some reason the quotes don't remain encoded... not sure why so I am forcing it back
		Value = e.Attribute(XName.Get("connectionString")).Value.Replace("\"", "&quot;")
	};
}

public List<Kvp> GetClientEndpoints(XElement element)
{
	return element.XPathSelectElements("system.serviceModel/client/*")
	.Select(x => new Kvp
	{
		Key = x.Attribute(XName.Get("name")).Value.Replace(".", "_"),
		Value = x.Attribute(XName.Get("address")).Value
	})
	.ToList();
}

public List<Kvp> GetAppSettings(XElement element)
{
	return element.XPathSelectElements("appSettings/*")
	.Select(x => new Kvp { 
		Key = x.Attribute(XName.Get("key")).Value, 
		Value = x.Attribute(XName.Get("value")).Value })
	.ToList();
}

public List<Kvp> GetVariablesFromTfs(string divisionJsonFileName)
{
	JObject jr = ReadTfsFile(Path.Combine(TFS_JSON, divisionJsonFileName));
	
	List<Kvp> lst = GetEnvironmentVariables(jr, "Production");
	
	int i = lst.Max(x => x.Order) + 1; //Inrement already
	
	lst.AddRange(GetRootVariables(i, jr));
	
	//Remove everything that shows up in the black list
	lst.RemoveAll(x => _jsonBlackList.Contains(x.Key));
	
	lst = lst.OrderBy(x => x.Order).ToList();
	
	return lst;
}

public List<Kvp> GetRootVariables(int startIndex, JObject rootNode)
{
	var vars = rootNode["variables"];
	
	var lst = new List<Kvp>();

	foreach (string key in _rootVariables)
	{
		JToken t = ((JObject)vars[key]).Property("value").Value;
		
		lst.Add(new Kvp
		{
			Order = startIndex++,
			Key = key,
			Value = t.Value<string>()
		});
	}

	return lst;
}

public List<Kvp> GetEnvironmentVariables(JObject rootNode, string environmentName)
{
	var prod = rootNode["environments"]
		.FirstOrDefault(x => x["name"].Value<string>() == environmentName);

	var lst = new List<Kvp>();

	int i = 0;

	foreach (JProperty p in prod["variables"].Children())
	{
		JToken t = ((JObject)p.Value).Property("value").Value;

		lst.Add(new Kvp
		{
			Order = i++,
			Key = p.Name,
			Value = t.Value<string>()
		});
	}
	
	return lst;
}

public class Kvp
{
	public int Order { get; set; }
	public string Key { get; set; }
	public string Value { get; set; }
}

public JObject ReadTfsFile(string fullFilePath)
{
	return JObject.Parse(File.ReadAllText(fullFilePath));
}

