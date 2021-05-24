<Query Kind="Program">
  <NuGetReference>SSH.NET</NuGetReference>
  <Namespace>Renci.SshNet</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

void Main()
{
	Console.WriteLine($"Last executed: {DateTime.Now.ToString()}");

	FtpLongList();
	//FtpDebug();

	
	
	Console.WriteLine("Finished");
}

private void UploadExample()
{
	var builder = new UriBuilder("ftp", "ServerA");

	builder.Uri.Dump();
	//"%2Froot/dirA/dirB" //%2F is a hack to reference an absolute path of a remote path
	builder.Path = "Home/dirA";

	var str = builder.ToString();

	var u = new UploadInstructions
	{
		RemotePath = str,
		FilesToUpload = new List<string>
		{
			@"C:\Dump\Test.txt"
		}
	};

	Upload(u);
}

public void MakeCopiesForRetest(string sourceFile, string destinationPath, string saveAsNameOnly, string extension, int copies)
{
	for (int i = 0; i < copies; i++)
	{
		var saveAs = Path.Combine(destinationPath, saveAsNameOnly + i + "." + extension);

		File.Copy(sourceFile, saveAs, true);
	}
}

public void FtpLongList()
{
	var request = GetFtpWebRequest(GetServerAUri());

	request.Method = WebRequestMethods.Ftp.ListDirectory;

	using(var response = GetResponse(request))
	{
		ReadResponse(response);
	}
}

public void FtpDebug()
{
	var request = GetFtpWebRequest(GetServerAUri());

	request.Method = WebRequestMethods.Ftp.ListDirectory;

	using (var response = GetResponse(request))
	{
		ReadResponse(response);
	}
}

private FtpWebResponse GetResponse(FtpWebRequest request)
{
	var response = (FtpWebResponse)request.GetResponse();

	return response;
}

private void ReadResponse(FtpWebResponse response)
{
	var r = response;

	Console.WriteLine($"ResponseUri: {r.ResponseUri}\nStatusCode: {r.StatusCode}\nStatusDescription:{r.StatusDescription}");
	
	using (var ftpResponseStream = response.GetResponseStream())
	{
		if (ftpResponseStream != null)
		{
			using (var reader = new StreamReader(ftpResponseStream))
			{
				//If you don't see anything, make sure to upload some files if you haven't already
				var str = reader.ReadToEnd();

				str.Dump();
			}
		}
	}
}

//Was using this for the FTP long list tests
//%2F is how you represent an absolute path for an FTP path, replace the leading slash
//https://stackoverflow.com/questions/1162053/any-way-to-specify-absolute-paths-in-ftp-urls
//https://tools.ietf.org/html/draft-casey-url-ftp-00
public string GetServerAUri()
{
	var builder = new UriBuilder("ftp", "server");
	
	builder.Uri.Dump();
	builder.Path = "%2FnotHome/dirA";
	
	var str = builder.ToString();
	
	return str;
}


public FtpWebRequest GetFtpWebRequest(string remotePath)
{
	var request = (FtpWebRequest)WebRequest.Create(remotePath);
	request.Credentials = new NetworkCredential("username", "password");

	return request;
}

#region Junky Code
//I got this Junky Code from somewhere else and most of it isn't very good
//I have a better version of this elsewhere, I need to replace this later
public List<string> GetDirectoryListing(string remotePath)
{
	var request = GetFtpWebRequest(remotePath);

	request.Method = WebRequestMethods.Ftp.ListDirectory;

	using (var response = GetFtpWebResponse(request))
	{
		var files = new List<string>();

		using (var stream = response.GetResponseStream())
		{
			if (stream == null) return files;

			using (var reader = new StreamReader(stream))
			{
				var directoryList = reader.ReadToEnd();

				if (directoryList.Contains("<a")) // this means that the response is in HTML format
				{
					// Get file names from the HTML response
					files = ParseHtmlListDirectory(directoryList);
				}
				else
				{
					files = directoryList
						.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
						.ToList();
				}
			}
		}

		return files;
	}
}

