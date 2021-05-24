<Query Kind="Program" />

private const string DtsxFile = @"C:\Dump\SsisPackage.dtsx";
private const string NS = "www.microsoft.com/SqlServer/Dts"; //Namespace
private const string P = "DTS"; //Prefix
private XmlNamespaceManager _namespaceManager;
private XElement _document;

void Main()
{
	_namespaceManager = new XmlNamespaceManager(new NameTable());
	_namespaceManager.AddNamespace(P, NS);

	_document = XElement.Load(DtsxFile);
	
	var dictVariables = GetVariables();
	
	//dictVariables.Dump();
	
	//At this point easier to search the XML as text because XML makes everything complicated for no reason
	var content = File.ReadAllText(DtsxFile);
	
	var keys = dictVariables.Keys.ToList();
	
	foreach (var k in keys)
	{
		var r = new Regex("User::" + k, RegexOptions.IgnoreCase | RegexOptions.Multiline);
		
		dictVariables[k] = r.Matches(content).Count;
	}

	Console.WriteLine(
$@"
Total variables : {dictVariables.Count}
Found zero times: {dictVariables.Where( x => x.Value == 0).Count()}
Found many times: {dictVariables.Where( x => x.Value > 0).Count()}
"
);
	
	dictVariables.OrderBy(x => x.Value).Dump();
}

private Dictionary<string, int> GetVariables()
{
	//Grab all variables in the XML
	List<XElement> variables = _document.XPathSelectElements("DTS:Variables/*", _namespaceManager).ToList();

	//XName name = XName.Get("{http://www.adventure-works.com}Root");
	//DTS:Namespace="User"
	//DTS:ObjectName="ApplyFacilityRouting"

	var varUserNamespaceValue = "User";
	var varNamespace = XName.Get($"{{{NS}}}Namespace");
	var varName = XName.Get($"{{{NS}}}ObjectName");
	
	//variables.Dump();
	
	var dict = variables
		.Where(x => x.Attribute(varNamespace).Value == varUserNamespaceValue)
		.ToDictionary(k => k.Attribute(varName).Value, v => 0);

	return dict;
}

/*
<DTS:Variables>
    <DTS:Variable
      DTS:Namespace="User"
      DTS:ObjectName="ApplyFacilityRouting">
      <DTS:VariableValue DTS:DataType="11">0</DTS:VariableValue>
    </DTS:Variable>
</DTS:Variables>	
*/


