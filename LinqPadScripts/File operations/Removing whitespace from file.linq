<Query Kind="Program" />

void Main()
{
	var arr = 
	File.ReadAllLines(@"C:\Dump\Target.txt")
	.Where(x => !string.IsNullOrWhiteSpace(x))
	.ToArray();
	
	File.WriteAllLines(@"C:\Dump\Output.txt", arr);
}
