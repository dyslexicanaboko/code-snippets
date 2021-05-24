<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Security</Namespace>
</Query>

//This script is incomplete, it's supposed to be an asynchronous version of the same script
//Direct URL access via load balancer
void Main()
{
	var lst = new List<string>()
	{
		"ClientA",
		"ClientB",
		"ClientC"
	};

	var lstDetail = new List<ServerDetail>(lst.Count);

	foreach (string client in lst)
	{
		lstDetail.Add(new ServerDetail()
		{
			Client = client,
			Uri = $"https://{client}.site.domain.net/startup.html"
		});
	}
	
	//lstDetail.Dump();
	
	CheckStatus(lstDetail).Dump();
}

public class ServerDetail
{
	public string Client { get; set; }
	public string Uri { get; set; }
}

public class ServerStatusResult : ServerDetail
{
	public ServerStatusResult(ServerDetail detail)
	{
		Client = detail.Client;
		Uri = detail.Uri;
		IsSiteAccessible = false;
		Error = string.Empty;
	}

	public DateTime Start { get; set; }
	
	public DateTime End { get; set; }

	public TimeSpan Elapsed { get { return End - Start; } }

	public bool IsSiteAccessible { get; set; }

	public string Error { get; set; }
}

public List<ServerStatusResult> CheckStatus(List<ServerDetail> lstDetails)
{
	var tasks = new List<Task<ServerStatusResult>>();

	Parallel.ForEach(lstDetails, (detail) => {
		tasks.Add(Task<ServerStatusResult>.Factory.StartNew((object obj) => IsSiteAccessible(obj as ServerDetail), detail));
	});

	Task.WaitAll(tasks.ToArray());

	return tasks.Select(x => x.Result).ToList();
}

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
	
	result.End = DateTime.Now;
	
	Console.Write(".");

	return result;
}