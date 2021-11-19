<Query Kind="Program">
  <Connection>
    <ID>840a0411-106f-4328-a770-2618871991a5</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Server>.</Server>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Semaphores</Database>
  </Connection>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
</Query>

//const int MaxTries = 10;

/*
	The goal here is to hammer the database for filling exclusive slots on the same row at the same time.
	The expectation is that depending on the isolation level you should be able to fill the appropriate slot
	without interfering with a different transaction filling an adjacent slot.
*/
void Main()
{
	ResetData();

	FillSlots();

	Console.WriteLine("Finished");
}

ConcurrentBag<RowAndSlot> _results = null;

private void ResetData()
{
	ExecuteCommand("[dbo].[ResetExclusiveSlots]");
}

//Insert row index into its corresponding spot per row
private List<RowAndSlot> OneToOneStrategy(int rows)
{
	var lst = new List<RowAndSlot>();

	//Generate rows and slot indicies
	for (int r = 1; r <= rows; r++)
	{
		for (int s = 0; s < 10; s++)
		{
			var row = new RowAndSlot(r, s, s);

			lst.Add(row);
		}
	}
	
	return lst;
}

//Use row one repeatedly and hammer it with slot updates N times
private List<RowAndSlot> HammerOneRowStrategy(int repetition)
{
	var lst = new List<RowAndSlot>();

	var v = 0;

	for (int r = 0; r < repetition; r++)
	{
		for (int s = 0; s < 10; s++)
		{
			var row = new RowAndSlot(1, s, v++);

			lst.Add(row);
		}
	}
	
	return lst;
}

private void FillSlots()
{
	_results = new ConcurrentBag<RowAndSlot>();

	//var lst = OneToOneStrategy(10);
	var lst = HammerOneRowStrategy(10);

	Console.WriteLine($"Threads: {lst.Count}");

	//Using Parallel ForEach so that the execution is randomized naturally
	Parallel.ForEach(lst, x => FillSlot(x));

	Console.WriteLine();
	Console.WriteLine();
	
	Console.WriteLine("\n\nRaw results in memory");
	
	//Raw results to look at
	_results.Where(x => !x.IsSuccessful).Dump();
	
	Console.WriteLine("\n\nResults from database");
	
	ExclusiveSlots.Dump();
}

// Define other methods and classes here
private void FillSlot(RowAndSlot row)
{
	Console.Write($"({row.RowNumber}, {row.SlotIndex}) | ");
	
	var r = row;
	
	try
	{
		Exec("[dbo].[FillExclusiveSlot]",
			row.RowNumber,
			row.SlotIndex,
			row.Value);

		r.IsSuccessful = true;
		r.FilledOn = DateTime.UtcNow;
	}
	catch (Exception ex)
	{
		r.IsSuccessful = false;
		r.ErrorMessage = ex.Message;
		r.HResult = ex.HResult;

		if (ex is SqlException)
		{
			var sqlEx = (SqlException)ex;
			
			r.ErrorCode = sqlEx.ErrorCode;
			r.Number = sqlEx.Number;
		}
	}
	
	_results.Add(r);
}

private void Exec(string storedProcedureName, params object[] parameters)
{
	using(var con = new SqlConnection(Connection.ConnectionString))
	{
		con.Open();
		
		using (var cmd = new SqlCommand(storedProcedureName, con))
		{
			cmd.CommandTimeout = 30;
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("exclusiveSlotId", parameters[0]));
			cmd.Parameters.Add(new SqlParameter("slotIndex", parameters[1]));
			
			cmd.ExecuteNonQuery();
		}
	}
}

public class RowAndSlot
{
	public RowAndSlot() { }
	
	public RowAndSlot(int rowNumber, int slotIndex, int value) 
	{ 
		RowNumber = rowNumber;

		SlotIndex = slotIndex;
		
		Value = value;
	}

	public int RowNumber { get; set; }

	public int SlotIndex { get; set; }
	
	public int Value { get; set; }

	public DateTime FilledOn { get; set; }
	
	public bool IsSuccessful { get; set; }
	
	public string ErrorMessage { get; set; }

	public int HResult { get; set; }
	
	public int ErrorCode { get; set; }
	
	public int Number { get; set; }
}