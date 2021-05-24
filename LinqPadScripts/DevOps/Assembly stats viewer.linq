<Query Kind="Program" />

void Main()
{
	var path = @"C:\Dev\ProjectA\bin\Debug\";
	
	GetAllAssemblyStats(path);
}

public void GetAllAssemblyStats(string path)
{
	var arrExt = new string[] {"*.exe", "*.dll" };

	var lst = new List<AsmInfo>();

	foreach (var ext in arrExt)
	{
		var files = new DirectoryInfo(path)
			.EnumerateFiles(ext, SearchOption.TopDirectoryOnly)
			.OrderBy(x => x.Name);
		
		foreach (var f in files)
		{
			var v = FileVersionInfo.GetVersionInfo(f.FullName);
			
			var a = new AsmInfo 
			{
				Name = f.Name,
				DateModified = f.LastWriteTime,
				Version = v.FileVersion
			};

			lst.Add(a);
		}
	}
	
	lst.Dump();
}

public class AsmInfo
{ 
	public string Name { get; set; }
	public DateTime DateModified { get; set; }
	public string Version { get; set; }
}