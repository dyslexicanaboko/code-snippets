<Query Kind="Program" />

void Main()
{
	Delta(0, 0);
	Delta(-10376.43M, 1200.26M);
	Delta(1200.26M, -10376.43M);
	Delta(2000, 1000);
	Delta(-2400, -5000);
}

// Define other methods, classes and namespaces here
public void Delta(decimal lastMonth, decimal thisMonth)
{
	var tm = thisMonth;
	var lm = lastMonth;
	decimal diff;
	
	if (tm < 0 && lm < 0)
	{
		if (tm > lm)
		{
			diff = Math.Abs(tm) - Math.Abs(lm);
		}
		else
		{
			diff = Math.Abs(lm) - Math.Abs(tm);
		}
		
		diff *= -1;
	}
	else if (tm >= 0 && lm >= 0)
	{
		if (tm > lm)
		{
			diff = tm - lm;
		}
		else
		{
			diff = lm - tm;
		}
	}
	else
	{
		diff = tm + lm;
	}

	diff.Dump();
}