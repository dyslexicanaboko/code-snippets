<Query Kind="Program" />

string BASE_PATH;
string WEB_CONFIG_FOLDER;

void Main()
{
	BASE_PATH = Path.GetDirectoryName(Util.CurrentQueryPath);
	WEB_CONFIG_FOLDER = Path.Combine(BASE_PATH, "Prod Web configs");
	
	List<PathInfo> lst = 
		Directory.EnumerateDirectories(WEB_CONFIG_FOLDER)
				 .Select(x => new PathInfo(x))
				 .ToList();
	
	foreach (PathInfo pi in lst)
	{
		string wc = Directory.EnumerateFiles(
			pi.FullPath, 
			"Web.config", 
			SearchOption.TopDirectoryOnly).FirstOrDefault();

		File.Move(wc, Path.Combine(BASE_PATH, pi.Client + ".config"));
	}
}

public class PathInfo
{ 
	public PathInfo(string fullPath)
	{
		string[] arr = fullPath.Split('\\');
		
		FullPath = fullPath;
		Client = arr[arr.Length - 1];
	}

	public string FullPath { get; set; }
	public string Client { get; set; }
}
