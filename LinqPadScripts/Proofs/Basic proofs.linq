<Query Kind="Program" />

void Main()
{
	//var l = GetList();
	
	//DeleteLastFive(l);
	
	//l.Dump();
	
	//DeleteOnForeachFails();
	
	//BasicLinqQuery();
	
	//TwoDimArray();
	
	JaggedArray();
}

public List<int> GetList()
{
	var length = 10;

	var lst = new List<int>(length);

	for (var i = 0; i < length; i++)
	{
		lst.Add(i);
	}

	return lst;
}

public void DeleteLastFive(List<int> numbers)
{
	if(numbers.Count < 5) return; //Or throw exception
	
	for (var i = numbers.Count - 1; i > 4; i--)
	{
		numbers.RemoveAt(i);
	}
	
	numbers.Dump();
}

public void DeleteOnForeachFails()
{
	var l = GetList();
	
	foreach (var n in l)
	{
		l.RemoveAt(n);
	}
}

public void BasicLinqQuery()
{
	var l = GetList();
	
	var q = (from n in l
			 where n > 5
			 select	n).ToList();
			 
	q.Dump();
}

public void TwoDimArray()
{
	var r = 10;
	var c = 10;
	
	var tda = new int[r, c];
	
	for (int i = 0; i < r; i++)
	{
		for (int j = 0; j < c; j++)
		{
			tda[i, j] = j;
		}
	}
	
	tda.Dump();
}

public void JaggedArray()
{
	var r = 10;

	var ja = new int[r][];

	for (int i = 0; i < r; i++)
	{
		var c = i + 1;
		
		ja[i] = new int[c];
		
		for (int j = 0; j < c; j++)
		{
			ja[i][j] = j;
		}
	}

	ja.Dump();
}
