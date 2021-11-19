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
const int ConcertId = 1;
/*
	The goal here is to hammer the database for ticket requests and 
	make sure no one is able to assign the same ticket more than one time.
*/
void Main()
{
	HammerTicketSales();
}

ConcurrentBag<Result> _results = null;

private void HammerTicketSales()
{	
	ResetData();
	
	var lst = Purchasers.ToList();

	HammerTicketSales(1, lst, ConcertId);

	Console.WriteLine("Finished");
}

private void ResetData()
{
	ExecuteCommand("ResetTickets @concertId = {0}", ConcertId);

	if (!Tickets.Any())
	{
		for (int i = 0; i < 10; i++)
		{
			Tickets.InsertOnSubmit(new Ticket { ConcertId = ConcertId });
		}
		
		SubmitChanges();
	}
}

private void HammerTicketSales(int run, List<Purchaser> purchasers, int concertId)
{
//	var remainingTickets = Tickets.Count(x => x.ConcertId == concertId && x.PurchasedOn == null);
//
//	if (remainingTickets == 0)
//	{
//		Console.WriteLine("Tickets are sold out.");
//		
//		return;
//	}

	_results = new ConcurrentBag<Result>();

	Console.WriteLine($"Threads: {purchasers.Count}");
	
	//Using Parallel ForEach so that the execution is randomized naturally
	Parallel.ForEach(purchasers, x => PurchaseTicket(x, concertId));

	Console.WriteLine();
	Console.WriteLine();

	//Ticket count from the database
	var dbTickets = Tickets.ToList();

	//Number of tickets sold according to the program
	var soldMem = _results.Count(x => x.IsSuccessful);

	//Number of tickets sold according to the database (LILO)
	var soldDb = (
		from p in Purchasers 
		join t in Tickets on p.PurchaserId equals t.PurchaserId 
		select p.PurchaserId).Count();

	Console.WriteLine("Did everyone get a single ticket?");
	Console.WriteLine("---------------------------------");
	Console.WriteLine($"Total Tickets    : {dbTickets.Count}");
	Console.WriteLine($"Total Purchasers : {purchasers.Count}");
	Console.WriteLine($"Sold in Database : {soldDb}");
	Console.WriteLine($"Sold in Memory   : {soldMem}");

	Console.WriteLine("\nHow many purchasers have purchased the same ticket?");

	//How many purchasers have purchased the same ticket?
	var screwUps = _results
		.GroupBy(x => x.TicketId)
		.Select(x => new
		{
			TicketId = x.Key,
			Purchasers = x.Count()
		}).ToList();
		
	screwUps.Dump();
	
	Console.WriteLine("\n\nRaw results in memory");
	
	//Raw results to look at
	_results.Dump();
	
	Console.WriteLine("\n\nResults from database");
	
	dbTickets.Dump();
}

// Define other methods and classes here
private void PurchaseTicket(Purchaser purchaser, int concertId)
{	
	//Console.WriteLine(purchaser.PurchaserId);
	var p = purchaser;

	Console.Write($"{p.PurchaserId} ");

	var r = new Result
	{
		PurchaserId = p.PurchaserId,
		FirstName = p.FirstName,
		LastName = p.LastName
	};
	
	try
	{
		var dt = Exec("[dbo].[ClaimTickets]",
			concertId,
			p.PurchaserId,
			1); //amount

		var dr = dt.Rows[0];

		r.IsSuccessful = true;
		r.TicketId = dr.Field<int>("TicketId");
		r.PurchasedOn = dr.Field<DateTime>("PurchasedOn");
		r.ConcertId = dr.Field<int>("ConcertId");
		r.ConcertName = dr.Field<string>("Concert");
	}
	catch (Exception ex)
	{
		//Console.WriteLine($"[{p.PurchaserId}] -> {ex.Message}");
		r.IsSuccessful = false;
		r.ErrorMessage = ex.Message;
	}
	
	_results.Add(r);
}

private DataTable Exec(string storedProcedureName, params object[] parameters)
{
	using(var con = new SqlConnection(Connection.ConnectionString))
	{
		con.Open();
		
		using (var cmd = new SqlCommand(storedProcedureName, con))
		{
			cmd.CommandTimeout = 30;
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("concertId", parameters[0]));
			cmd.Parameters.Add(new SqlParameter("purchaserId", parameters[1]));
			cmd.Parameters.Add(new SqlParameter("amount", parameters[2]));

			using (var da = new SqlDataAdapter(cmd))
			{
				var ds = new DataSet();
				
				da.Fill(ds);
				
				var dt = ds.Tables[0];
				
				return dt;
			}
		}
	}
}

public class Result
	: Purchaser
{
	public int TicketId { get; set; }

	public int ConcertId { get; set; }
	
	public string ConcertName { get; set; }

	public DateTime PurchasedOn { get; set; }
	
	public bool IsSuccessful { get; set; }
	
	public string ErrorMessage { get; set; }

	public Purchaser GetAsPurchaser()
	{
		var p = new Purchaser
		{
			PurchaserId = PurchaserId,
			FirstName = FirstName,
			LastName = LastName
		};
		
		return p;
	}
}