<Query Kind="Program" />

void Main()
{
	//NumberBeforeLineWithTemplateFileSource(
	//@"C:\Dev\TargetFile.sql",
	//4,
	//"\t-- {0}",
	//0).Dump();

	NumberBeforeLineWithTemplateStringSource(
	_data,
	0,
	"// {0}",
	0).Dump();
}

//Put number at the end of the line
private string NumberBeforeLineWithTemplateFileSource(string fullFilePath, int skip, string template, int startingNumber = 0)
{
	var lines = File.ReadAllLines(fullFilePath).Skip(skip).ToArray();

	return NumberBeforeLineWithtemplate(lines, template);
}

private string NumberBeforeLineWithTemplateStringSource(string stringData, int skip, string template, int startingNumber = 0)
{
	var lines = stringData.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Skip(skip).ToArray();

	return NumberBeforeLineWithtemplate(lines, template);
}

private string NumberBeforeLineWithtemplate(string[] lines, string template)
{
	var sb = new StringBuilder();

	for (int i = 0; i < lines.Length; i++)
	{
		sb.AppendLine(string.Format(template, i))
		  .AppendLine(lines[i]);
	}

	var content = sb.ToString();

	return content;
}

private string _data =
@".Append(SafeGet(() => { return Row.PropertyA; }, () => { return Row.PropertyA_IsNull; }, ref i)).Append(P)
.Append(SafeGet(() => { return Row.PropertyB; }, () => { return Row.PropertyB_IsNull; }, ref i)).Append(P)
.Append(SafeGet(() => { return Row.PropertyC; }, () => { return Row.PropertyC_IsNull; }, ref i)).Append(P);";