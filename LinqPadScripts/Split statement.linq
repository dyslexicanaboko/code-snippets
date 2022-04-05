<Query Kind="Program" />

void Main()
{
	var split = SplitStatement(0.5m, 3413.85m, 206.85m, 679.20m);
	
	split.ToString().Dump();
}

// You can define other methods, fields, classes and namespaces here
public AdjustedTotal SplitStatement(decimal splitRate, decimal statementTotal, params decimal[] exclusions)
{
	statementTotal = MakeNegative(statementTotal);

	var exclusionTotal = 0m;

	foreach (var e in exclusions)
	{
		exclusionTotal += MakeNegative(e);
	}

	var adjustedTotal = statementTotal - exclusionTotal;
	
	var higherSplit = adjustedTotal*splitRate;
	
	var lowerSplit = adjustedTotal - higherSplit + exclusionTotal;

	var r = new AdjustedTotal
	{
		SplitRate = splitRate,
		SplitHigh = higherSplit,
		SplitLow = lowerSplit,
		BalanceOriginal = statementTotal,
		BalanceAdjusted = adjustedTotal,
		Exclusions = exclusionTotal
	};
	
	return r;
}

public decimal MakeNegative(decimal target)
{
	if(target < 0) return target;
	
	target = -target;
	
	return target;
}

public class AdjustedTotal
{
	public decimal SplitRate { get; set; }
	public decimal SplitHigh { get; set; }
	public decimal SplitLow { get; set; }
	public decimal BalanceOriginal { get; set; }
	public decimal BalanceAdjusted { get; set; }
	public decimal Exclusions { get; set; }

	public override string ToString()
	{
		var s = $"Balance: {BalanceOriginal:c2} -> High: {SplitHigh:c2} | Low: {SplitLow:c2} @ {SplitRate:p2}{Environment.NewLine}" +
		$"Adjusted: {BalanceAdjusted:c2} without exclusions {Exclusions:c2}";
		
		return s;
	}
}