<Query Kind="Program">
  <NuGetReference>YahooQuotesApi</NuGetReference>
  <Namespace>NodaTime</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>YahooQuotesApi</Namespace>
</Query>

async Task Main()
{
	var symbol = "PCTY";

	var dtm = GetMostRecentWeekday();

	var quotes = new YahooQuotesBuilder()
	.WithHistoryStartDate(Instant.FromDateTimeUtc(dtm))
	.Build();

	var item = await quotes.GetAsync(symbol, HistoryFlags.PriceHistory) ?? throw new ArgumentException("Unknown symbol");

	var history = item.PriceHistory.Value.First();

	var sq = new StockQuote
	{
		Symbol = symbol,
		CompanyName = item.LongName,
		Bid = item.Bid,
		Ask = item.Ask,
		LastDate = new DateTime(history.Date.Year, history.Date.Month, history.Date.Day),
		LastPrice = history.Close
	};
	
	sq.Dump();
}

public class StockQuote
{
	public string Symbol { get; set; }
	public string CompanyName { get; set; }
	public decimal Bid { get; set; }
	public decimal Ask { get; set; }
	public DateTime LastDate { get; set; }
	public double LastPrice { get; set; }
}

public DateTime GetMostRecentWeekday()
{
	var dtm = DateTime.UtcNow.Date;

	//If it's the weekend then return Friday, not sure how to handle holidays yet
	//I do have my American holiday code
	if (dtm.DayOfWeek != DayOfWeek.Saturday && dtm.DayOfWeek != DayOfWeek.Sunday) return dtm;
	
	while(dtm.DayOfWeek != DayOfWeek.Friday) dtm = dtm.AddDays(-1);
	
	return dtm;
}
