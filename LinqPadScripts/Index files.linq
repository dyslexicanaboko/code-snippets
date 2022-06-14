<Query Kind="Program" />

void Main()
{
	var lst = ListDirectories(@"M:\Media\Music\_Trusted\");
	
	lst.Count.Dump();

	var counts = ListDirectoryFileCounts(@"M:\Media\Music\_Trusted\").Where(x => x.FileCount == 0).ToList();
	
	counts.Dump();

	//CreateMoveContentOperations(lst);

	//lst.Dump();

	
}



public List<Operation> FromRootDirectory()
{
	var dir = new DirectoryInfo(@"M:\Media\Music\")
		.GetDirectories()
		.Where(x =>
				x.Name.Length > 1
			&& !x.Name.Equals("_Unorganized", StringComparison.InvariantCultureIgnoreCase)
			&& !x.Name.Equals("Soundtracks", StringComparison.InvariantCultureIgnoreCase)
		)
		.Select(x => x.FullName)
		.OrderBy(x => x)
		.ToList();

	var lst = new List<Operation>();

	foreach (var d in dir)
	{
		var dirSub = ListDirectories(d);

		lst.AddRange(dirSub);
	}

	lst = lst.OrderBy(x => x.TargetPath).ToList();

	return lst;
}

public void CreateMoveOperations(List<Operation> operations)
{
	operations = operations.OrderBy(x => x.TargetPath).ToList();

	foreach (var m in operations)
	{
		Console.WriteLine($"MOVE /Y \"{m.TargetPath}\" \"M:\\Media\\Music\\{m.Index}\"");
	}
}

public void CreateMoveContentOperations(List<Operation> operations)
{
	operations = operations.OrderBy(x => x.TargetPath).ToList();

	foreach (var dir in operations)
	{
		var lstSub = ListDirectories(dir.TargetPath);

		foreach (var s in lstSub)
		{
			Console.WriteLine($"MOVE /Y \"{s.TargetPath}\" \"M:\\Media\\Music\\{dir.Index}\\{dir.DirectoryName}\\\"");
		}
	}
}

public void CreateIndexFolders(List<Operation> operations)
{
	var indexes = operations
		.Select(x => x.Index)
		.Distinct()
		.OrderBy(x => x)
		.ToList();

	foreach (var i in indexes)
	{
		Console.WriteLine($"IF NOT EXIST \"M:\\Media\\Music\\{i}\" MKDIR \"M:\\Media\\Music\\{i}\"");
	}
}

public List<DirFileCount> ListDirectoryFileCounts(string directory)
{
	var dirs = new DirectoryInfo(directory)
		.GetDirectories();

	var lst = new List<DirFileCount>(dirs.Length);

	foreach (var d in dirs)
	{
		var m = new DirFileCount();
		m.Path = d.FullName;
		
		var dir = new DirectoryInfo(d.FullName);
		
		m.FileCount = dir.EnumerateDirectories().Count() + dir.EnumerateFiles().Count();
		
		lst.Add(m);
	}	
		
	return lst;
}

public class DirFileCount
{ 
	public string Path { get; set; }
	
	public int FileCount { get; set; }
}

public List<Operation> ListDirectories(string directory)
{
	var dir = new DirectoryInfo(directory)
		.GetDirectories()
		.Select(x =>
		{
			var name = x.Name;
			
			if (x.Name.StartsWith("The ", StringComparison.InvariantCultureIgnoreCase))
			{
				name = name.Substring(4);
			}
						
			var o = new Operation
			{
				Index = name.Substring(0, 1).ToUpper(),
				DirectoryName = x.Name,
				TargetPath = x.FullName
			};
			
			return o;
		})
		.ToList();
		
	return dir;
}

public class Operation
{
	public string TargetPath { get; set; }
	
	public string DirectoryName { get; set; }

	public string Index { get; set; }
}
