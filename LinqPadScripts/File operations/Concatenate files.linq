<Query Kind="Program" />

void Main()
{
	var strOutput = UseSearchPath();
	//var strOutput = UseInstructionsFile();

	Console.WriteLine($"\n\nOutput file: {strOutput}\t\n{DateTime.Now.ToString()}");
}

private string UseInstructionsFile()
{
	var lst = File.ReadAllLines(@"C:\instructions.txt").ToList();
	
	var strOutput = @"C:\Output.sql";
	
	ConcatenateFiles(lst, strOutput, $"{Environment.NewLine}GO{Environment.NewLine}");
	
	return strOutput;
}

private string UseSearchPath()
{
	//Better to copy your target scripts to a folder
	string strPath = @"C:\Dump\delete2\";

	var dir = new DirectoryInfo(strPath);

	var strOutput = Path.Combine(strPath, "Output.sql");

	//An IO Exception can be thrown if you run this back to back - this is an attemp to remove the existing file if found
	if (File.Exists(strOutput))
		File.Delete(strOutput); //Not guaranteed to work

	List<string> lstFi = dir
		.EnumerateFiles("*.sql")
		.Where(x => x.FullName != strOutput)
		.Select(x => x.FullName)
		.ToList();

	ConcatenateFiles(lstFi, strOutput, $"{Environment.NewLine}GO{Environment.NewLine}");
	//ConcatenateFiles(lstFi, strOutput, null);
	
	return strOutput;
}

private void ConcatenateFiles(List<string> fullFilePaths, string outputFilePath, string insertBetweenFiles = null)
{
	try
	{	        
		using (var sw = new StreamWriter(outputFilePath))
		{
			LoopWithProgress(fullFilePaths, "Combining files", (t) =>
			{
				using (var sr = new StreamReader(t))
				{
					sw.Write(sr.ReadToEnd());
	
					if (insertBetweenFiles == null) return;
	
					sw.Write(insertBetweenFiles);
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