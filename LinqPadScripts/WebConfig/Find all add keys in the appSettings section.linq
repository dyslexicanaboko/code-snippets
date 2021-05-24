<Query Kind="Program" />

void Main()
{
	var _element = XElement.Load(@"C:\Dev\Program.exe.config");
	
	IEnumerable<XElement> lstAdds = _element.XPathSelectElements("appSettings/*");
	
	lstAdds.Dump();
}
