<Query Kind="Program" />

void Main()
{
	var data = @"lowercaseA
	lowercaseB
	lowercaseC
	";

FirstLetterCapitalized(data);

}

public void FirstLetterCapitalized(string data)
{
	//Blanket lower case everything
	var content = data.ToLower();

	var sb = new StringBuilder();

	using(var r = new StringReader(content))
	{
		var line = r.ReadLine();

		while (line != null)
		{
			var arr = line.Split(' ');
			
			var lastIndex = arr.Length - 1;
			
			for (int i = 0; i < arr.Length; i++)
			{
				var word = arr[i];
				
				//First letter
				var firstLetter = word.Substring(0, 1).ToUpper();
				
				var restOfWord = word.Substring(1, word.Length - 1);
				
				sb.Append(firstLetter).Append(restOfWord);

				if (i == lastIndex) continue;
				
				sb.Append(" ");
			}

			sb.AppendLine();

			line = r.ReadLine();
		}
	}
	
	var str = sb.ToString();
	
	str.Dump();
}
