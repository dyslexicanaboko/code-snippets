<Query Kind="Program" />

string BASE_PATH;

void Main()
{
	BASE_PATH = Path.GetDirectoryName(Util.CurrentQueryPath);

	var strHtml = File.ReadAllText(Path.Combine(BASE_PATH, "HtmlTarget.htm"));

	var start = strHtml.Length;

	Console.WriteLine($"Start size: {strHtml.Length}");
	Console.WriteLine($"============================");
	
	/* 1. Remove all instances of \r or \n
	   2. Remove all white space in between tags
	   3. Remove all id attributes and their contents */
	
	//These are string literals that need to be removed because 
	//everything is actually on one line
	strHtml = strHtml.Replace(@"\r", string.Empty)
		   			 .Replace(@"\n", string.Empty)
					 .Replace("\\\"", "\"")
					 .Replace("&nbsp;", "&#160;");

	//This is for the rest of the whitespace
	strHtml = Regex.Replace(strHtml, @"\s*(<[^>]+>)\s*", "$1", RegexOptions.Singleline);
	
	strHtml = Regex.Replace(strHtml, " id=\\\"([^\"]*)\\\"", string.Empty, RegexOptions.Singleline|RegexOptions.IgnoreCase);

	var doc = new XmlDocument();
	doc.LoadXml(strHtml);
	
	//Data manipulation via XML...
	
	strHtml.Dump();

	Console.WriteLine($"============================");
	Console.WriteLine($"End size: {strHtml.Length} -> {(decimal)strHtml.Length / (decimal)start}");
}

