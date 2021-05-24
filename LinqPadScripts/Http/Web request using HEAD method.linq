<Query Kind="Program">
  <Namespace>System.Net</Namespace>
</Query>

private const string UrlGitHub = "https://github.com/Nexus-Mods/Nexus-Mod-Manager/releases/download/0.65.7/Nexus.Mod.Manager-0.65.7.exe";
private const string UrlAmazon = "https://github-production-release-asset-2e65be.s3.amazonaws.com/57035078/099e5e52-7f70-11e8-90fb-21ac6d4c7540?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=AKIAIWNJYAX4CSVEH53A%2F20180708%2Fus-east-1%2Fs3%2Faws4_request&X-Amz-Date=20180708T173642Z&X-Amz-Expires=300&X-Amz-Signature=8c84192bcb96c047f9f3a7c99b6e532aa47527e16d5c8da1354dc6ca23fab187&X-Amz-SignedHeaders=host&actor_id=0&response-content-disposition=attachment%3B%20filename%3DNexus.Mod.Manager-0.65.7.exe&response-content-type=application%2Foctet-stream";

void Main()
{
//	NmmWebRequestGitHub();
//	
//	NmmWebRequestAmazon();

	using (var r = MakeHeadWebRequest(UrlGitHub))
	{
		if(r.Success)
		{
			Console.WriteLine("Successfully contacted");

			r.Response.Dump();
			
			return;
		}

		r.Dump();

		using (var r2 = MakeHeadWebRequest(r.UriResponse))
		{
			r2.Dump();
		}
	}
}

private ResponseResult MakeHeadWebRequest(string url)
{
	var uri = new Uri(url);
	
	return MakeHeadWebRequest(uri);
}

private ResponseResult MakeHeadWebRequest(Uri uri)
{
	var wr = (HttpWebRequest)WebRequest.Create(uri);

	void H(string key, string value)
	{
		wr.Headers.Add(key, value);
	}

	//wr.Method = "HEAD";
	wr.Method = "GET";
	wr.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36";
	wr.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
	wr.AddRange(0, 1);
	wr.AllowAutoRedirect = true;
	wr.KeepAlive = true;
	wr.Host = uri.Host;

	H("Accept-Language", "en-US,en;q=0.9");
	H("Cache-Control", "no-cache");
	H("Pragma", "no-cache");
	H("Upgrade-Insecure-Requests", "1");

	//These cannot be used with HEAD
	//wr.SendChunked = true;
	//wr.TransferEncoding = "gzip, deflate, br";

	try
	{
		var r = (HttpWebResponse)wr.GetResponse();
		
		return new ResponseResult(r, uri);
	}
	catch (WebException wex)
	{
		return new ResponseResult(wex, uri);
	}
	catch (Exception ex)
	{
		ex.Dump();
		
		throw;
	}
}

private void NmmWebRequestGitHub()
{
	#region Header info Request 1
	//Accept: text / html,application / xhtml + xml,application / xml; q = 0.9,image / webp,image / apng,*/*;q=0.8
	//Accept-Encoding: gzip, deflate, br
	//Accept-Language: en-US,en;q=0.9
	//Cache-Control: no-cache
	//Connection: keep-alive
	//Host: github.com
	//Pragma: no-cache
	//Upgrade-Insecure-Requests: 1
	//User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36
	#endregion

	var wr = (HttpWebRequest)WebRequest.Create(UrlGitHub);

	void H(string key, string value)
	{
		wr.Headers.Add(key, value);
	}

	wr.Method = "HEAD";
	wr.AddRange(0, 1);
	wr.AllowAutoRedirect = true;
	wr.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
	wr.KeepAlive = true;
	wr.Host = "github.com";
	wr.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36";

	H("Accept-Language", "en-US,en;q=0.9");
	H("Cache-Control", "no-cache");
	H("Pragma", "no-cache");
	H("Upgrade-Insecure-Requests", "1");

	//These cannot be used with HEAD
	//wr.SendChunked = true;
	//wr.TransferEncoding = "gzip, deflate, br";

	try
	{
		using (var wrpFileMetadata = (HttpWebResponse)wr.GetResponse())
		{
			if (wrpFileMetadata.StatusCode == HttpStatusCode.OK ||
				wrpFileMetadata.StatusCode == HttpStatusCode.PartialContent)
			{
				Console.WriteLine("Success");

				wrpFileMetadata.Headers.Dump();
			}
		}
	}
	catch (Exception ex)
	{
		ex.Dump();
	}
}

private void NmmWebRequestAmazon()
{
	var wr = (HttpWebRequest)WebRequest.Create(UrlAmazon);

	void H(string key, string value)
	{
		wr.Headers.Add(key, value);
	}

	wr.Method = "HEAD";
	wr.AddRange(0, 1);
	wr.AllowAutoRedirect = true;
	wr.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
	wr.KeepAlive = true;
	wr.Host = "github-production-release-asset-2e65be.s3.amazonaws.com";
	wr.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36";

	H("Accept-Language", "en-US,en;q=0.9");
	H("Cache-Control", "no-cache");
	H("Pragma", "no-cache");
	H("Upgrade-Insecure-Requests", "1");

	try
	{
		using (var wrpFileMetadata = (HttpWebResponse)wr.GetResponse())
		{
			if (wrpFileMetadata.StatusCode == HttpStatusCode.OK ||
				wrpFileMetadata.StatusCode == HttpStatusCode.PartialContent)
			{
				Console.WriteLine("Success");

				wrpFileMetadata.Headers.Dump();
			}
		}
	}
	catch (Exception ex)
	{
		ex.Dump();
	}
}

public class ResponseResult
	: IDisposable
{
	public ResponseResult(WebException webException, Uri uri) 
		: this((HttpWebResponse)webException.Response, uri)
	{
		Success = false;	
	}
	
	public ResponseResult(HttpWebResponse response, Uri uri)
	{
		Success = true;

		Response = response;
		
		UriRequest = uri;
	}

	public HttpWebResponse Response { get; set; }

	public Uri UriRequest { get; set; }
	
	public Uri UriResponse => Response.ResponseUri;

	public bool Success { get; set; }
	
	public void Dispose()
	{
		Response?.Dispose();
	}
}

// Define other methods and classes here
private void DownloadFile()
{
	using (var w = new System.Net.WebClient())
	{
		w.DownloadFile(UrlGitHub, @"J:\Dump\NMM Logs\NMM.exe");
	}
}