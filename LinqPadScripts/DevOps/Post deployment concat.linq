<Query Kind="Program" />

//Find all files listed in the post deployment script that look like this:
//Ex: :r .\PostDeployScript.sql
private static readonly Regex ReTargetFile = new Regex(@":r\s+\.\\(.+\.sql)");
private static readonly string D = $"{Environment.NewLine}GO{Environment.NewLine}";
private const string BaseOutputPath = @"C:\Output\";
private const string SrcProductA = @"C:\Dev\RepoA\Scripts\";
private const string SrcProductB = @"C:\Dev\RepoB\Scripts\";
private const string SrcProductC = @"C:\Dev\RepoC\Scripts\";

void Main()
{
	//var strOutput = UseSearchPath();
	//var strOutput = UseInstructionsFile();

	/* Pre, Schema, Seed, Post */

	var s = new Stamp(1, 0, 0, 0);
	var p = s.VersionPeriods;
	var u = s.VersionUnderscore;
	var d = s.TodaysDate;

	var basePathVersion = Path.Combine(BaseOutputPath, p);

	CreateFolders(basePathVersion); 

	#region Profiles
	var pProductAPreDeploy = new Profile
	{
		Name = "Product A - Pre deployment",
		InstructionsFullFilePath = Path.Combine(SrcProductA, $@"One time execution\{p}\", "PreDeployment.sql"),
		SaveAsFullFilePath = Path.Combine(basePathVersion, $"Product A {p} Pre deployment {d}.sql")
	};

	var pProductASeed = new Profile
	{
		Name = "Product A - seed scripts",
		InstructionsFullFilePath = Path.Combine(SrcProductA, @"Post-Deployment\", "PostDeployment.sql"),
		SaveAsFullFilePath = Path.Combine(basePathVersion, $"Product A {p} Seed scripts {d}.sql")
	};

	var pProductAPostDeploy = new Profile
	{
		Name = "Product A - Post deployment",
		InstructionsFullFilePath = Path.Combine(SrcProductA, $@"One time execution\{p}\", "PostDeployment.sql"),
		SaveAsFullFilePath = Path.Combine(basePathVersion, $"Product A {p} Post deployment {d}.sql")
	};

	var pProductBOneTime = new Profile
	{
		Name = "Product B - One time execution",
		InstructionsFullFilePath = Path.Combine(SrcProductB, @"One time only\"), //None as of 12/18/2020
		SaveAsFullFilePath = Path.Combine(basePathVersion, $"Product B Post deployment {d}.sql")
	};

	var pProductBSeed = new Profile
	{
		Name = "Product B - seed scripts",
		//C:\Dev\nadcwpapptfs01\Git\MultiTenancy\src\Hca.MultiTenancy.Database\Scripts\Post-Deployment\PostDeployment.sql
		InstructionsFullFilePath = Path.Combine(SrcProductB, @"Post-Deployment\", "PostDeployment.sql"),
		SaveAsFullFilePath = Path.Combine(basePathVersion, $"Product B Seed scripts {d}.sql")
	};

	var pProductCSeed = new Profile
	{
		Name = "Product C - seed scripts",
		InstructionsFullFilePath = Path.Combine(SrcProductC, @"Scripts\Post deployment\", "PostDeployment.sql"),
		SaveAsFullFilePath = Path.Combine(basePathVersion, $"Product C Seed scripts {d}.sql")
	};
	#endregion

	//Don't forget to number your files after they are generated
	var lstProfiles = new List<Profile>
	{
		pProductAPostDeploy
	};

	GenerateScripts(lstProfiles);

	Console.WriteLine($"\n\nFinished {lstProfiles.Count} profile(s) @ {DateTime.Now.ToString()}");
}

private void CreateFolders(params string[] paths)
{
	foreach (var p in paths)
	{
		Directory.CreateDirectory(p);
	}
}

private void GenerateScripts(List<Profile> profiles)
{
	foreach (var p in profiles)
	{
		BuildProfileList(p);

		p.Dump();

		ConcatenateFiles(p);

		Console.WriteLine($"\n\nOutput file: {p.SaveAsFullFilePath}\t\n{DateTime.Now.ToString()}");
	}
}

private void BuildProfileList(Profile profile)
{
	var p = profile;
	
	Console.WriteLine($"Building: {p.Name}");
	
	//Get base path
	p.BasePath = Path.GetDirectoryName(p.InstructionsFullFilePath);
	
	var lstLines = File.ReadAllLines(p.InstructionsFullFilePath).ToList();
	
	var lstInstructions = new List<string>();
	
	foreach (var l in lstLines)
	{
		if (string.IsNullOrWhiteSpace(l)) continue;
		
		//Commented out lines ignore
		if(l.StartsWith("--")) continue;
		
		var m = ReTargetFile.Match(l);
		
		if(!m.Success) continue;
		
		var fileName = m.Groups[1].Value;
		
		var fullFilePath = Path.Combine(p.BasePath, fileName);

		if (!File.Exists(fullFilePath))
		{
			Console.WriteLine("Not found: " + fullFilePath);
			
			continue;
		}
		
		lstInstructions.Add(fullFilePath);
	}
	
	p.TargetFilePaths = lstInstructions;
}

private void ConcatenateFiles(Profile profile)
{
	try
	{	        
		using (var sw = new StreamWriter(profile.SaveAsFullFilePath))
		{
			LoopWithProgress(profile.TargetFilePaths, "Combining files", (t) =>
			{
				var fn = Path.GetFileName(t);
				
				using (var sr = new StreamReader(t))
				{
					sw.WriteLine($"PRINT '{fn}'");
					sw.WriteLine($"GO");
					sw.WriteLine();
					sw.Write(sr.ReadToEnd());
					sw.WriteLine();
					sw.WriteLine("GO");
					sw.WriteLine("PRINT ' --- ' ");

					if (D == null) return;
	
					sw.Write(D);
				}
			});
		}
	}
	catch (IOException)
	{
		Console.WriteLine("\n\n\nIf you keep getting this error, just delete the file yourself manually. This is a Windows problem.\n\n\n\n");
		
		throw;
	}
}

public void LoopWithProgress<T>(List<T> data, string processMessage, Action<T> method, string progressTick = "|")
{
	Console.WriteLine($"{processMessage} ({data.Count:n0}):");

	//Show a progress tick every 1%
	var frequency = Convert.ToInt32(data.Count * 0.01);

	//Count between progress ticks
	var minor = 0;

	//Put back into query
	for (int i = 0; i < data.Count; i++)
	{
		var obj = data[i];

		method(obj);

		if (minor == frequency)
		{
			Console.Write(progressTick);

			minor = 0;
		}
		else
			minor++;
	}

	Console.WriteLine(" - Finished");
}

public class Stamp
{
	public Stamp(int major, int minor, int patch, int preRelease)
	{
		Major = major;
		Minor = minor;
		Patch = patch;
		PreRelease = preRelease;

		VersionPeriods = FormatVersion(@".");
		VersionUnderscore = FormatVersion(@"_");
		TodaysDate = DateTime.Now.ToString("yyyy.MM.dd");
	}

	public int Major { get; set; }
	public int Minor { get; set; }
	public int Patch { get; set; }
	public int PreRelease { get; set; }

	private string FormatVersion(string separator)
	{
		var str = $"{Major}{separator}{Minor}{separator}{Patch}{separator}{PreRelease}";
		
		return str;
	}

	public string VersionPeriods { get; set; }
	public string VersionUnderscore { get; set; }
	public string TodaysDate { get; set; }
}

public class Profile
{
	public string Name { get; set; }
	
	public string SaveAsFullFilePath { get; set; }
	
	public string InstructionsFullFilePath { get; set; }
	
	public string BasePath { get; set; }
	
	public List<string> TargetFilePaths { get; set; } = new List<string>();
}