<Query Kind="Program" />

string BASE_PATH;
string TEMPLATE = "<File firstVisibleLine=\"0\" xOffset=\"0\" scrollWidth=\"2992\" startPos=\"0\" endPos=\"0\" selMode=\"0\" lang=\"XML\" encoding=\"-1\" filename=\"{0}\" backupFilePath=\"\" originalFileLastModifTimestamp=\"1498220393\" />";
string ContainerTemplate;

void Main()
{
	BASE_PATH = Path.GetDirectoryName(Util.CurrentQueryPath);
	ContainerTemplate = File.ReadAllText(Path.Combine(BASE_PATH, "ContainerTemplate.txt"));

	#region
	var pGroupABlue = new Profile
	{
		Group = "A",
		Color = "Blue",
		UncPathTemplate = @"\\{{SERVER}}\E$\Sites\{{CLIENT}}\Web.config",
		Servers = new List<string>
		{
			"ServerA",
			"ServerB",
			"ServerC"
		},
		Clients = new List<string>
		{
			"Client1",
			"Client2",
			"Client3"
		}
	};

	var pGroupBBlue = new Profile
	{
		Group = "B",
		Color = "Blue",
		UncPathTemplate = @"\\{{SERVER}}\E$\Sites\{{CLIENT}}\Web.config",
		Servers = new List<string>
		{
			"ServerA",
			"ServerB",
			"ServerC",
			"ServerD",
			"ServerF"
		},
		Clients = new List<string>
		{
			"Client4",
			"Client5",
			"Client6"
		}
	};

	var pGroupCBlue = new Profile
	{
		Group = "C",
		Color = "Blue",
		UncPathTemplate = @"\\{{SERVER}}\E$\Sites\{{CLIENT}}\Web.config",
		Servers = new List<string>
		{
			"ServerL",
			"ServerM",
			"ServerN",
			"ServerO",
			"ServerP"
		},
		Clients = new List<string>
		{
			"Client7",
			"Client8",
			"Client9"
		}
	};

	var pGroupDBlue = new Profile
	{
		Group = "D",
		Color = "Blue",
		UncPathTemplate = @"\\{{SERVER}}\E$\Sites\{{CLIENT}}\Web.config",
		Servers = new List<string>
		{
			"ServerA",
			"ServerB",
			"ServerC",
			"ServerD",
			"ServerF",
			"ServerL",
			"ServerM",
			"ServerN",
			"ServerO",
			"ServerP",
			"ServerA",
			"ServerB",
			"ServerC"
		},
		Clients = new List<string>
		{
			"FS-API"
		}
	};

	var profiles = new List<Profile>
	{
		//pGroupABlue,
		//pGroupBBlue,
		//pGroupCBlue,
		pGroupDBlue
	};
	#endregion

	ProcessProfile(profiles);

	//var saveTo = Path.Combine(BASE_PATH, SESSIONS_FOLDER);
	//File.WriteAllText(Path.Combine(saveTo, s + ".npppsession.xml"), content);
}

public void ProcessProfile(List<Profile> profiles)
{
	foreach (var p in profiles)
	{
		var content = CreateNotepadPlusPlusSession(p);
		
		Console.WriteLine(content);
	}
}

public string CreateNotepadPlusPlusSession(Profile profile)
{
	var sb = new StringBuilder();

	sb.AppendLine($"<!-- {profile.Group} Group {profile.Color} contains {profile.Count()} files -->");
	
	foreach (var s in profile.Servers)
	{
		var strServer = profile.UncPathTemplate.Replace("{{SERVER}}", s);
		
		foreach (var d in profile.Clients)
		{
			var fileName = strServer.Replace("{{CLIENT}}", d);
			
			sb.AppendLine(string.Format(TEMPLATE, fileName));
		}
	}
	
	var content = string.Format(ContainerTemplate, sb.ToString());
		
	return content;
}

public string GenerateHtmlLinks(Profile profile)
{
	var sb = new StringBuilder();

	sb.AppendLine($"<!-- {profile.Group} Group {profile.Color} contains {profile.Count()} files -->");

	foreach (var s in profile.Servers)
	{
		var strServer = profile.UncPathTemplate.Replace("{{SERVER}}", s);

		foreach (var d in profile.Clients)
		{
			var fileName = strServer.Replace("{{CLIENT}}", d);

			sb.AppendLine(string.Format(TEMPLATE, fileName));
		}
	}

	var content = string.Format(ContainerTemplate, sb.ToString());

	return content;
}

public enum Operation
{ 
	NotePadPlusPlusSession = 0,
	HtmlLinksPage = 1
}

public class Profile
{
	public string Group { get; set; }
	public string Color { get; set; }
	public Operation OperationType { get; set; }
	public string UncPathTemplate { get; set; } = @"\\{{SERVER}}\E$\Sites\{{CLIENT}}\Web.config";
	public string HtmlLinkTemplate { get; set; } = @"\\{{SERVER}}\E$\Sites\{{CLIENT}}\Web.config";
	public List<string> Servers { get; set; }
	public List<string> Clients { get; set; }
	
	public int Count() => Servers.Count * Clients.Count;
}