<Query Kind="Program" />

string BASE_PATH;
string TEMPLATES;
string SAVE_TO;

void Main()
{
	BASE_PATH = Path.GetDirectoryName(Util.CurrentQueryPath);
	TEMPLATES = Path.Combine(BASE_PATH, "Templates");
	SAVE_TO = Path.Combine(BASE_PATH, "Output");

	//Create the path if it doesn't exist
	if(!Directory.Exists(SAVE_TO)) Directory.CreateDirectory(SAVE_TO);

	//C:\Dev\DB Copies\{{Database}}_backup_201605152315.bak
	//C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\

	//List<Profile> lst = GetLocalProfiles();
	//List<Profile> lst = GetServerProfiles();
	List<Profile> lst = GetProfiles();

	foreach (Profile p in lst)
		GenerateRestoreScript(p);
}

private List<Profile> GetLocalProfiles()
{
	var localWestFlorida = new Profile()
	{
		Database = "ClientDb"
	};

	var localSanAntonio = new Profile()
	{
		Database = "ClientDb"
	};

	var localSutter = new Profile()
	{
		Database = "ClientDb"
	};

	var localContinental = new Profile()
	{
		Database = "ClientDb"
	};

	var lst = new List<Profile>()
	{
		localSutter,
		localSanAntonio,
		localWestFlorida,
		localContinental
	};

	lst.ForEach(x =>
	{
		x.FileNamePrefix = "Local";
	});

	return lst;
}

private List<Profile> GetServerProfiles()
{
	var serverSutter = new Profile()
	{
		Database = "ClientDb"
	};

	var serverSanAntonio = new Profile()
	{
		Database = "ClientDb"
		
	};

	var serverWestFlorida = new Profile()
	{
		Database = "ClientDb"
	};

	var lst = new List<Profile>()
	{
		serverSutter,
		serverSanAntonio,
		serverWestFlorida
	};

	//The servers all have the same Bak location
	lst.ForEach(x =>
	{
		x.BakFileLocation = @"E:\DB Backup Copies\";
		x.FileNamePrefix = "ArchiveServer";
		x.MdfPath = @"F:\Sql Server MDF LDF\";
		x.LdfPath = @"G:\Sql Server MDF LDF\";
	});
	
	return lst;
}

private List<Profile> GetProfiles()
{
	var lst = new List<Profile>()
	{
		new Profile() { Database = "ClientDbA" },
		new Profile() { Database = "ClientDbB" },
		new Profile() { Database = "ClientDbC" },
		new Profile() { Database = "ClientDbD" },
		new Profile() { Database = "ClientDbE" },
		new Profile() { Database = "ClientDbF" },
		new Profile() { Database = "ClientDbG" }
	};

	//The servers all have the same Bak location
	lst.ForEach(x =>
	{
		x.BakFileLocation = @"G:\";
		x.FileNamePrefix = "ServerA";
		x.MdfPath = @"F:\MSSQL\Data\";
		x.LdfPath = @"E:\MSSQL\Logs\";
	});

	return lst;
}

public class Profile
{
	public Profile()
	{
		//These are my defaults on my local
		SetMdfLdfPath(@"C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\");
		BakFileLocation = @"C:\Dev\DB Copies\";
	}

	public void SetMdfLdfPath(string commonPath)
	{
		MdfPath = commonPath;
		LdfPath = commonPath;
	}

	public string FileNamePrefix { get; set; }
	public string Database { get; set; }
	public string BakFileLocation { get; set; }
	public string MdfPath { get; set; }
	public string LdfPath { get; set; }
}

public void GenerateRestoreScript(Profile profile)
{
	string strTemplate = File.ReadAllText(Path.Combine(TEMPLATES, "RestoreTemplate.txt"));
	
	string strBakFullFilePath = Path.Combine(profile.BakFileLocation, profile.Database + ".bak");
	
	string strScript =
		strTemplate
			.Replace("{{Database}}", profile.Database)
			.Replace("{{BakFileLocation}}", strBakFullFilePath)
			.Replace("{{MdfPath}}", profile.MdfPath)
			.Replace("{{LdfPath}}", profile.LdfPath);

	Console.WriteLine($"Restore script created for {profile.Database} @ {DateTime.Now.ToString()}");
	
	string strRestorScript = Path.Combine(SAVE_TO, profile.FileNamePrefix + ".Restore " + profile.Database + " DB.sql");
	
	File.WriteAllText(strRestorScript, strScript);
}