<Query Kind="Program">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
</Query>

void Main()
{
	string str = @"C:\Dump\2017.03.20_ScheduleGarbage.xml";
	
	XmlDocument doc = new XmlDocument();
	doc.Load(@"C:\Dump\2017.03.20_ScheduleGarbage.xml");
	
	string json = JsonConvert.SerializeXmlNode(doc);

	File.WriteAllText(str + ".json", json);
}

// Define other methods and classes here
