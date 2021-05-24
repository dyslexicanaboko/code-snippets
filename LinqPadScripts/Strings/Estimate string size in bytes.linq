<Query Kind="Program" />

void Main()
{
	string characters = "Put any string you want here";

	Console.WriteLine($"String length : {characters.Length:n0}");

	ByteArraySizes(characters);
}

//Not saying this is accurate - I used this to test out some things and I don't think it worked out too well
public void ByteArraySizes(string characters)
{
	var lstEncodings = new List<Encoding>
	{
		new UnicodeEncoding(), //UTF-16
		new ASCIIEncoding(),
		new UTF7Encoding(),
		new UTF8Encoding(),
		new UTF32Encoding()
	};
	
	var dict = new Dictionary<string, int>();
	
	foreach (var encoding in lstEncodings)
	{
		var bytes = encoding.GetBytes(characters);

		dict.Add(encoding.GetType().Name, bytes.Length);
	}

	//Order by smallest size to largest
	var lst = dict.OrderBy(x => x.Value).Select(x => $"{x.Key} : {x.Value:n0}").ToList();

	lst.ForEach(Console.WriteLine);
}

