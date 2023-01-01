<Query Kind="Program">
  <Namespace>System.Collections.Concurrent</Namespace>
</Query>

void Main()
{
	var dict = new ConcurrentDictionary<string, int>();

	for (int i = 0; i < 100; i++)
	{
		//Key
		//Value for Add
		//Factory for update
		dict.AddOrUpdate("a", 0, (k, v) => v + 5);
	}
	
	dict.Dump();
}

// You can define other methods, fields, classes and namespaces here