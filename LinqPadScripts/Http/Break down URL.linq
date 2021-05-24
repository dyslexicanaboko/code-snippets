<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Collections.Concurrent.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Collections.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.dll</Reference>
</Query>

void Main()
{
	BreakDownUrl("http://website.net/Path/Page.aspx?A=0&B=&C=1&D=0&E=False&F=");
}

// Define other methods and classes here
public void BreakDownUrl(string url)
{
	var uri = new Uri(url);
	
	//uri.Segments.Dump();
	Console.WriteLine("Division: {0}", uri.Segments[2]);
	
	//uri.Query.Dump();
	
	var colQsp = System.Web.HttpUtility.ParseQueryString(uri.Query);
	
	//colQsp.GetType().Dump();
	var lst = new List<Kvp>();
	
	foreach(string key in colQsp.AllKeys)
		lst.Add(new Kvp() { Key = key, Value = colQsp[key] });
	
	lst.Dump();
	//Console.WriteLine("{0} = [{1}]", key, colQsp[key]);
}

public class Kvp
{
	public string Key { get; set; }
	public string Value { get; set; }
}