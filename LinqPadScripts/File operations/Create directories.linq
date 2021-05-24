<Query Kind="Statements" />

//Create a list of nested directories via C#
//Excludes files because they cannot be differentiated properly

var data = @"C:\Dump\DirectoryA\
C:\Dump\DirectoryB\
C:\Dump\DirectoryC\
";

var lines = data
	.Split(Environment.NewLine.ToCharArray())
	.Where(x => !string.IsNullOrWhiteSpace(x))
	.Distinct()
	.OrderBy(x => x)
	.ToArray();

//lines.Dump();

foreach (var d in lines)
{
	if(!Directory.Exists(d))
	{
		d.Dump();
		
		Directory.CreateDirectory(d);
	}
}
