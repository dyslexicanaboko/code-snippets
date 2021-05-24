<Query Kind="Program" />

void Main()
{
	var text = @"amy smart
angela bassett
aubrey plaza
christina hendricks
courteney cox
elijah dushku
emily ratajkowski
eva mendez
gabrielle union
jaime pressly
jennifer garner
jennifer lawrence
katy perry
kelly rowland
kerry washington
marcia cross
mika brzezinski
nicki minaj
olivia wilde
paget brewster
rachael leigh cook
rachel mcadams
regina hall
rihanna
rosario dawson
salma hayek
sanaa lathan
selena gomez
selma blair
stacey dash
teri hatcher
tiffanie seaberry
zooey deschanel";
	
	CapitalizeWords(text).Dump();
}

// Define other methods and classes here
private string CapitalizeWords(string text)
{
	var arr = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

	arr.Dump();

	var sb = new StringBuilder();

	foreach (var e in arr)
	{
		var t = e.Split(' ');

		sb.Append(CapitalizeWord(t[0]));

		if (t.Length > 1)
		{
			sb.Append(" ")
			  .Append(CapitalizeWord(t[1]));
		}
		
		sb.AppendLine();
	}
	
	return sb.ToString();
}

private string CapitalizeWord(string word)
{
	var arr = word.ToCharArray();
	
	arr[0] = Char.ToUpper(arr[0]);
	
	var s = new string(arr);
	
	return s;
}