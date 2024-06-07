<Query Kind="Program" />

private Random _random = new Random();

void Main()
{
	for (int i = 0; i < 10; i++)
	{
		GetPin(1, 6, 8).Dump();
	}
}

public string GetPin(int digitMin, int digitMax, int length)
{
	var arr = new int[length];
	
	for (int i = 0; i < length; i++)
	{
		arr[i] = _random.Next(digitMin, digitMax);
	}
	
	return string.Join(string.Empty, arr);
}

// Define other methods and classes here
