<Query Kind="Program" />

void Main()
{
	var re = new Regex("[^A-Za-z0-9]");
	var output = re.Replace(input, " ");
	
	var lst = output
	.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
	.Distinct()
	.OrderBy(x => x)
	.ToList();
	
	lst.Dump();
}

string input = @"
   Stack trace goes here
";