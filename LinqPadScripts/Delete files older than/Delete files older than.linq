<Query Kind="Program" />

const bool DryRun = true;
const int Days = 30;
string BasePath = Path.GetDirectoryName(Util.CurrentQueryPath);

void Main()
{
	var p = new Profile();
	
	p.SearchPaths.Add(new PathAndPattern(@"J:\Downloads\", "*"));
	p.SearchPaths.Add(new PathAndPattern(@"J:\Dump\", "*"));

	p.WhiteListFiles.Add(WildCardToRegex(@"temp*.*"));
	p.WhiteListDirectories.Add(@"J:\Dump\Don't delete\");
	p.WhiteListDirectories.Add(@"J:\Dump\Scan dump\");

	var operations = CleanUp(p, DryRun);
	
	var path = Path.Combine(BasePath, "Delete operations.log");
	
	var txt = GetText(operations);
	
	File.AppendAllText(path, txt);
}

private string GetText(List<string> logs)
{
	var n = Environment.NewLine;

	var header = $"{DateTime.Now:yyyy.MM.dd HH:mm:ss}{n}===================================================={n}";
	
	var body = string.Join(n, logs);
	
	var final = header + body + n + n;
	
	return final;
}

private List<string> CleanUp(Profile profile, bool dryRun)
{
	var p = profile;

	var operations = new List<string>();

	var lstFiles = new List<FileInfo>();

	//Get files to delete
	foreach (PathAndPattern obj in p.SearchPaths)
	{
		lstFiles.AddRange(new DirectoryInfo(obj.Path).GetFiles(obj.Pattern, SearchOption.AllDirectories)
											 .Where(x => (DateTime.Now - x.CreationTime).Days > Days)
											.ToList());
	}

	//FullName has to be used instead of DirectoryName because DirectoryName will throw an exception for anything over 260 characters
	p.WhiteListDirectories.ForEach(d => lstFiles.RemoveAll(x => x.FullName.StartsWith(d)));

	//Remove all whitelisted files
	for (int i = lstFiles.Count - 1; i >= 0; i--)
	{
		var f = lstFiles[i].Name;

		if (SkipFile(f, p.WhiteListFiles))
			lstFiles.RemoveAt(i);
	}

	//Order by creation time
	lstFiles = lstFiles.OrderBy(x => x.CreationTime).ToList();

	Console.WriteLine($"Deleted Files {lstFiles.Count}");

	//Delete the individual files
	foreach (var x in lstFiles)
	{
		try
		{
			operations.Add($"File,{x.CreationTime:yyyy/MM/dd HH:mm:ss},{x.DirectoryName},{x.Name}");
			
			if(!dryRun) File.Delete(x.FullName);
		}
		catch (Exception ex)
		{
			//Oh well
			Console.WriteLine(ex.Message);
		}
	}

	var lstFolders = FindEmptyFoldersToDelete(lstFiles, p.SearchPaths);

	Console.WriteLine($"Deleted Directories {lstFolders.Count}");

	//Lastly delete the folders that are empty, but not the search paths
	foreach (var folder in lstFolders)
	{
		try
		{
			operations.Add($"Directory,NULL,{folder},NULL");

			if(!dryRun) Directory.Delete(folder, true);
		}
		catch (Exception ex)
		{
			//Oh well
			Console.WriteLine(ex.Message);
		}
	}
	
	return operations;
}

//Get the folders to delete ultimately that are empty after files have been deleted
private List<string> FindEmptyFoldersToDelete(List<FileInfo> files, List<PathAndPattern> searchPaths)
{
	//Unfortunately because of the PathTooLongException I have to jump through hoops to make this work
	List<string> lst =
		files
			.Select(x => x.FullName)
			.Distinct()
			.ToList();
	
	//Get distinct paths manually
	var paths = new List<string>();

	for (var i = lst.Count - 1; i > 0; i--)
	{
		try
		{
			var filePath = lst[i];

			var path = Path.GetDirectoryName(filePath);

			//Remove all white listed paths
			if (searchPaths.Any(p => p.Path == path)) continue;

			//Make the list distinct manually
			if(!paths.Contains(path))
				paths.Add(path);
		}
		catch (PathTooLongException ptle)
		{
			//Just leave these files behind because of this exception
			Console.WriteLine(ptle.Message);
		}
	}

	//Remove all directories that aren't empty
	for (var i = paths.Count - 1; i > 0; i--)
	{
		try
		{
			var path = paths[i];
			
			var di = new DirectoryInfo(path);

			if (di.EnumerateFiles().Count() > 0)
			{
				lst.RemoveAt(i);
			}
		}
		catch (PathTooLongException ptle)
		{
			//Just leave these files behind because of this exception
			Console.WriteLine(ptle.Message);
		}
	}

	return paths;
}

private bool SkipFile(string value, IList<string> whiteList)
{
	foreach (string r in whiteList)
	{
		if(Regex.IsMatch(value, r, RegexOptions.IgnoreCase))
			return true;
	}
	
	return false;
}

private string WildCardToRegex(string value)
{
	return "^" + Regex.Escape(value).Replace("\\*", ".*") + "$";
}

public class Profile
{
	public List<string> WhiteListFiles { get; set; } = new List<string>();
	public List<string> WhiteListDirectories { get; set; } = new List<string>();
	public List<PathAndPattern> SearchPaths { get; set; } = new List<PathAndPattern>();
}

// Define other methods and classes here
public class PathAndPattern
{
	public PathAndPattern(string path, string pattern)
	{
		Path = System.IO.Path.GetDirectoryName(path);
		Pattern = pattern;
	}
	
	public string Path { get; set; }
	
	public string Pattern { get; set; }
}