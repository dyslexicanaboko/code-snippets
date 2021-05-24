<Query Kind="Program" />

void Main()
{
	var arrFiles = new string[]
	{
		@"C:\Dump\delete16\FileA.txt",
		@"C:\Dump\delete16\FileB.txt"
	};
	
	foreach (var file in arrFiles)
	{
		var lines = File.ReadAllLines(file);
		
		var sorted = lines.OrderBy(x => x).ToArray();
		
		//This is an assumption that the files being sorted are UTF8 (they usually are)
		File.WriteAllLines(file, sorted, Encoding.UTF8);
	}
	
	Console.WriteLine($"Finished @ {DateTime.Now}");
}
