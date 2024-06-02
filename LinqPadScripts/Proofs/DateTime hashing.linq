<Query Kind="Program" />

void Main()
{
	GetHash(DateTime.Parse("0001-01-01")).Dump();
	GetHash(DateTime.Parse("2000-12-01")).Dump();
	GetHash(DateTime.Parse("3000-01-01")).Dump();
}

public int GetHash(DateTime dtm)
{
	//DateTime.Today.GetHashCode().Dump();
	//Int64 ticks = InternalTicks;
	//return unchecked((int)ticks) ^ (int)(ticks >> 32);

	Int64 ticks = dtm.Ticks;
	//Console.WriteLine($"Ticks: {ticks}");
	//Console.WriteLine($"Shift: {(ticks >> 32)}");
	//Console.WriteLine($"Cast : {((int)ticks)}");

	return unchecked((int)ticks ^ (int)(ticks >> 32));
}
