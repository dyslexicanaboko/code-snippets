<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Security</Namespace>
</Query>

//Server and client version of script
void Main()
{
	var lst = new List<string>()
	{
		"ClientA",
		"ClientB",
		"ClientC"
	};

	var lstDetail = new List<ServerDetail>();

	var lstServers = new List<string>()
	{
		"10.0.0.1",
		"10.0.0.2",
		"10.0.0.3"
	};

	foreach (string client in lst)
	{
		foreach (string server in lstServers)
		{
			lstDetail.Add(new ServerDetail()
			{
				Server = server,
				Client = client,
				Uri = $"http://{server}/{client}/startup.html"
			});
		}
	}
	
	CheckStatus(lstDetail).Dump();
}

public class ServerDetail
{
	public string Server { get; set; }
	public string Client { get; set; }
	public string Uri { get; set; }
}


private static int _index = 0;
private static object _index_lock = new object();
private static int GetIndex()
{
	lock (_index_lock)
	{
		return _index++;
	}
}

public class ServerStatusResult : ServerDetail
{
	public ServerStatusResult(ServerDetail detail)
	{
		Server = detail.Server;
		Client = detail.Client;
		Uri = detail.Uri;
		IsSiteAccessible = false;
		Error = string.Empty;
	}

	public int Index { get; set; }

	public DateTime Start { get; set; }
	
	public DateTime End { get; set; }

	public TimeSpan Elapsed { get { return End - Start; } }

	public bool IsSiteAccessible { get; set; }

	public string Error { get; set; }
}

public List<ServerStatusResult> CheckStatus(List<ServerDetail> lstDetails)
{
	var tasks = new List<ServerStatusResult>();

	Parallel.ForEach(lstDetails, (detail) => {
		tasks.Add(IsSiteAccessible(detail));
	});

	return tasks.ToList();
}

// Define other methods and classes here
//public async Task<ServerStatusResult> IsSiteAccessible(ServerDetail detail)
public ServerStatusResult IsSiteAccessible(ServerDetail detail)
{
	var result = new ServerStatusResult(detail);

	result.IsSiteAccessible = false;
	result.Start = DateTime.Now;
	
	try
	{
		string strPage = null;

		if (string.IsNullOrWhiteSpace(detail.Uri))
			result.Error = "Server URI is blank or null";
		else
		{
			using (var wc = new WebClient())
			{
				//Not actually using SSL right now so I am going to just not do this
				//				try
				//				{
				//					//Change SSL checks so that all checks pass
				//					ServicePointManager.ServerCertificateValidationCallback =
				//						new RemoteCertificateValidationCallback(
				//							delegate
				//							{ return true; }
				//						);
				//				}
				//				catch
				//				{
				//
				//				}

				//strPage = await wc.DownloadStringTaskAsync(detail.URI);
				strPage = wc.DownloadString(detail.Uri);
			}

			result.IsSiteAccessible = strPage.Contains("Application is initalizing");
		}
	}
	catch (WebException wex)
	{
		if (wex.Response != null)
		{
			HttpWebResponse response = (HttpWebResponse)wex.Response;

			result.Error = string.Format("Code: {0} - {1} - {2}, Error: {3}", response.StatusCode.ToString(), (int)response.StatusCode, response.StatusDescription, wex.Message);
		}

		result.Error = wex.Message;
	}
	catch (Exception ex)
	{
		result.Error = ex.Message;
	}
	
	result.Index = GetIndex();
	result.End = DateTime.Now;
	
	Console.Write(".");

	return result;
}