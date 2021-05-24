<Query Kind="Program" />

void Main()
{
	GetHash("ABCD");
	
	GetHash("DCBA");
}

public int GetHash(string item, bool printDebug = false)
{
	if (item == null) return 0;
	
	var strHc = item.GetHashCode();
	
	Console.WriteLine($"item = {strHc:n0}");
	
	if(item == string.Empty) return strHc;
	
	var arr = item.ToCharArray();
	
	var hc = 0;
	
	//This is only for debug
	for (int i = 0; i < arr.Length; i++)
	{
		var c = arr[i];
		var chc = c.GetHashCode();
		var phc = chc * (i + 1);
		var n = (int)c;

		hc += phc;
		
		if(!printDebug) continue;
		
		Console.WriteLine($"[{i}]");
		Console.WriteLine($"CHAR [{c}] CHC [{chc}] PHC [{phc}]");
		Console.WriteLine($"ASCII [{n}] HC [{n.GetHashCode()}]");
	}

	Console.WriteLine($"hc = {hc:n0}");

	return hc;
}

