<Query Kind="Program" />

void Main()
{
	Directory.CreateDirectory(@"\\ServerA\E$\Path\");
	
	//File.WriteAllText(@"\\ServerA\E$\Path\Whatever.txt", "Blah");
	
	//RecursiveDelete(@"\\ServerA\E$\Path\Archive\");
}

private static void RecursiveDelete(string path)
{
	var lst = new DirectoryInfo(path).EnumerateDirectories().ToList();
	
	lst.ForEach(RecursiveDelete);
}

private static void RecursiveDelete(DirectoryInfo directory)
{
	if (!directory.Exists)
		return;

	foreach (var dir in directory.EnumerateDirectories())
	{
		RecursiveDelete(dir);
	}

	Console.WriteLine(directory.FullName);

	//This has to be done otherwise if anything changed this object won't implicitly know, it has to explicitly be told
	directory.Refresh();

	directory.Delete(true);
}