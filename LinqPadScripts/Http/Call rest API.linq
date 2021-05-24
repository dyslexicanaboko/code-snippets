<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Security</Namespace>
</Query>

void Main()
{
	//https://loggingframework.api.hws.app.medcity.net/swagger/index.html
	CallRestApi("https://api.net/", "{ \"application\": \"string\", \"logged\": \"2019-09-16T14:42:18.144Z\", \"level\": \"string\", \"message\": \"string\", \"userName\": \"string\", \"serverName\": \"string\", \"port\": \"string\", \"url\": \"string\", \"https\": true, \"serverAddress\": \"string\", \"remoteAddress\": \"string\", \"logger\": \"string\", \"callsite\": \"string\", \"exception\": \"string\" }");
}

private void CallRestApi(string url, string json)
{
	try
	{
		using (var client = new HttpClient())
		{
			try
			{
				//Change SSL checks so that all checks pass
				ServicePointManager.ServerCertificateValidationCallback =
					new RemoteCertificateValidationCallback(
						delegate
						{ return true; }
					);
			}
			catch
			{
			
			}
			
			client.BaseAddress = new Uri(url);
			client
				.DefaultRequestHeaders
				.Accept
				.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			var content = new StringContent(json, Encoding.UTF8, "text/json");

			var result = client.PostAsync("/api/v1/RouteName", content).Result;

			result.Dump();
		}
	}
	catch
	{
		
		throw;
	}
}