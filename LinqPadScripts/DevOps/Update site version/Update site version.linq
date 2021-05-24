<Query Kind="Program" />

string BASE_PATH;
string REPO_PATH;

const string CONT_KEY = "61258";

void Main(string[] args)
{
	BASE_PATH = Path.GetDirectoryName(Util.CurrentQueryPath);
	REPO_PATH = @"C:\Dev\SiteRepo\";
	
	var newVersion = "1.1.0.0";

	var skipContinueCheck = args != null && args.Length != 0;

	if (skipContinueCheck)
	{
		newVersion = args[0];
		
		REPO_PATH = args[1];
	}
	
	string strProperties = @"Properties\AssemblyInfo.cs";

	var lstProjectShortPath = new List<string>
	{
		"ProjectA",
		"ProjectB",
		"ProjectC"
	};

	var lstFiles = lstProjectShortPath
		.Select(x => Path.Combine(REPO_PATH, x, strProperties))
		.ToList();
	
	//Dedicated SQL file if keeping track of database version
	lstFiles.Add(Path.Combine(REPO_PATH, @"SetDbVersion.sql"));

	lstFiles.Dump();
	
	Console.WriteLine($"Search for old version number and replace with \"{newVersion}\"");

	if (!skipContinueCheck)
	{
		Console.WriteLine("Are you sure you want to do this? Enter the pass key in order to continue.");

		var input = Console.ReadLine();

		if (input != CONT_KEY)
		{
			Console.WriteLine("Invalid key!");

			return;
		}
	}
	
	UpdateVersionNumber(lstFiles, newVersion);
}

public void UpdateVersionNumber(List<string> files, string newVersionNumber)
{
	//(Match zero through nine separated by one period) <-- repeated 4 times basically, no trailing period
	var r = new Regex(@"[0-9]+[\.]{1}[0-9]+[\.]{1}[0-9]+[\.]{1}[0-9]+");

	var changes = 0;
	
	foreach (var f in files)
	{
		string text = File.ReadAllText(f);

		var mc = r.Matches(text);

		if (mc.Count == 0)
		{
			Console.WriteLine($"Version number(s) not found in \"{f}\"");
			
			continue;
		}

		Console.WriteLine($"Found {mc.Count} matches in {f}");
		
		text = r.Replace(text, newVersionNumber);
		
		//The encoding is very important, do not remove it.
		File.WriteAllText(f, text, Encoding.UTF8);
		
		changes++;
	}

	Console.WriteLine($"Files changed: {changes} out of {files.Count}");
}