public IList<string> Upload(UploadInstructions instructions)
{
	CreateRemotePath(instructions.RemotePath);

	var lstFileNames = new List<string>(instructions.FilesToUpload.Count);

	foreach (var fullFilePath in instructions.FilesToUpload)
	{
		var fileName = Path.GetFileName(fullFilePath);

		var fullRemoteFilePath = Path.Combine(instructions.RemotePath, fileName);

		var request = GetFtpWebRequest(fullRemoteFilePath);

		request.Method = WebRequestMethods.Ftp.UploadFile;

		using (var fs = File.OpenRead(fullFilePath))
		{
			var buffer = new byte[fs.Length];

			fs.Read(buffer, 0, buffer.Length);

			using (var rs = request.GetRequestStream())
			{
				rs.Write(buffer, 0, buffer.Length);

				rs.Flush();
			}
		}

		lstFileNames.Add(fileName);
	}

	return lstFileNames;
}


/// <summary>
/// Create directory on FTP Server if directory not exist.
/// </summary>
public void CreateRemotePath(string directoryName)
{
	try
	{
		if (DoesRemotePathExists(directoryName, false)) return;

		//split the remote path directory and check each folder exist
		var dirArray = directoryName.Split('/');

		var dirPath = string.Empty;

		foreach (var dir in dirArray)
		{
			if (string.IsNullOrWhiteSpace(dir) || dir.Contains("/")) continue;

			dirPath = Path.Combine(dirPath, dir);

			//TODO: This is VERY inefficient!
			if (!DoesRemotePathExists(dirPath, false))
			{
				DoesRemotePathExists(dirPath, true);
			}
		}
	}
	catch
	{
		//TODO: Need to handle this better
	}
}

/// <summary>
/// Create directory on server or check directory exists or not on FTP server
/// </summary>
private bool DoesRemotePathExists(string remotePath, bool isMakeDirectory)
{
	var isExists = true;

	try
	{
		var request = GetFtpWebRequest(remotePath);

		request.Method = isMakeDirectory
			? WebRequestMethods.Ftp.MakeDirectory
			: WebRequestMethods.Ftp.ListDirectory;

		using (request.GetResponse())
		{
			//Execute only
		}
	}
	catch
	{
		isExists = false;
	}

	return isExists;
}

private FtpWebResponse GetFtpWebResponse(FtpWebRequest request)
{
	try
	{
		//This can fail if the URL is bogus, UN and/or PW are no good
		var response = (FtpWebResponse)request.GetResponse();

		return response;
	}
	catch (WebException wex)
	{
		throw;
	}
}

/// <summary>
///     Get the ftpServerDetails if file names from the HTML response , received from FTP
/// </summary>
/// <param name="htmlString"></param>
/// <returns></returns>
private List<string> ParseHtmlListDirectory(string htmlString)
{
	var r = new Regex("<a .*?>(.*?)</a>");

	var matches = r.Matches(htmlString);

	var lst = new List<string>();

	foreach (Match match in matches)
	{
		lst.Add(match.Groups[1].Value);
	}

	return lst;
}

private List<string> GetAllChildDirectoryListing(string remotePath)
{
	var request = GetFtpWebRequest(remotePath);

	request.Method = WebRequestMethods.Ftp.ListDirectory;

	using (var response = GetFtpWebResponse(request))
	{
		var files = new List<string>();

		using (var stream = response.GetResponseStream())
		{
			if (stream == null) return files;

			using (var reader = new StreamReader(stream))
			{
				var directoryList = reader.ReadToEnd();

				if (directoryList.Contains("<a")) // this means that the response is in HTML format
				{
					// Get file names from the HTML response
					files = ParseHtmlListDirectory(directoryList);
				}
				else
				{
					files = directoryList
						.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
						.ToList();
				}
			}
		}

		return files;
	}
}

public class UploadInstructions
{
	public string RemotePath { get; set; }

	public IList<string> FilesToUpload { get; set; }
}
#endregion

