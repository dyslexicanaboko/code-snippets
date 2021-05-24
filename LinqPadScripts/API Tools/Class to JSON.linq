<Query Kind="Program">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
</Query>

void Main()
{
	var obj = new RequestModel
	{
		DeptId = 1,
		StartTime = D("2020-09-02"),
		EndTime = D("2020-09-04")
	};
	
	JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented).Dump();
}

public DateTime D(string dateString)
{
	var dtm = Convert.ToDateTime(dateString);
	
	return dtm;
}

//Place classes to serialize here
public class RequestModel
{
	public int DeptId { get; set; }
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }
}
