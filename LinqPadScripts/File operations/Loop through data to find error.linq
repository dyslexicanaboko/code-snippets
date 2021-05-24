<Query Kind="Statements" />

var lines = File.ReadAllLines(@"C:\Dump\import.txt");

for (int i = 0; i < lines.Length; i++)
{
	var line = lines[i];
	
	var arr = line.Split('|');
	
	try
	{
		int.Parse(arr[1]);
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Line: {i + 1} ==> {line} \n Data ==> {arr[1]}");

		ex.Dump();

		break;
	}	
}

Console.WriteLine("Finished");
