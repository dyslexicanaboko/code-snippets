<Query Kind="Program" />

void Main()
{
	//Removing exclusions from balance entirely
	var adjustedBalance = 4648.03m - 2047.87m;

	var split = SplitStatement(0.5m, adjustedBalance);
	//var split = SplitStatement(0.5m, 4648.03m, 2000m, 47.87m);
	
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
	
	var split1 = adjustedTotal*splitRate;
	
	var split2 = adjustedTotal - split1 + exclusionTotal;

	var r = new AdjustedTotal
	{
		SplitRate = splitRate,
		Split1 = split1,
		Split2 = split2,
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
	public decimal Split1 { get; set; }
	public decimal Split2 { get; set; }
	public decimal BalanceOriginal { get; set; }
	public decimal BalanceAdjusted { get; set; }
	public decimal Exclusions { get; set; }

	public override string ToString()
	{
		var s = $"Balance: {BalanceOriginal:c2} -> Split1: {Split1:c2} | Split2: {Split2:c2} @ {SplitRate:p2}{Environment.NewLine}" +
		$"Adjusted: {BalanceAdjusted:c2} without exclusions {Exclusions:c2}";
		
		return s;
	}
}