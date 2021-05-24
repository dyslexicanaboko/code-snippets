<Query Kind="Program" />

void Main()
{
	var testFile = @"C:\Dump\bigFile.txt";
	
	//CreateTestFile(testFile, 500);
	
	TruncateLogFile(testFile, 10);
}

public void CreateTestFile(string filePath, int numberOfLines)
{
	var lines = new List<string>(numberOfLines);

	for (int i = 0; i < lines.Capacity; i++)
	{
		lines.Add("Fake data");
	}

	File.WriteAllLines(filePath, lines);
}

// Define other methods and classes here
public void TruncateLogFile(string filePath, int maxLinesInFile)
{
	if(!File.Exists(filePath)) return;
	
	var lines = File.ReadAllLines(filePath);

	if(lines.Length < maxLinesInFile) return;
	
	var overage = lines.Length - maxLinesInFile;
	
	var bottomHalf = lines.Skip(overage).ToArray();
	
	File.WriteAllLines(filePath, bottomHalf);
}