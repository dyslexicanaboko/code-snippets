<Query Kind="Program" />

void Main()
{
	//PrintBeforeAndAfter_string();
	
	//Console.WriteLine();
	
	PrintBeforeAndAfter_String();
}

public void PrintBeforeAndAfter_string()
{
	Console.WriteLine("string");
	
	string s = "One Two Three Four";
	
	Console.WriteLine(s);

	ChangeTextMethod_string(s);

	Console.WriteLine(s);
}

public void PrintBeforeAndAfter_String()
{
	Console.WriteLine("String");
	
	String s = "One Two Three Four";

	Console.WriteLine(s);

	ChangeTextMethod_String(s);

	Console.WriteLine(s);

	ChangeTextMethod_string(s);

	Console.WriteLine(s);
}

// Define other methods and classes here
public void ChangeTextMethod_string(string s)
{
	s = " Five";
}

public void ChangeTextMethod_String(String s)
{
	s = " Five";
}