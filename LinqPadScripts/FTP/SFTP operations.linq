<Query Kind="Program">
  <NuGetReference>SSH.NET</NuGetReference>
  <Namespace>Renci.SshNet</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

void Main()
{
	Console.WriteLine($"Last executed: {DateTime.Now.ToString()}");

	//SftpUploadFile(@"C:\FileToUpload.txt", "/RemotePath/");
	
//	MakeCopiesForRetest(
//		@"C:\Dump\TestFile.txt",
//		@"C:\CopyTo",
//		"TestFileName",
//		"txt",
//		5);

	//FtpLongList();
	FtpDebug();
	
	Console.WriteLine("Finished");
}

public void MakeCopiesForRetest(string sourceFile, string destinationPath, string saveAsNameOnly, string extension, int copies)
{
	for (int i = 0; i < copies; i++)
	{
		var saveAs = Path.Combine(destinationPath, saveAsNameOnly + i + "." + extension);

		File.Copy(sourceFile, saveAs, true);
	}
}

#region FTP
public void FtpLongList()
{
	var request = GetFtpClient();

	request.Method = WebRequestMethods.Ftp.ListDirectory;

	using(var response = GetResponse(request))
	{
		ReadResponse(response);
	}
}

public void FtpDebug()
{
	var request = GetFtpClient();

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
public FtpWebRequest GetFtpClient()
{
	var builder = new UriBuilder("ftp", "ServerA");

	builder.Uri.Dump();
	builder.Path = "/";

	var request = (FtpWebRequest)WebRequest.Create(builder.Uri);
	request.Credentials = new NetworkCredential("username", "password");

	return request;
}
#endregion

#region SFTP
public void SftpUploadFile(string fullFilePath, string remotePath, string remoteFileName = null)
{
	var uploadFileName = Path.Combine(remotePath, (remoteFileName == null ? Path.GetFileName(fullFilePath) : remoteFileName));

	Console.WriteLine($"Uploading: {fullFilePath}\nto\n{uploadFileName}\n");

	using (var client = GetSftpClient())
	{
		client.Connect();

		using (var fs = new FileStream(fullFilePath, FileMode.Open))
		{
			var p = new ActionProgress((ulong)fs.Length, 100);

			Console.WriteLine($"File size: {fs.Length:n0} bytes\n");
			
			client.BufferSize = 4 * 1024; // bypass Payload error large files 

			client.UploadFile(fs, uploadFileName, true, p.CallBack);
			
			Console.WriteLine("\n\nUpload complete");
		}

		client.Disconnect();
	}
}

public void SftpUploadFiles(string path, string filePattern, string remotePath)
{
	var files = new DirectoryInfo(path).GetFiles(filePattern).Select(x => x.FullName).ToList();

	var sb = new StringBuilder();

	Console.WriteLine($"Uploading {files.Count} files");

	using (var client = GetSftpClient())
	{
		client.Connect();
		
		foreach (var f in files)
		{
			var remoteFullPath = Path.Combine(remotePath, Path.GetFileName(f));
	
			sb.Append($"Uploading: {f} to {remoteFullPath} ");
	
			using (var fs = new FileStream(f, FileMode.Open))
			{
				var p = new ActionProgress((ulong)fs.Length, 100);

				sb.AppendLine($"File size: {fs.Length:n0} bytes");

				client.BufferSize = 4 * 1024; // bypass Payload error large files 

				client.UploadFile(fs, remoteFullPath, true, p.CallBack);
			}
		}

		client.Disconnect();
	}
	
	var console = sb.ToString();
	
	Console.WriteLine("\n\n" + console);
}

public SftpClient GetSftpClient()
{
	//Username and password are case sensitive
	var client = new SftpClient("ServerA", 22, "username", "password");
	
	return client;
}
#endregion

public class ActionProgress
{
	public ActionProgress(ulong fileSizeInBytes, ulong progressResolution)
	{
		_fileSizeInBytes = fileSizeInBytes;
		
		_progressResolution = progressResolution;
		
		_increment = _fileSizeInBytes / _progressResolution;
		
		_nextSize = _increment;
	}
	
	private ulong _fileSizeInBytes;
	
	private ulong _progressResolution;

	private ulong _increment;
	
	private ulong _nextSize;

	public void CallBack(ulong totalBytesUploaded)
	{
		if (totalBytesUploaded >= _nextSize)
		{
			_nextSize += _increment;

			Console.Write("|");
		}
	}
}