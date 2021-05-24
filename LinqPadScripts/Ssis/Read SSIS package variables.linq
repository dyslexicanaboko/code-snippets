<Query Kind="Program" />

void Main()
{
	var nodeList = ReadSissPackage(@"C:\Dev\SsisPackage.dtsx");
	
	//nodeList.Dump();
	
	var lst = TranslateXmlToObjects(nodeList);
	
	lst.Dump();
}

public List<DtsVariable> TranslateXmlToObjects(XmlNodeList nodeList)
{
	var lst = new List<DtsVariable>(nodeList.Count);
	
	foreach (XmlNode node in nodeList)
	{
		var v = new DtsVariable
		{
			Namespace = node.Attributes["DTS:Namespace"].Value,
		 	ObjectName = node.Attributes["DTS:ObjectName"].Value
		};

		lst.Add(v);

		if (!node.HasChildNodes) continue;

		var child = node.ChildNodes[0];

		if(child == null) continue;
		
		//The Data Type and Value are a nested node inside of the Variable node
		v.DataType = child.Attributes["DTS:DataType"].Value;
		v.Value = child.InnerText;
	}
	
	return lst;
}

// Define other methods and classes here
public XmlNodeList ReadSissPackage(string path)
{
	var document = new XmlDocument();
	document.Load(path);

	//This namespace is standard in all SSIS packages
	var nsManager = new XmlNamespaceManager(document.NameTable);
	nsManager.AddNamespace("DTS", "www.microsoft.com/SqlServer/Dts");

	//Select all variable nodes from the Variables root node
	var lst = document.SelectNodes("//DTS:Variables/*", nsManager);

	return lst;
}

public class DtsVariable
{
	//Scope of variable
	public string Namespace { get; set; }
	//Variable name
	public string ObjectName { get; set; }
	//Variable's data type
	public string DataType { get; set; }
	//Value of variable
	public string Value { get; set; }
}