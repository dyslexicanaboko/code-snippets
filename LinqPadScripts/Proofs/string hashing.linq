<Query Kind="Program" />

void Main()
{
	//"string".GetHashCode().Dump();
	//GetHash("string");
	//GetHashJs("string");
	//302915211950166140
	//285468193110112700
	//1853701448
	
	//(2 ^ 6).Dump();
	
	//(((5381 << 5) + 5381)^58).Dump();
	//193432842 + (193422079 * 1566083941)
	
	TestSet();
}

public void TestSet()
{
	const int letters = 26;
	const int startCharCode = 97;
	var endCharCode = startCharCode + letters;
	
	var sb = new StringBuilder();
	sb.AppendLine("[");
	
	for (var i = startCharCode; i < endCharCode; i++)
	{
		var l = ((char)i).ToString();

		var hcL = GetHash(l);

		var u = l.ToUpper();

		var hcU = GetHash(u);

		Console.WriteLine($"{l}, {hcL}, {u}, {hcU}");

		sb.Append("['").Append(l).Append("', ").Append(hcL).Append("],").AppendLine();
		sb.Append("['").Append(u).Append("', ").Append(hcU).Append("],").AppendLine();
	}

	sb.AppendLine("]");
	
	sb.ToString().Dump();
}

public int GetHash(string str)
{
	unsafe
	{
		fixed (char* src = str)
		{
			int hash1 = 5381;
			int hash2 = hash1;

			int c;
			char* s = src;
			while ((c = s[0]) != 0)
			{
				Console.WriteLine($"C1: {c}");
				
				hash1 = ((hash1 << 5) + hash1) ^ c;
				
				Console.WriteLine($"H1: {hash1}");

				c = s[1];

				Console.WriteLine($"C2: {c}");

				if (c == 0)
					break;

				hash2 = ((hash2 << 5) + hash2) ^ c;
				
				Console.WriteLine($"H2: {hash2}");

				s += 2;
			}

			var final = hash1 + (hash2 * 1566083941);

			Console.WriteLine($"HS: {final}");
			
			return final;
		}
	}
}

public void GetHashJs(string str)
{
	int hash1 = 5381;
	int hash2 = hash1;

	int c;
	char[] s = str.ToCharArray();
	
	for (int i = 0; i < s.Length; i++)
	{
		c = s[i]; //Get frame
		Console.WriteLine($"C: {c}");

		if (i % 2 == 0)
		{
			//Frame 1 gets the event indices
			hash1 = ((hash1 << 5) + hash1) ^ c;
			Console.WriteLine($"H1: {hash1}");
		}
		else
		{
			//Frame 2 gets the odd indices
			hash2 = ((hash2 << 5) + hash2) ^ c;
			Console.WriteLine($"H2: {hash2}");
		}
	}

	var final = hash1 + (hash2 * 1566083941);

	Console.WriteLine($"HS: {final}");
}