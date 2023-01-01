<Query Kind="Program" />

void Main()
{
	//20 elements, batch size 5
	//Even 20/5 = 4, R0
	//0 - 00 - 4
	//1 - 05 - 9
	//2 - 10 - 14
	//3 - 15 - 19

	//23 elements, batch size 5
	//Odd 23/5 ~ 4, R1
	//0 - 00 - 4
	//1 - 05 - 9
	//2 - 10 - 14
	//3 - 15 - 19
	//4 - 20 - 23

	var batchSize = 5;
	var sequenceSize = 20;

	DotNet4_8Below(sequenceSize, batchSize);

	DotNet6Plus(sequenceSize, batchSize);
}

//dot net 4.8 and below do not have access to the Linq.Take(...) range overload
private void DotNet4_8Below(int sequenceSize, int batchSize)
{
	var lst = Enumerable.Range(0, sequenceSize).ToList();

	var isEven = lst.Count % batchSize == 0;
	var batches = lst.Count / batchSize;

	if (!isEven) batches++;

	for (var b = 0; b < batches; b++)
	{
		var i = b * batchSize;
		var e = i + batchSize;
		
		if(e > lst.Count) e = lst.Count;
		
		for (; i < e; i++)
		{
			Console.WriteLine(lst[i]);
		}
	}
}

//dot net 6 plus has access to the Take(i, e) overload.
private void DotNet6Plus(int sequenceSize, int batchSize)
{
	var lst = Enumerable.Range(0, sequenceSize).ToList();

	var isEven = lst.Count % batchSize == 0;
	var batches = lst.Count / batchSize;

	if (!isEven) batches++;

	for (var b = 0; b < batches; b++)
	{
		var i = b * batchSize;
		var e = i + batchSize;

		if (e > lst.Count) e = lst.Count;

		//This method protects against index out of bounds, but don't use that as a crutch
		lst.Take(new Range(new Index(i), new Index(e))).Dump();
	}
}
