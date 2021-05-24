<Query Kind="Program" />

void Main(string[] args)
{
	// The idea here is to use a replacement string for the incrementation
	string str =
   @"({0}, 5, 1, 'F', 'P', 'A')
,({0}, 5, 2, 'F', 'P', 'B')
,({0}, 5, 3, 'F', 'P', 'C')";

	AddNumbers(str).ToString().Dump();
}

private StringBuilder AddNumbers(string str)
{
	int i = 1;
	string strLine = null;
	var sb = new StringBuilder();

	using (var sr = new StringReader(str))
	{
		while ((strLine = sr.ReadLine()) != null)
		{
			sb.AppendFormat(strLine, i).AppendLine();

			i++;
		}
	}

	return sb;
}

