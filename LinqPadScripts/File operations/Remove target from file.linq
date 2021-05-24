<Query Kind="Program" />

private int _found;
private int _notFound;

void Main()
{
	var basePath = @"C:\Dev\DatabaseProject\";

	//var r = new Regex(@"GRANT EXECUTE\s+ON OBJECT::\[.+\].\[.+\] TO \[[u|U][s|S][r|R]\]\s+AS \[.+\];", RegexOptions.Multiline);
	var r = new Regex(@"GO\s+GO", RegexOptions.Multiline);

	//SearchFile(
	//@"C:\Dev\SpecificFile.sql",
	//r,
	//string.Empty);

	GetAllFilesFromPathRecursively(basePath, "*.sql", r);
}

// Define other methods and classes here
private void GetAllFilesFromPathRecursively(string basePath, string fileNamePattern, Regex fileSearch, string replacement = null)
{
	//Use for getting all of the matching files
	var dir = new DirectoryInfo(basePath);

	//This could take a very long time to execute
	var lstFiles = dir
		.EnumerateFiles(fileNamePattern, SearchOption.AllDirectories)
		.Select(x => x.FullName)
		.ToList();

	//lstFiles.Dump();

	//This contains all files sql files, unfiltered
	Console.WriteLine($"Files to be searched: {lstFiles.Count}");

	LoopWithProgress(lstFiles, "Operation label", (filePath) => {
		SearchFile(filePath, fileSearch, replacement);
	});

	Console.WriteLine($"Found    : {_found}");
	Console.WriteLine($"Not Found: {_notFound}");
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

private void SearchFile(string filePath, Regex fileSearch, string replacement)
{
	//Console.Write(filePath);
	//Console.Write(",");
	
	var content = File.ReadAllText(filePath);

	if (!fileSearch.IsMatch(content))
	{
		_notFound++;
		
		//Console.WriteLine("Not-found");
		
		return;
	}

	//Console.WriteLine("Found");

	_found++;

	if(replacement == null) return;

	var modified = fileSearch.Replace(content, replacement);
	
	//modified.Dump();

	//This is an assumption that the files being worked with are UTF8 (they usually are)
	File.WriteAllText(filePath, modified, Encoding.UTF8);
}