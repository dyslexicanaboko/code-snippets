<Query Kind="Program" />

void Main()
{
	var a = "Dream Keeps Making Things Worse (ft. Kwite) Some Ordinary Podcast #103.mp3";
	var b = "Dream Keeps Making Things Worse (ft. Kwite) Some Ordinary Podcast #103.mp3";
	
	CompareCharsAndBytes(a, b);
}

public (char[], int[]) GetArrays(string target)
	=> (target.ToCharArray(),
		target.Select(x => (int)x).ToArray());

public void CompareCharsAndBytes(string left, string right)
{
	Console.WriteLine($"Are strings equal? {left == right}");

	var (charsA, bytesA) = GetArrays(left);
	var (charsB, bytesB) = GetArrays(right);

	Console.WriteLine($"Is char sequence equal? {charsA.SequenceEqual(charsB)}");

	var lst = new List<dynamic>(charsA.Length);

	for (var i = 0; i < charsA.Length; i++)
	{
		lst.Add(new
		{
			CharA = charsA[i],
			CharB = charsB[i],
			ByteA = bytesA[i],
			ByteB = bytesB[i],
			Equal = bytesA[i] == bytesB[i]
		});
	}

	lst.Dump();
}

public void ListCharsAndBytes(string target)
{
	var (charsA, bytesA) = GetArrays(target);

	var lst = new List<dynamic>(charsA.Length);

	for (var i = 0; i < charsA.Length; i++)
	{
		lst.Add(new
		{
			CharT = charsA[i],
			ByteT = bytesA[i],
		});
	}

	lst.Dump();
}

