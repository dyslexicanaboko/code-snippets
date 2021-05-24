<Query Kind="Program" />

void Main()
{
	string strPath = @"C:\Target\";
	string strOutputPath = @"C:\Output\";
	
	List<PathInfo> lst = 
		Directory.EnumerateDirectories(strPath)
				 .Select(x => new PathInfo(x))
				 .ToList();

	string strNewFile = null;
	string strNewPath = null;
	string strNewDivisionPath = null;
	
	List<string> lstOriginalFiles;
	
	foreach (PathInfo pi in lst)
	{
		lstOriginalFiles = Directory.EnumerateFiles(
			pi.FullPath, 
			"*.config", 
			SearchOption.AllDirectories).ToList();

		strNewDivisionPath = Path.Combine(strOutputPath, pi.Client);

		if (!Directory.Exists(strNewDivisionPath))
			Directory.CreateDirectory(strNewDivisionPath);

		foreach (string original in lstOriginalFiles)
		{
			//strNewFile = pi.Division + "." + Path.GetFileName(original);
			strNewFile = Path.GetFileName(original);
			
			strNewPath = Path.Combine(strNewDivisionPath, strNewFile);
			
			File.Copy(original, strNewPath, true);
			
			strNewPath.Dump();
		}
		
		ConcatenateFiles(lstOriginalFiles, Path.Combine(strNewDivisionPath, "Combo.config"));
	}
}

private void ConcatenateFiles(IList<string> fullFilePaths, string outputFilePath)
{
	using (var sw = new StreamWriter(outputFilePath))
	{
		foreach (string t in fullFilePaths)
		{
			using (var sr = new StreamReader(t))
			{
				sw.Write(sr.ReadToEnd());
			}
		}
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
