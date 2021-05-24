<Query Kind="Program" />

void Main()
{
	var strPath = @"C:\Dump\Confluence.txt";
	
	var lines = GetLines(strPath);
	
	GetCsv(lines, 3);
}

/*
1. Edit a confluence page
2. Click on a table
3. Click on the copy button at the top
4. Paste into note pad and your data should be put into a token per line format
*/
public void GetCsv(string[] lines, int columnWidth)
{
	var c = 0;
	
	var sb = new StringBuilder();
	
	foreach (var line in lines)
	{
		sb.Append(line);
		
		c++;

		if (c < columnWidth)
		{
			sb.Append(",");
			
			continue;
		}
		
		c = 0;
		
		sb.AppendLine();
	}
	
	var str = sb.ToString();
	
	str.Dump();
}

public string[] GetLines(string path)
{
	var lines = File.ReadAllLines(path)
		.Where(x => !string.IsNullOrEmpty(x))
		.ToArray();

	return lines;
}