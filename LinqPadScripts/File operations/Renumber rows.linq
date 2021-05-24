<Query Kind="Program" />

void Main()
{
	var txt =
	@"obj.PropertA = Convert.ToInt32(arr[0]); i = 0;
		obj.PropertyB = Convert.ToString(arr[1]); i = 1;
		obj.PropertyC = Convert.ToString(arr[2]); i = 2;";

	var arr = txt.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

	var sb = new StringBuilder();

	var r = new Regex("i = ([0-9]+);", RegexOptions.Singleline);

	for (int i = 0; i < arr.Length; i++)
	{
		var line = arr[i];

		if(string.IsNullOrWhiteSpace(line)) continue;

		var m = r.Match(line);
		
		//m.Dump();
		
		var n = Convert.ToInt32(m.Groups[1].Value) + 1;

		var str = $"i = {n};";
		
		var numbered = r.Replace(line, str);

		//var numbered = line.Replace("arr[0]", $"arr[{i}]");

		sb.AppendLine(numbered);
	}

	var txt2 = sb.ToString();

	txt2.Dump();
}
