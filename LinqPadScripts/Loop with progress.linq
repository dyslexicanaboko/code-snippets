<Query Kind="Program" />

void Main()
{
	var lst = new List<int>(100);
		
	var r = new Random();	
		
	for (int i = 0; i < lst.Capacity; i++)
	{
		lst.Add(r.Next());
	}
	
	LoopWithProgress(lst, "Test", (x) => {});
}

// This is not perfect and it is meant for long running processes
public void LoopWithProgress<T>(List<T> data, string processMessage, Action<T> method, string progressTick = "|")
{
	Console.WriteLine($"{processMessage} ({data.Count:n0}):");

	//Show a progress tick every 1%
	var frequency = Convert.ToInt32(data.Count * 0.01);

	//Count between progress ticks
	var minor = 0;

	//Put back into query
	for (int i = 0; i < data.Count; i++)
	{
		var obj = data[i];

		method(obj);

		if (minor == frequency)
		{
			Console.Write(progressTick);

			minor = 0;
		}
		else
			minor++;
	}

	Console.WriteLine(" - Finished");
}