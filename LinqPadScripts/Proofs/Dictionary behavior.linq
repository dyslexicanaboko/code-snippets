<Query Kind="Program" />

void Main()
{
	//var blah = new int[5];
	//
	//blah.Dump();

	var lst = new List<int>() { 1 };
	Console.WriteLine(lst.Remove(1));
	Console.WriteLine(lst.Remove(1));
	lst.Dump();


	var lst2 = new List<int>(10) { 1, 2, 3, 4, 5, 6 };
	lst2.Dump();

	var dict = new Dictionary<int, int> { { 1, 1 }, { 1, 1 }, { 1, 1 } };
	//var dict = new Dictionary<N, N> { {new N(1), new N(1)}, {new N(1), new N(1)},{new N(1), new N(1)},};
	
	foreach (var kvp in dict)
	{
		//kvp.Key.Number++;
		//kvp.Value.Number++;
		//kvp.Key++;
		//kvp.Value++;
	}
	
	dict.Dump();
}

// You can define other methods, fields, classes and namespaces here
public class N
{
	public N(int number)
	{
		Number = number;	
	}
	public int Number { get; set; }

	public override bool Equals(object obj)
	{
		return Number == ((N)obj).Number;
	}

	public override int GetHashCode()
	{
		return Number;
	}
}